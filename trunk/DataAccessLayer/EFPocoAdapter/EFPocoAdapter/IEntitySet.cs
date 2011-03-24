// Copyright (c) Microsoft Corporation.  All rights reserved.

using System.Linq;

namespace EFPocoAdapter
{
    public interface IEntitySet<T> : IQueryable<T>
    {
        string EntitySetName { get; }
        string EntityContainerName { get; }
        
        void InsertOnSaveChanges(T entity);
        void DeleteOnSaveChanges(T entity);
        void Attach(T entity);
        void Detach(T entity);
        void ApplyPropertyChanges(T entity);
    }
}
