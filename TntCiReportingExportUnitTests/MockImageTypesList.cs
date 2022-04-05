using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Tnt.KofaxCapture.TntCiReportingExportUnitTests
{
    public class MockImageTypesList : IEnumerable
    {
        private readonly List<MockImageType> _imageTypes;

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            // We can't just return the List's GetEnumerator() because generic enumerators don't work 100% in COM (it 
            // seems to work in VB6 release scripts, but not .NET ones).
            foreach (var imageType in _imageTypes)
            {
                yield return imageType;
            }
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count { get { return _imageTypes.Count; } }

        /// <summary>
        /// Initialises a new instance of the Scansation.KofaxCapture.KcpExportWrapper.ImageTypesList class.
        /// </summary>
        public MockImageTypesList()
        {
            _imageTypes = new List<MockImageType>();
        }

        /// <summary>
        /// Initialises a new instance of the Scansation.KofaxCapture.KcpExportWrapper.ImageTypesList class using the
        /// specified list of ImageTypes.
        /// </summary>
        /// <param name="imageTypes">List of ImageTypes.</param>
        public MockImageTypesList(IEnumerable<MockImageType> imageTypes) : this()
        {
            AddRange(imageTypes);
        }

        /// <summary>
        /// Adds the specified ImageTypes to the list.
        /// </summary>
        /// <param name="imageTypes">ImageTypes to add.</param>
        [ComVisible(false)]
        public void AddRange(IEnumerable<MockImageType> imageTypes)
        {
            if (imageTypes != null)
            {
                _imageTypes.AddRange(imageTypes);
            }
        }
    }
}
