// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;

namespace EFPocoAdapter.DataClasses
{
    public class IListEntityCollectionAdapter<TPocoEntity, TAdapterObject> : IList<TPocoEntity>, ILazyLoadable 
        where TAdapterObject : class, IEntityWithRelationships, IPocoAdapter<TPocoEntity>
    {
        private EntityCollection<TAdapterObject> _collection;
        private EFPocoContext _context;
        private IPocoAdapter _parentEntityAdapter;
        private string _memberName;

        public IListEntityCollectionAdapter(IPocoAdapter parentEntityAdapter, string memberName, IEnumerable<TAdapterObject> collection, EFPocoContext context)
        {
            _collection = (EntityCollection<TAdapterObject>)collection;
            _context = context;
            _parentEntityAdapter = parentEntityAdapter;
            _memberName = memberName;
        }

        #region IList<TPocoEntity> Members

        public int IndexOf(TPocoEntity item)
        {
            int pos = 0;

            foreach (TPocoEntity po in this)
            {
                if (Object.ReferenceEquals(po, item))
                    return pos;
                pos++;
            }
            return -1;
        }

        public void Insert(int index, TPocoEntity item)
        {
            // ignore the index
            Add(item);
        }

        public void RemoveAt(int index)
        {
            Remove(this[index]);
        }

        public TPocoEntity this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index");

                int pos = 0;

                foreach (TAdapterObject ao in _collection)
                {
                    if (pos == index)
                        return ao.PocoEntity;
                    pos++;
                }
                throw new InvalidOperationException("Should not be reached.");
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException("index");

                int pos = 0;

                foreach (TAdapterObject ao in _collection)
                {
                    if (pos == index)
                    {
                        if (Object.ReferenceEquals(ao.PocoEntity, value))
                            return;

                        _collection.Remove(ao);
                        _collection.Add(_context.GetAdapterObject<TAdapterObject>(value));
                        return;
                    }
                    pos++;
                }
            }
        }

        #endregion

        #region ICollection<TPocoEntity> Members

        public void Add(TPocoEntity item)
        {
            EnsureDataLoaded();
            _collection.Add(_context.GetAdapterObject<TAdapterObject>(item));
            _context.RaiseCollectionChangeDetected(_parentEntityAdapter.PocoEntity, _memberName, new object[] { item }, new object[0]);
        }

        public void Clear()
        {
            EnsureDataLoaded();
            if (_collection.Count > 0)
            {
                _context.RaiseCollectionChangeDetected(_parentEntityAdapter.PocoEntity, _memberName, new object[0], this.Cast<object>().ToArray() );
            }
            _collection.Clear();
        }

        public bool Contains(TPocoEntity item)
        {
            EnsureDataLoaded();
            return _collection.Contains(_context.GetAdapterObject<TAdapterObject>(item));
        }

        public void CopyTo(TPocoEntity[] array, int arrayIndex)
        {
            foreach (TPocoEntity po in this)
            {
                array[arrayIndex++] = po;
            }
        }

        public int Count
        {
            get
            {
                EnsureDataLoaded();
                return _collection.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return _collection.IsReadOnly; }
        }

        public bool Remove(TPocoEntity item)
        {
            EnsureDataLoaded();
            bool ok = _collection.Remove(_context.GetAdapterObject<TAdapterObject>(item));
            if (ok)
                _context.RaiseCollectionChangeDetected(_parentEntityAdapter.PocoEntity, _memberName, new object[0], new object[] { item });
            return ok;
        }

        #endregion

        #region IEnumerable<TPocoEntity> Members

        public IEnumerator<TPocoEntity> GetEnumerator()
        {
            EnsureDataLoaded();
            return _collection.Select(e => e.PocoEntity).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private void EnsureDataLoaded()
        {
            if (!_collection.IsLoaded && _context.DeferredLoadingEnabled && _parentEntityAdapter.CanLoadProperty(_memberName))
            {
                using (ThreadLocalContext.Set(_context))
                {
                    _collection.Load();
                }
            }
        }

        public bool IsLoaded
        {
            get { return _collection.IsLoaded; }
        }

        public void Load()
        {
            EnsureDataLoaded();
        }
    }
}
