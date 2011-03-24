// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;

namespace EFPocoAdapter.DataClasses
{
    public static class ThreadLocalContext
    {
        [ThreadStatic]
        private static EFPocoContext _current;

        public static EFPocoContext Current
        {
            get { return _current; }
            internal set { _current = value; }
        }

        public static IDisposable Set(EFPocoContext context)
        {
            EFPocoContext oldContext = Current;
            Current = context;
            return new Cleanup(oldContext);
        }

        private class Cleanup : IDisposable
        {
            private EFPocoContext _oldContext;

            public Cleanup(EFPocoContext oldContext)
            {
                _oldContext = oldContext;
            }

            #region IDisposable Members

            public void Dispose()
            {
                ThreadLocalContext.Current = _oldContext;
                GC.SuppressFinalize(this);
            }

            #endregion
        }
    }
}
