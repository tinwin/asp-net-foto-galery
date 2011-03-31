// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Data.Objects;
using EFPocoAdapter.DataClasses;

namespace EFPocoAdapter.Internal
{
    internal class PocoEntitySet<T> : EFPocoQuery<T>, IEntitySet<T>
    {
        private string _setName;
        private string _containerName;

        internal PocoEntitySet(EFPocoContext context, ObjectQuery query, string containerName, string setName) : base(context, query)
        {
            _setName = setName;
            _containerName = containerName;
        }

        #region IEntitySet<T> Members

        public void InsertOnSaveChanges(T entity)
        {
            Context.PersistenceAwareContext.AddObject(_setName, Context.GetAdapterObject(entity));
        }

        public void DeleteOnSaveChanges(T entity)
        {
            Context.PersistenceAwareContext.DeleteObject(Context.GetAdapterObject(entity));
        }

        public void Attach(T entity)
        {
            Context.PersistenceAwareContext.AttachTo(_setName, Context.GetAdapterObject(entity));
        }

        public void Detach(T entity)
        {
            var adapter = Context.GetAdapterObject(entity);
            Context.PersistenceAwareContext.Detach(adapter);
            Context.UnregisterAdapter(entity, adapter);
        }

        public void ApplyPropertyChanges(T entity)
        {
            Context.PersistenceAwareContext.ApplyPropertyChanges(_setName, Context.GetAdapterObject(entity));
        }

        public string EntitySetName
        {
            get { return _setName; }
        }

        public string EntityContainerName
        {
            get { return _containerName; }
        }

        #endregion

        public override string ToString()
        {
            return _setName;
        }
    }
}
