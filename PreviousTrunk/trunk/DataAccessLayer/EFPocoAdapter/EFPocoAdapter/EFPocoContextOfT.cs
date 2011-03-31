// Copyright (c) Microsoft Corporation.  All rights reserved.

using System.Data.Objects;
using EFPocoAdapter.DataClasses;

namespace EFPocoAdapter
{
    public class EFPocoContext<TAdapterContext> : EFPocoContext
        where TAdapterContext : ObjectContext, IPocoAdapterObjectContext
    {
        public TAdapterContext WrappedContext
        {
            get { return (TAdapterContext)PersistenceAwareContext; }
        }

        public EFPocoContext(TAdapterContext adapterContext)
            : base(adapterContext)
        {
        }
    }
}
