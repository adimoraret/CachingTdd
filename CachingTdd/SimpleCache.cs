using System;
using System.Collections.Generic;

namespace CachingTdd
{
    public class SimpleCache<T>
    {
        private object _locker = new object();
        private IDictionary<string, T> _cachedValues = new Dictionary<string, T>();

        public T Get(string key, Func<string, T> getNewValue)
        {
            lock (_locker)
            {
                if (!_cachedValues.ContainsKey(key))
                {
                    var value = getNewValue(key);
                    _cachedValues.Add(key, value);
                }
                return _cachedValues[key];
            }
        }
    }
}