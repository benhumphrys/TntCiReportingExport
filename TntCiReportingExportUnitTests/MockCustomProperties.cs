using System;
using System.Collections;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.TntCiReportingExportUnitTests
{
    /// <summary>
    /// Wrapper for Kofax.ReleaseLib.ICustomProperties2.
    /// </summary>
    public class MockCustomProperties : ICustomProperties2, CustomProperties
    {
        private readonly CollectionBase<CustomProperty> _collection = new CollectionBase<CustomProperty>();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <summary>
        /// Adds a new custom property to the collection.
        /// </summary>
        /// <param name="name">Name of the custom property.</param>
        /// <param name="value">Value of the custom property.</param>
        public void Add(string name, string value)
        {
            var property = new MockCustomProperty { Name = name, Value = value };
            _collection.Add(property, name);
        }

        /// <summary>
        /// Remove the specified object from the collection.
        /// </summary>
        /// <param name="key">Key of object to remove.</param>
        public void Remove(ref object key)
        {
            _collection.Remove(ref key);
        }

        /// <summary>
        /// Required to adhere to interface.  Not implemented.
        /// </summary>
        /// <param name="absPropCollection">Not implemented.</param>
        /// <param name="readOnlyFlag">Not implemented.</param>
        void ICustomProperties2._Init(object absPropCollection, int readOnlyFlag)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove all properties from the collection.
        /// </summary>
        public void RemoveAll()
        {
            _collection.RemoveAll();
        }

        /// <summary>
        /// Determines if a property with the specified name/key exists.
        /// </summary>
        /// <param name="key">Name/key of the property.</param>
        /// <returns>True is the property exists; false if not.</returns>
        public CustomProperty TryGetValue(object key)
        {
            return _collection.TryGetValue(key) ? _collection.get_Item(ref key) : null;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An object that can be used to iterate through the collection.</returns>
        public IEnumerator GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <summary>
        /// Required to adhere to interface.  Not implemented.
        /// </summary>
        /// <param name="absPropCollection">Not implemented.</param>
        /// <param name="readOnlyFlag">Not implemented.</param>
        void ICustomProperties._Init(object absPropCollection, int readOnlyFlag)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count
        {
            get { return _collection.Count; }
        }

        /// <summary>
        /// Indicates if the collection is read-only.
        /// </summary>
        public int ReadOnly { get; set; }

        /// <summary>
        /// Retrieve the item based upon the specified key.
        /// </summary>
        /// <param name="key">key of the item to retrieve.</param>
        /// <returns>Item associated with the key.</returns>
        public CustomProperty get_Item(ref object key)
        {
            return _collection.get_Item(ref key);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An object that can be used to iterate through the collection.</returns>
        IEnumerator ICustomProperties.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
	}
}
