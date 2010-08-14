using System;
using System.Collections;
using System.Collections.Generic;

namespace GeneticProgramming.Utility
{
    public class ThreadSafeDictionary<TKey, TValue> : IDictionary<TKey,TValue>
    {
        #region Implementation of IEnumerable

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (this.accessLock)
            {
                return this.dictionary.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<KeyValuePair<TKey,TValue>>

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            lock (this.accessLock)
            {
                this.dictionary.Add(item);
            }
        }

        public void Clear()
        {
            lock (this.accessLock)
            {
                this.dictionary.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (this.accessLock)
            {
                return this.dictionary.Contains(item);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock(this.accessLock)
            {
                this.dictionary.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (this.accessLock)
            {
                return this.dictionary.Remove(item);
            }
        }

        public int Count
        {
            get
            {
                lock (this.accessLock)
                {
                    return this.dictionary.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                lock (this.accessLock)
                {
                    return this.dictionary.IsReadOnly;
                }
            }
        }

        #endregion

        #region Implementation of IDictionary<TKey,TValue>

        public bool ContainsKey(TKey key)
        {
            lock (this.accessLock)
            {
                return this.dictionary.ContainsKey(key);
            }
        }

        public void Add(TKey key, TValue value)
        {
            lock (this.accessLock)
            {
                this.dictionary.Add(key, value);
            }
        }

        public bool Remove(TKey key)
        {
            lock (this.accessLock)
            {
                return this.dictionary.Remove(key);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (this.accessLock)
            {
                return this.dictionary.TryGetValue(key, out value);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (this.accessLock)
                {
                    return this.dictionary[key];
                }
            }
            set
            {
                lock (this.accessLock)
                {
                    this.dictionary[key] = value;
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                lock (this.accessLock)
                {
                    return this.dictionary.Keys;
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                lock (this.accessLock)
                {
                    return this.dictionary.Values;
                }
            }
        }

        #endregion

        #region Private fields

        private object accessLock = new object();

        private readonly IDictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        #endregion
    }
}