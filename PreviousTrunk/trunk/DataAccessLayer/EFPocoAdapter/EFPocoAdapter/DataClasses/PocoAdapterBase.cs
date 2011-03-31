// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Text;
using System.Collections;
using EFPocoAdapter.Internal;
using System.Data.Objects;

namespace EFPocoAdapter.DataClasses
{
    public abstract class PocoAdapterBase<TPocoClass> : EntityObject, IPocoAdapter, IPocoAdapter<TPocoClass>
        where TPocoClass : class
    {
        [CLSCompliant(false)]
        protected TPocoClass _pocoEntity;
        private EFPocoContext _context;
        private byte _flags;

        protected bool IsDetectingChanges
        {
            get { return (_flags & 0x01) != 0; }
            set { _flags = (byte)((_flags & ~0x01) | (value ? 0x01 : 0x00)); }
        }

        private bool IsPopulating
        {
            get { return (_flags & 0x02) != 0; }
            set { _flags = (byte)((_flags & ~0x02) | (value ? 0x02 : 0x00)); }
        }

        protected PocoAdapterBase() : this(null)
        {
        }

        protected PocoAdapterBase(TPocoClass pocoObject)
        {
            _context = ThreadLocalContext.Current;
            bool allowProxies = false;
            if (_context != null)
            {
                allowProxies = _context.EnableChangeTrackingUsingProxies;
            }
            _pocoEntity = pocoObject ?? (TPocoClass)(allowProxies ? CreatePocoEntityProxy() : CreatePocoEntity());
            Init();
            InitCollections(allowProxies);
            RegisterAdapterInContext();
        }

        protected void RegisterAdapterInContext()
        {
            if (_context != null && _pocoEntity != null)
                _context.RegisterAdapter(this, _pocoEntity);
        }

        public EFPocoContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        object IPocoAdapter.PocoEntity
        {
            get { return PocoEntity; }
            set { PocoEntity = (TPocoClass)value; }
        }

        #region IPocoAdapter<TPocoClass> Members

        public TPocoClass PocoEntity
        {
            get {
                if (_pocoEntity == null && !IsPopulating)
                {
                    IsPopulating = true;
                    PopulatePocoEntity(Context != null && Context.EnableChangeTrackingUsingProxies);
                    IsPopulating = false;
                }
                return (TPocoClass)_pocoEntity;
            }
            protected set { _pocoEntity = value; }
        }

        #endregion

        public abstract object CreatePocoEntity();
        public abstract object CreatePocoEntityProxy();
        public virtual void PopulatePocoEntity(bool enableProxies)
        {
        }

        public virtual void Init()
        {
        }

        public virtual void InitCollections(bool enableProxies)
        {
        }

        public virtual void DetectChanges()
        {
        }

        protected void DetectChanges<TValue>(TValue? pocoValue, ref TValue? adapterValue, string propertyName)
            where TValue : struct, IEquatable<TValue>
        {
            if (!adapterValue.HasValue)
            {
                if (pocoValue.HasValue)
                {
                    Context.RaiseChangeDetected(_pocoEntity, propertyName, null, pocoValue.Value);
                    ReportPropertyChanging(propertyName);
                    adapterValue = pocoValue;
                    ReportPropertyChanged(propertyName);
                }
            }
            else if (!pocoValue.HasValue)
            {
                Context.RaiseChangeDetected(_pocoEntity, propertyName, adapterValue.Value, null);
                ReportPropertyChanging(propertyName);
                adapterValue = null;
                ReportPropertyChanged(propertyName);
            }
            else if (!adapterValue.Value.Equals(pocoValue.Value))
            {
                Context.RaiseChangeDetected(_pocoEntity, propertyName, adapterValue.Value, pocoValue.Value);
                ReportPropertyChanging(propertyName);
                adapterValue = pocoValue.Value;
                ReportPropertyChanged(propertyName);
            }
        }

        protected void DetectChanges<TValue>(TValue pocoValue, ref TValue adapterValue, string propertyName)
            where TValue : struct, IEquatable<TValue>
        {
            if (!adapterValue.Equals(pocoValue))
            {
                Context.RaiseChangeDetected(_pocoEntity, propertyName, adapterValue, pocoValue);
                ReportPropertyChanging(propertyName);
                adapterValue = pocoValue;
                ReportPropertyChanged(propertyName);
            }
        }

        protected void DetectChanges(string pocoValue, ref string adapterValue, string propertyName)
        {
            if (adapterValue != pocoValue)
            {
                Context.RaiseChangeDetected(_pocoEntity, propertyName, adapterValue, pocoValue);
                ReportPropertyChanging(propertyName);
                adapterValue = pocoValue;
                ReportPropertyChanged(propertyName);
            }
        }

        protected void DetectChanges(byte[] pocoValue, ref byte[] adapterValue, string propertyName)
        {
#if COMPARE_ARRAY_CONTENTS
            if (!CompareArrays(adapterValue, pocoValue))
#else
            if (!Object.ReferenceEquals(adapterValue, pocoValue))
#endif
            {
                Context.RaiseChangeDetected(_pocoEntity, propertyName, adapterValue, pocoValue);
                ReportPropertyChanging(propertyName);
                adapterValue = pocoValue;
                ReportPropertyChanged(propertyName);
            }
        }

#if COMPARE_ARRAY_CONTENTS
        private bool CompareArrays(byte[] a1, byte[] a2)
        {
            if (a1 == null)
            {
                return (a2 == null) ? true : false;
            }
            else
            {
                if (a2 == null)
                    return false;

                if (a1.Length != a2.Length)
                    return false;

                for (int i = 0; i < a1.Length; ++i)
                    if (a1[i] != a2[i])
                        return false;
                return true;

            }
        }
#endif

        protected void DetectChanges<TAdapterType, TValue>(ICollection<TValue> pocoValues, EntityCollection<TAdapterType> adapterValue, string propertyName)
            where TAdapterType : class, IEntityWithKey, IEntityWithRelationships, IPocoAdapter<TValue>, new()
            where TValue : class
        {
            bool oldDetectingChanges = IsDetectingChanges;
            IsDetectingChanges = true;
            try
            {
                ILazyLoadable odl = pocoValues as ILazyLoadable;
                if (odl != null && !odl.IsLoaded)
                    return;

                EntityCollection<TAdapterType> baseline = adapterValue;
                if (!baseline.IsLoaded && (pocoValues == null || pocoValues.Count == 0))
                    return;
                ICollection<TValue> pocoValuesSet;
                if (pocoValues.Count > 100)
                    pocoValuesSet = new HashSet<TValue>(pocoValues);
                else
                    pocoValuesSet = new OptimizedSetForChangeDetection<TValue>(pocoValues);

                List<object> addedObjects = null;
                List<object> removedObjects = null;
                List<TAdapterType> adaptersToRemove = null;

                foreach (TAdapterType at in baseline)
                {
                    TValue po = at.PocoEntity;

                    if (!pocoValuesSet.Remove(po))
                    {
                        if (adaptersToRemove == null)
                        {
                            adaptersToRemove = new List<TAdapterType>();
                            removedObjects = new List<object>();
                        }
                        removedObjects.Add(po);
                        adaptersToRemove.Add(at);
                    }
                }
                if (adaptersToRemove != null)
                {
                    foreach (TAdapterType ad in adaptersToRemove)
                    {
                        baseline.Remove(ad);
                    }
                }
                // wrap and add remaining items to the collection
                foreach (TValue po in pocoValuesSet)
                {
                    if (addedObjects == null)
                    {
                        addedObjects = new List<object>();
                    }
                    addedObjects.Add(po);
                    baseline.Add(Context.GetAdapterObject<TAdapterType>(po));
                }
                if (addedObjects != null || removedObjects != null)
                {
                    Context.RaiseCollectionChangeDetected(_pocoEntity, propertyName, addedObjects ?? new List<object>(), removedObjects ?? new List<object>());
                }
            }
            finally
            {
                IsDetectingChanges = oldDetectingChanges;
            }
        }

        protected void DetectChanges<TAdapterType, TValue>(TValue pocoValue, EntityReference<TAdapterType> adapterValue, string propertyName)
            where TAdapterType : class, IEntityWithKey, IEntityWithRelationships, IPocoAdapter<TValue>, new()
            where TValue : class
        {
            bool oldDetectingChanges = IsDetectingChanges;
            IsDetectingChanges = true;
            try
            {
                if (adapterValue.Value == null)
                {
                    if (pocoValue == null)
                    {
                        return;
                    }
                    else
                    {
                        TAdapterType newValue = Context.GetAdapterObject<TAdapterType>(pocoValue);
                        Context.RaiseChangeDetected(_pocoEntity, propertyName, null, pocoValue);
                        adapterValue.Value = newValue;
                    }
                }
                else if (pocoValue == null)
                {
                    Context.RaiseChangeDetected(_pocoEntity, propertyName, adapterValue.Value.PocoEntity, null);
                    adapterValue.Value = null;
                }
                else if (!Object.ReferenceEquals(adapterValue.Value.PocoEntity, pocoValue))
                {
                    TAdapterType newValue = Context.GetAdapterObject<TAdapterType>(pocoValue);
                    TValue myPocoValue = adapterValue.Value.PocoEntity;
                    Context.RaiseChangeDetected(_pocoEntity, propertyName, myPocoValue, pocoValue);
                    adapterValue.Value = newValue;
                }
            }
            finally
            {
                IsDetectingChanges = oldDetectingChanges;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (EntityKey != null)
            {
                sb.Append(EntityKey.EntitySetName);
                sb.Append("[");
                string separator = "";
                if (EntityKey.EntityKeyValues != null)
                {
                    foreach (var km in EntityKey.EntityKeyValues)
                    {
                        sb.Append(separator);
                        sb.Append(km.Key);
                        sb.Append("=");
                        sb.Append(km.Value);
                        separator = ",";
                    }
                }
                sb.Append("]");
            }
            else
            {
                sb.Append(this.GetType().Name);
                sb.Append("[");
                sb.Append("EntityKey==null");
                sb.Append("]");
            }
            return sb.ToString();
        }

        protected void UpdateCollection<TPocoEntity,TAdapterObject>(ICollection<TPocoEntity> pocoCollection, EntityCollection<TAdapterObject> adapterCollection) 
            where TAdapterObject : class, IEntityWithRelationships, IPocoAdapter<TPocoEntity>
        {
            ILazyLoadable odl = pocoCollection as ILazyLoadable;
            if (odl == null)
            {
                pocoCollection.Clear();
                foreach (TAdapterObject ao in adapterCollection)
                    pocoCollection.Add(ao.PocoEntity);
            }
        }

        protected EntityCollection<TTargetEntity> GetRelatedCollection<TTargetEntity>(string relationshipName, string targetRoleName)
            where TTargetEntity : class, IEntityWithRelationships

        {
            return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<TTargetEntity>(relationshipName, targetRoleName);
        }

        protected EntityReference<TTargetEntity> GetRelatedReference<TTargetEntity>(string relationshipName, string targetRoleName)
            where TTargetEntity : class, IEntityWithRelationships
        {
            return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<TTargetEntity>(relationshipName, targetRoleName);
        }

        public void ConvertToProxy()
        {
            _pocoEntity = (TPocoClass)CreatePocoEntityProxy();
        }

        void IPocoAdapter.RaiseChangeDetected(string member, object oldValue, object newValue)
        {
            Context.RaiseChangeDetected(this, member, oldValue, newValue);
        }

        protected T ConvertTo<T,R>(IEnumerable collection)
            where T : ICollection<R>, new()
        {
            T result = new T();
            foreach (IPocoAdapter<R> r in collection)
            {
                result.Add(r.PocoEntity);
            }
            return result;
        }

        public bool CanLoadProperty(string propertyName)
        {
            ObjectStateEntry ose;

            if (this.Context == null)
                return false;

            if (!this.Context.DeferredLoadingEnabled)
                return false;

            if (this.Context.IsDisposed)
                return false;

            if (!this.Context.PersistenceAwareContext.ObjectStateManager.TryGetObjectStateEntry(this, out ose))
                return false;

            if (ose.State == EntityState.Unchanged || ose.State == EntityState.Modified)
                return true;
            else
                return false;
        }
    }
}
