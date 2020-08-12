using System;
using System.Runtime.InteropServices;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.A6.TntExportPacsRelUnitTests
{
    /// <summary>
    /// Implementation of Kofax.ReleaseLib.Value.
    /// </summary>
	public class MockValue : Value
	{
        /// <summary>
        /// Required to adhere to interface.  Not implemented.
        /// </summary>
        /// <param name="sourceName">Not implemented.</param>
        /// <param name="destination">Not implemented.</param>
        /// <param name="value">Not implemented.</param>
        /// <param name="sourceType">Not implemented.</param>
        /// <param name="dataType">Not implemented.</param>
        /// <param name="bstrTableName">Not implemented.</param>
        /// <param name="bstrRowSeparator">Not implemented.</param>
        /// <returns>Not implemented.</returns>
        [return: MarshalAs(UnmanagedType.I4)]
	    public KfxReturnValue _Init(string sourceName, 
                                    string destination, 
                                    string value,
                                    [param: MarshalAs(UnmanagedType.I4)] KfxLinkSourceType sourceType,
                                    [param: MarshalAs(UnmanagedType.I4)] KfxIndexFieldType dataType, 
                                    string bstrTableName, 
                                    string bstrRowSeparator)
	    {
	        throw new NotImplementedException();
	    }

        /// <summary>
        /// Returns a string that indicates the field or column name in the external data 
        /// repository. This string can be used as the key to retrieve an individual member from 
        /// the collection. 
        /// </summary>
	    public string Destination { get; set; }

        /// <summary>
        /// Returns a data string to output. The data may need to be converted from a string to 
        /// the appropriate data type before saving the data. If a table field is obtained from 
        /// ReleaseData, then Value is the list of values for all rows. 
        /// </summary>
	    public string Value { get; set; }

        /// <summary>
        /// Returns a KfxLinkSourceType value that indicates the type of the document index data 
        /// field (index field, batch field, text constant, or Kofax Capture Value). 
        /// </summary>
        public KfxLinkSourceType SourceType
        {
            [return: MarshalAs(UnmanagedType.I4)]
            get;

            [param: MarshalAs(UnmanagedType.I4)]
            set;
        }

        /// <summary>
        /// Returns a KfxIndexFieldType value that indicates the data type of the value. 
        /// </summary>
        public KfxIndexFieldType DataType
        {
            [return: MarshalAs(UnmanagedType.I4)]
            get;

            [param: MarshalAs(UnmanagedType.I4)]
            set;
        }

        /// <summary>
        /// Returns a string that indicates the name of the document index data field. 
        /// </summary>
	    public string SourceName { get; set; }

        /// <summary>
        /// Returns a TableName value that indicates the name of the table. If the corresponding 
        /// field belongs to a table the table name can be obtained, otherwise this value is empty (""). 
        /// </summary>
	    public string TableName { get; set; }

        /// <summary>
        /// Required to adhere to interface.  Not implemented.
        /// </summary>
        /// <param name="lKey">Not implemented.</param>
        /// <returns>Not implemented.</returns>
	    public string get_RowValue(int lKey)
	    {
	        throw new NotImplementedException();
	    }

        /// <summary>
        /// Get the RowsCount.  Not implemented.
        /// </summary>
	    public int RowsCount { get; set; }

        /// <summary>
        /// Represents a collection of Value objects for each row of a table. Nothing/null if a 
        /// single-value field. 
        /// </summary>
        /// <param name="key">Key of the item to retrieve.</param>
        /// <returns>Item associated with the key.</returns>
        /// <remarks>Tables are not yet supported, so always returns null.</remarks>
	    public object get_RowValues(ref object key)
        {
            return null;
        }
	}
}
