using System;
using System.Collections;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.TntCiReportingExportUnitTests
{
    /// <summary>
    /// Wrapper for Kofax.ReleaseLib.Values.
    /// </summary>
    public class MockValues : Values
    {
        private readonly CollectionBase<Value> _collection = new CollectionBase<Value>();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <summary>
        /// Add a new item to the collection.
        /// </summary>
        /// <param name="item">item to add.</param>
        /// <param name="key">Key to the item.</param>
        public void Add(Value item, string key)
        {
            _collection.Add(item, key);
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
        /// <param name="pdispOpsSession">Not implemented.</param>
        /// <param name="pdispBatch">Not implemented.</param>
        /// <param name="pdispDoc">Not implemented.</param>
        /// <param name="absFieldColl">Not implemented.</param>
        /// <param name="relLinkColl">Not implemented.</param>
        /// <param name="batchFieldColl">Not implemented.</param>
        /// <param name="bstrRowSeparator">Not implemented.</param>
        /// <param name="readOnly">Not implemented.</param>
        public void _Init(object pdispOpsSession, object pdispBatch, object pdispDoc, 
                          object absFieldColl, object relLinkColl, object batchFieldColl, 
                          string bstrRowSeparator, int readOnly)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve the item based upon the specified key.
        /// </summary>
        /// <param name="key">Key of the item to retrieve.</param>
        /// <returns>Item associated with the key.</returns>
        public Value get_Item(ref object key)
        {
            return _collection.get_Item(ref key);
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count { get { return _collection.Count; } }

        /// <summary>
        /// Indicates if the collection is read-only.
        /// </summary>
        public int ReadOnly { get; set; }

        /// <summary>
        /// Required to adhere to interface.  Not implemented.
        /// </summary>
        /// <param name="bstrTableName">Not implemented.</param>
        /// <param name="bstrSource">Not implemented.</param>
        /// <param name="lRow">Not implemented.</param>
        /// <returns>Not implemented.</returns>
        public Value get_RowValue(string bstrTableName, string bstrSource, int lRow)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Required to adhere to interface.  Not implemented.
        /// </summary>
        /// <param name="bstrTableName">Not implemented.</param>
        /// <returns>Not implemented.</returns>
        public int get_RowCount(string bstrTableName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Required to adhere to interface.  Not implemented.
        /// </summary>
        public object Tables { get; set; }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IValues.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
	}
}
