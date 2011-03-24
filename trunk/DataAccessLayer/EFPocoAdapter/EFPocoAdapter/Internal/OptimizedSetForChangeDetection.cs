using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFPocoAdapter.Internal
{
    // not a full set, only Remove() and iteration is supported
    internal class OptimizedSetForChangeDetection<T> : ICollection<T>
        where T : class
    {
        private T[] _data;
        private int _dataCount;

        public OptimizedSetForChangeDetection(IEnumerable<T> init)
        {
            _data = init.ToArray();
            _dataCount = _data.Length;
        }

        #region ICollection<T> Members

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(T item)
        {
            for (int i = 0; i < _dataCount; ++i)
            {
                if (_data[i] == item)
                {
                    if (i != _dataCount - 1)
                    {
                        _data[i] = _data[_dataCount - 1];
                    }
                    _dataCount--;
                    return true;
                }
            }
            return false;
        }

        #endregion

        internal class SetEnumerator : IEnumerator<T>
        {
            private int _currentPos = 0;
            private int _length = 0;
            private T[] _data;

            public SetEnumerator(OptimizedSetForChangeDetection<T> parent)
            {
                _currentPos = -1;
                _data = parent._data;
                _length = parent._dataCount;
            }

            #region IEnumerator<T> Members

            public T Current
            {
                get { return _data[_currentPos]; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                return ++_currentPos < _length;
            }

            public void Reset()
            {
                _currentPos = -1;
            }

            #endregion
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return new SetEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
