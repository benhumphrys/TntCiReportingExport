using System;
using System.Collections;
using System.Collections.Generic;
using Tnt.KofaxCapture.TntCiReportingExportUnitTests.Properties;

namespace Tnt.KofaxCapture.TntCiReportingExportUnitTests
{
    /// <summary>
    /// Collection class that is used to hold Kofax Capture wrapper objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionBase<T> : IEnumerable where T : class 
    {
        private readonly Dictionary<string, T> _items = new Dictionary<string, T>();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            // We can't just return the Dictionary's GetEnumerator() because generic enumerators don't work 100% in COM.
            foreach (var item in _items)
            {
                yield return item.Value;
            }
        }

        /// <summary>
        /// Add a new item to the collection.
        /// </summary>
        /// <param name="item">item to add.</param>
        /// <param name="key">Key to the item.</param>
        public void Add(T item, string key)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (key == null) throw new ArgumentNullException("key");

            _items.Add(key, item);
        }

        /// <summary>
        /// Remove the specified object from the collection.
        /// </summary>
        /// <param name="key">Key of object to remove.</param>
        public void Remove(ref object key)
        {
            var keyString = GetDerivedKey(key);
            
            T value;
            if (_items.TryGetValue(keyString, out value))
            {
                _items.Remove(keyString);
            }
            else
            {
                throw new ArgumentException(string.Format(Resources.ElementWithKeyNotFound, keyString), "key");
            }
        }

        /// <summary>
        /// Derive the key string given the actual key (which for the Kofax API, is an object).
        /// </summary>
        /// <param name="key">Key to derive.</param>
        /// <returns>String key.</returns>
        private string GetDerivedKey(object key)
        {
            var keyString = key as string;

            if (keyString == null && (key is int || key is short || key is byte))
            {
                var keyInt = Convert.ToInt32(key);
                keyInt--;

                if (keyInt >= 0 && keyInt < _items.Values.Count)
                {
                    var keys = new string[_items.Keys.Count];
                    _items.Keys.CopyTo(keys, 0);
                    keyString = keys[keyInt];
                }
            }

            if (keyString == null) throw new ArgumentException(string.Format(Resources.UnsupportedKeyType, key), "key");

            return keyString;
        }

        /// <summary>
        /// Retrieve the item based upon the specified key.
        /// </summary>
        /// <param name="key">Key of the item to retrieve.</param>
        /// <returns>Item associated with the key.</returns>
        public T get_Item(ref object key)
        {
            var keyString = GetDerivedKey(key);

            T value;
            if (_items.TryGetValue(keyString, out value))
            {
                return _items[keyString];
            }
            
            throw new ArgumentException(string.Format(Resources.ElementWithKeyNotFound, keyString), "key");
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count { get { return _items.Count; }}

        /// <summary>
        /// Indicates if the collection is read-only.
        /// </summary>
        public int ReadOnly { get; set; }

        /// <summary>
        /// Remove all items from the collection.
        /// </summary>
        public void RemoveAll()
        {
            _items.Clear();
        }

        /// <summary>
        /// Tries to get the item with the specified key.  If the item exists, returns true.
        /// </summary>
        /// <param name="key">Key of item to get.</param>
        /// <returns>True if the exist exists; false if not.</returns>
        public bool TryGetValue(object key)
        {
            var keyString = GetDerivedKey(key);

            T value;
            return _items.TryGetValue(keyString, out value);
        }
    }
}