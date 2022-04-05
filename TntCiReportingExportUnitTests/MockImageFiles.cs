using System;
using System.Collections;
using System.IO;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.TntCiReportingExportUnitTests
{
    /// <summary>
    /// Wrapper for Kofax.ReleaseLib.ImageFiles
    /// </summary>
	public class MockImageFiles : ImageFiles
    {
        private readonly CollectionBase<ImageFile> _collection = new CollectionBase<ImageFile>();
            
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
	    public void Add(ImageFile item, string key)
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
        /// Copies the images to the destination location. If ReleaseData.UseOriginalFileNames is 
        /// set, then files are named based on their original file names. If a duplicate file name 
        /// is used, then an error occurs and some image files in the document may not be copied. 
        /// </summary>
        /// <param name="directory">Optional directory path to copy the images to.</param>
        /// <param name="imageType">Type of images to output.</param>
	    public void Copy(string directory = "", int imageType = -1)
	    {
            for (var i = 1; i <= Count; i++)
            {
                object key = i;
                var imageFile = (MockImageFile) get_Item(ref key);

                ReleasedDirectory = UnitTestUtility.CopyFile(imageFile.FileName, imageFile.DocId, directory,
                    Path.GetExtension(imageFile.FileName));
            }
	    }

        /// <summary>
        /// Deletes the image files that were copied to the export directory. The optional 
        /// parameter specifies the export directory for image files. If directory is omitted, 
        /// the image files are deleted from the directory specified by ReleaseData.ImageFilePath. 
        /// </summary>
        /// <param name="directory"></param>
	    public void Delete(string directory = "")
	    {
	        throw new NotImplementedException();
	    }

        /// <summary>
        /// Required to adhere to interface.  Not implemented.
        /// </summary>
        /// <param name="opsSession">Not implemented.</param>
        /// <param name="pBatch">Not implemented.</param>
        /// <param name="document">Not implemented.</param>
        /// <param name="imageType">Not implemented.</param>
        /// <param name="directory">Not implemented.</param>
        /// <param name="skipFirst">Not implemented.</param>
        /// <param name="useOriginalFileNames">Not implemented.</param>
        /// <param name="readOnlyFlag">Not implemented.</param>
	    public void _Init(object opsSession, object pBatch, object document, int imageType, 
                          string directory, int skipFirst, int useOriginalFileNames, 
                          int readOnlyFlag)
	    {
	        throw new NotImplementedException();
	    }

        /// <summary>
        /// Retrieve the item based upon the specified key.
        /// </summary>
        /// <param name="key">Key of the item to retrieve.</param>
        /// <returns>Item associated with the key.</returns>
	    public ImageFile get_Item(ref object key)
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
        /// Returns a string that identifies the full path of the directory for exporting the 
        /// image files associated with a document. If the image files are exported as a set of 
        /// single-page images, the ReleaseDirectory path includes the subdirectory for the 
        /// images files. 
        /// </summary>
	    public string ReleasedDirectory { get; set; }

        /// <summary>
        /// Returns a long integer (Boolean) that indicates whether the image files associated 
        /// with a document are exported as a set of single-page images to a subdirectory (True) 
        /// or as a multi-page image file to the specified export directory (False). 
        /// </summary>
	    public int ReleasedToDirectory { get; set; }

        /// <summary>
        /// Set to true if the document contains one or more non-image files (eDocuments). If 
        /// the document contains only regular image files, this property returns false. 
        /// </summary>
	    public int ContainsNonImageFile { get; set; }

        /// <summary>
        /// Returns a string that identifies the copied location of the non-image files in the 
        /// document. If the document contains only image files, this property returns an empty 
        /// string. 
        /// </summary>
	    public string NonImageFilesReleasedDirectory { get; set; }
        
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IImageFiles.GetEnumerator()
	    {
            return _collection.GetEnumerator();
	    }
	}
}
