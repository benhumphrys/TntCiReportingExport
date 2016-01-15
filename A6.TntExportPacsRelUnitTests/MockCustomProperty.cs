using System;
using System.Runtime.InteropServices;

namespace Tnt.KofaxCapture.A6.TntExportPacsRelUnitTests
{
    /// <summary>
    /// Implementation of Kofax.ReleaseLib.CustomProperty.
    /// </summary>
    public class MockCustomProperty : Kofax.ReleaseLib.CustomProperty
	{
        /// <summary>
        /// Required to adhere to interface.  Not implemented.
        /// </summary>
        /// <param name="pProperty">Not implemented.</param>
        /// <param name="pRelSetup">Not implemented.</param>
        /// <param name="name">Not implemented.</param>
        /// <param name="value">Not implemented.</param>
        /// <returns>Return value.</returns>
        [return: MarshalAs(UnmanagedType.I4)]
        public Kofax.ReleaseLib.KfxReturnValue _Init(object pProperty, object pRelSetup, string name, string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the Name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Value of the property.
        /// </summary>
	    public string Value { get; set; }

        /// <summary>
        /// Gets or sets the _Property.
        /// </summary>
	    public int _Property { get; set; }
	}
}
