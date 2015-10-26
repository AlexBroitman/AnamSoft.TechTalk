using System;
using System.Collections;
using System.Collections.Generic;

namespace CacheEnumerable
{
    public static class CacheEnumerableExtension
    {
        public static IEnumerable<T> AsCacheEnumerable<T>(this IEnumerable<T> sequence)
        {
            return sequence is IList<T> || sequence is ICollection<T> || sequence is CacheEnumerable<T> || sequence is Array
                ? sequence
                : new CacheEnumerable<T>(sequence);
        }
    }
    public class CacheEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        private IEnumerator<T> _sourceEnumerator;
        private List<T> _cache;
        private readonly object _lock = new object();
        public CacheEnumerable(IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            _source = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_sourceEnumerator != null)
                return new CacheEnumerator(this);
            _cache = new List<T>();
            _sourceEnumerator = _source.GetEnumerator();
            return new CacheEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void InvalidateCache()
        {
            _sourceEnumerator = null;
            _cache = null;
        }

        public int CachedItems
        {
            get { return _cache == null ? 0 : _cache.Count; } 
        }

        private class CacheEnumerator : IEnumerator<T>
        {
            private int _index = -1;
            private readonly CacheEnumerable<T> _sequence;
            public CacheEnumerator(CacheEnumerable<T> sequence)
            {
                if (sequence == null)
                    throw new ArgumentNullException("sequence");

                _sequence = sequence;
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                _index++;
                if (_index < _sequence._cache.Count)
                    return true;
                lock (_sequence._lock)
                {
                    if (_index < _sequence._cache.Count)
                        return true;
                    if (!_sequence._sourceEnumerator.MoveNext())
                        return false;
                    _sequence._cache.Add(_sequence._sourceEnumerator.Current);
                    return true;
                }
            }

            public T Current
            {
                get
                {
                    if (_index < 0 || _index >= _sequence._cache.Count)
                        throw new InvalidOperationException();

                    return _sequence._cache[_index];
                }
            }

            public void Reset()
            {
                _index = -1;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}
