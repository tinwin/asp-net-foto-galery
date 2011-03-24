// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EFPocoAdapter.DataClasses;
using EFPocoAdapter.Internal;

namespace EFPocoAdapter
{
    public class EFPocoContext : IDisposable
    {
        private ObjectContext _persistenceAwareContext;
        private QueryTranslationCache _queryTranslationCache;
        private Dictionary<object, IPocoAdapter> _adapterObjects = new Dictionary<object,IPocoAdapter>();
        private Dictionary<object, IPocoAdapter> _newAdapterObjects;
        private List<IPocoAdapter> _adapterObjectsList = new List<IPocoAdapter>();
        private IQueryProvider _entityFrameworkQueryProvider;
        private IQueryProvider _queryProvider;

        internal bool IsDisposed { get; private set; }

        protected internal ObjectContext PersistenceAwareContext
        {
            get { return _persistenceAwareContext; }
            set
            {
                IPocoAdapterObjectContext paoc = value as IPocoAdapterObjectContext;

                if (paoc == null)
                    throw new ArgumentException("objectContext must implement IPocoAdapterObjectContext");

                _persistenceAwareContext = value;
                _entityFrameworkQueryProvider = ((IQueryable)PersistenceAwareContext.CreateQuery<object>("1")).Provider;
                _queryTranslationCache = paoc.QueryTranslationCache;
                _queryProvider = new EFPocoQueryProvider(this);
            }
        }

        internal IQueryProvider EntityFrameworkQueryProvider
        {
            get { return _entityFrameworkQueryProvider; }
        }

        public IQueryProvider QueryProvider
        {
            get { return _queryProvider; }
        }

        public DbConnection Connection
        {
            get { return PersistenceAwareContext.Connection; }
        }


        public bool EnableChangeTrackingUsingProxies { get; set; }
        public bool DeferredLoadingEnabled { get; set; }

        [Obsolete("Use DeferredLoadingEnabled instead.")]
        public bool EnableLazyLoading
        {
            get { return this.DeferredLoadingEnabled; }
            set { this.DeferredLoadingEnabled = true; }
        }

        public event EventHandler<ChangeDetectedEventArgs> ChangeDetected;
        public event EventHandler<CollectionChangeDetectedEventArgs> CollectionChangeDetected;

        public EFPocoContext()
        {
            EnableChangeTrackingUsingProxies = true;
            DeferredLoadingEnabled = true;
        }

        ~EFPocoContext()
        {
            Dispose(false);
        }

        public EFPocoContext(IPocoAdapterObjectContext pocoAdapterContext)
            : this()
        {
            PersistenceAwareContext = (ObjectContext)pocoAdapterContext;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_persistenceAwareContext != null)
                {
                    _persistenceAwareContext.Dispose();
                    _persistenceAwareContext = null;
                }
                IsDisposed = true;
            }
        }

        public void SaveChanges()
        {
            DetectChanges();
            _persistenceAwareContext.SaveChanges();
        }

        public void DetectChanges()
        {
            for (int i = 0; i < _adapterObjectsList.Count; ++i)
            {
                IPocoAdapter pa = _adapterObjectsList[i];
                pa.DetectChanges();
            }
        }

        private Dictionary<string, object> _entitySets = new Dictionary<string, object>(); 

        protected IEntitySet<T> GetEntitySet<T>(string name)
        {
            object obj;

            if (_entitySets.TryGetValue(name, out obj))
                return (IEntitySet<T>)obj;

            MethodInfo createQueryOfT = typeof(ObjectContext).GetMethod("CreateQuery").MakeGenericMethod(GetAdapterType(typeof(T)));
            ObjectQuery oq = (ObjectQuery)createQueryOfT.Invoke(PersistenceAwareContext, new object[] { name, new ObjectParameter[0] });

            IEntitySet<T> result = new PocoEntitySet<T>(this, oq, this.PersistenceAwareContext.DefaultContainerName, name);
            _entitySets.Add(name, result);
            return result;
        }

        public T GetAdapterObject<T>(object entity)
        {
            return (T)GetAdapterObject(entity);
        }

        public IPocoAdapter GetAdapterObject(object entity)
        {
            return GetAdapterObject(entity, false);
        }

        public IPocoAdapter GetAdapterObject(object entity, bool detached)
        {
            if (entity == null)
                return null;
            
            IPocoAdapter value;

            if (_adapterObjects.TryGetValue(entity, out value))
            {
                return value;
            }

            if (_newAdapterObjects != null)
            {
                if (_newAdapterObjects.TryGetValue(entity, out value))
                    return value;
            }

            Type pocoType;
            IEntityProxy proxy = entity as IEntityProxy;

            if (proxy != null)
            {
                if (proxy.Adapter != null)
                {
                    if (proxy.Adapter.Context != null && !proxy.Adapter.Context.IsDisposed)
                        throw new InvalidOperationException("Entity is already attached with another active context. Cannot reattach.");
                }

                pocoType = entity.GetType().BaseType;
            }
            else
            {
                pocoType = entity.GetType();
            }

            Func<object,IPocoAdapter> adapterCreator = _queryTranslationCache.AdapterCreators[pocoType];
            Debug.Assert(adapterCreator != null); // otherwise it is abstract or non-entity

            IPocoAdapter adapterObject = adapterCreator(entity);
            if (proxy != null)
                proxy.Adapter = adapterObject;
            adapterObject.Context = this;
            if (!detached)
            {
                RegisterAdapter(adapterObject, entity);
            }
            adapterObject.DetectChanges();

            return adapterObject;
        }

        public T CreateObject<T>() 
            where T : class, new()
        {
            using (ThreadLocalContext.Set(this))
            {
                return ((IPocoAdapter<T>)_queryTranslationCache.AdapterCreators[typeof(T)](null)).PocoEntity;
            }
        }

        public bool IsProxy(object entity)
        {
            return entity is IEntityProxy;
        }

        private static string ExpressionToPropertyName<T, T2>(Expression<Func<T, T2>> selector)
        {
            MemberExpression me = selector.Body as MemberExpression;
            if (me == null)
                throw new ArgumentException("MemberException expected.");

            if (me.Expression.NodeType != ExpressionType.Parameter)
                throw new ArgumentException("Invalid lambda expression");

            if (selector.Parameters[0] != me.Expression)
                throw new ArgumentException("Invalid lambda parameter.");

            return me.Member.Name;
        }

        public bool IsPropertyLoaded<T>(T obj, Expression<Func<T, object>> selector)
        {
            return IsPropertyLoaded(obj, ExpressionToPropertyName(selector));
        }

        private IRelatedEnd GetRelatedEnd(object obj, string propertyName)
        {
            IPocoAdapter adapterObject = GetAdapterObject(obj) as IPocoAdapter;
            PropertyInfo pi = adapterObject.GetType().GetProperty(propertyName);
            IRelatedEnd relatedEnd = null;
            if (pi != null)
                relatedEnd = pi.GetValue(adapterObject, null) as IRelatedEnd;
            if (relatedEnd == null)
            {
                pi = adapterObject.GetType().GetProperty(propertyName + "Reference");
                if (pi != null)
                    relatedEnd = pi.GetValue(adapterObject, null) as IRelatedEnd;
            }
            if (relatedEnd == null)
                throw new ArgumentException("Property '" + propertyName + "' is not a navigation property.");
            return relatedEnd;
        }

        public bool IsPropertyLoaded(object entity, string propertyName)
        {
            return GetRelatedEnd(entity, propertyName).IsLoaded;
        }

        public EFPocoQuery<T> CreateQuery<T>(string query, params ObjectParameter[] parameters)
        {
            Type adapterType;

            if (!_queryTranslationCache.TypeMapping.TryGetValue(typeof(T), out adapterType))
                adapterType = typeof(T);
            MethodInfo mi = typeof(ObjectContext).GetMethod("CreateQuery").MakeGenericMethod(adapterType);
            ObjectQuery objectQuery = (ObjectQuery)mi.Invoke(PersistenceAwareContext, new object[] { query, parameters });
            return new EFPocoQuery<T>(this, objectQuery);
        }

        private void Convert(Func<IPocoAdapter, bool> filter, Func<IPocoAdapter, object> creator, bool enableProxies)
        {
            // copy all state from POCO object to adapter objects
            DetectChanges();

            // we may be getting calls in the middle of PopulatePocoEntity()
            bool oldEnableProxies = this.EnableChangeTrackingUsingProxies;
            EnableChangeTrackingUsingProxies = enableProxies;

            for (int i = 0; i < _adapterObjectsList.Count; ++i)
            {
                IPocoAdapter pa = _adapterObjectsList[i];
                pa.PocoEntity = creator(pa);
            }

            for (int i = 0; i < _adapterObjectsList.Count; ++i)
            {
                IPocoAdapter pa = _adapterObjectsList[i];
                pa.InitCollections(enableProxies);
            }
            
            // PopulatePocoEntity may cause calls to RegisterAdapter()
            //
            // this will prevent them from messing up a list of adapters
            //
            _newAdapterObjects = new Dictionary<object, IPocoAdapter>();

            Dictionary<object, IPocoAdapter> newAdapterObjects = new Dictionary<object, IPocoAdapter>();
            for (int i = 0; i < _adapterObjectsList.Count; ++i)
            {
                IPocoAdapter pa = _adapterObjectsList[i];
                pa.PopulatePocoEntity(enableProxies);
                RegisterAdapter(pa, pa.PocoEntity);
            }
            EnableChangeTrackingUsingProxies = oldEnableProxies;
            // adapter object list does not change, only mapping from POCOs to adapters
            // so no need to update _adapterObjectList
            _adapterObjects = _newAdapterObjects;
            _newAdapterObjects = null;
        }

        public void ConvertProxiesToPoco()
        {
            Convert(c => c is IEntityProxy, c => c.CreatePocoEntity(), false);
        }

        public void ConvertPocoToProxies()
        {
            Convert(c => !(c is IEntityProxy), c => c.CreatePocoEntityProxy(), true);
        }

        public void LoadProperty<T>(T entity, Expression<Func<T, object>> selector)
        {
            string propertyName = ExpressionToPropertyName(selector);

            using (ThreadLocalContext.Set(this))
            {
                GetRelatedEnd(entity, propertyName).Load();
            }
        }

        public void LoadProperty<T>(T entity, string propertyName)
        {
            using (ThreadLocalContext.Set(this))
            {
                GetRelatedEnd(entity, propertyName).Load();
            }
        }

        public void RaiseChangeDetected(object entity, string member, object oldValue, object newValue)
        {
            if (ChangeDetected != null)
            {
                ChangeDetected.Invoke(this, new ChangeDetectedEventArgs()
                {
                    Entity = entity,
                    Member = member,
                    OldValue = oldValue,
                    NewValue = newValue,
                });
            }
        }

        internal bool HaveCollectionChangeDetectedListeners
        {
            get { return (CollectionChangeDetected != null); }
        }

        internal void RaiseCollectionChangeDetected(object entity, string member, ICollection<object> addedObjects, ICollection<object> removedObjects)
        {
            if (HaveCollectionChangeDetectedListeners)
            {
                CollectionChangeDetected.Invoke(this, new CollectionChangeDetectedEventArgs()
                {
                    Entity = entity,
                    Member = member,
                    AddedObjects = addedObjects,
                    RemovedObjects = removedObjects,
                });
            }
        }

        internal QueryTranslator CreateUnboundQueryTranslator()
        {
            return new QueryTranslator(this.GetType(), PersistenceAwareContext.GetType(), _queryTranslationCache, null);
        }

        internal QueryTranslator CreateContextBoundQueryTranslator()
        {
            return new QueryTranslator(this.GetType(), PersistenceAwareContext.GetType(), _queryTranslationCache, this);
        }

        internal Type GetAdapterType(Type type)
        {
            Type result;

            if (_queryTranslationCache.TypeMapping.TryGetValue(type, out result))
                return result;
            else
                return type;
        }

        public MetadataWorkspace MetadataWorkspace
        {
            get { return PersistenceAwareContext.MetadataWorkspace; }
        }

        internal void RegisterAdapter(IPocoAdapter pocoAdapter, object pocoEntity)
        {
            // we need special treatment if we're in the middle of POCO<->Proxies conversion
            if (_newAdapterObjects != null)
            {
                if (_newAdapterObjects.ContainsKey(pocoEntity))
                    return;
                _newAdapterObjects.Add(pocoEntity, pocoAdapter);
                return;
            }

            // Console.WriteLine("Registering adapter {0} for poco:{1} adapter: {2}", pocoAdapter, pocoEntity.GetHashCode(), pocoAdapter.GetHashCode());

            if (!_adapterObjects.ContainsKey(pocoEntity))
            {
                _adapterObjects.Add(pocoEntity, pocoAdapter);
                _adapterObjectsList.Add(pocoAdapter);
            }
        }

        internal void UnregisterAdapter(object entity, IPocoAdapter adapter)
        {
            if (_adapterObjects.Remove(entity))
            {
                adapter.Context = null;
            }
        }
    }
}
