using System;

namespace Tnt.KofaxCapture.TntCiReportingExportUnitTests
{
    /// <summary>
    /// Mock Kofax.ReleaseLib.ImageFile.
    /// </summary>
	public class MockImageFile : Kofax.ReleaseLib.ImageFile 
	{
        public int DocId { get; private set; }

        public MockImageFile(int docId)
        {
            DocId = docId;
        }

        /// <summary>
        /// Required to adhere to interface.  Not implemented.
        /// </summary>
        /// <param name="name">Not implemented.</param>
        /// <param name="type">Not implemented.</param>
        /// <param name="directory">Not implemented.</param>
        /// <param name="destName">Not implemented.</param>
        /// <param name="extension">Not implemented.</param>
	    public void _Init(string name, int type, string directory = "", string destName = "", string extension = "")
	    {
	        throw new NotImplementedException();
	    }

        /// <summary>
        /// Copies the image file from the Capture image folder to the ReleaseData.ImageFilePath property. The export 
        /// directory may be specified as a parameter to this method. Duplicate file names are overwritten. The 
        /// ReleaseData.UseOriginalFileNames property is obeyed. 
        /// </summary>
        /// <param name="directory">Directory path to copy the image file to.</param>
	    public void Copy(string directory = "")
	    {
	        throw new NotImplementedException();
	    }

        /// <summary>
        /// Deletes the image file that was copied to the export directory. The optional parameter bstrDir specifies the 
        /// export directory for the image file. If directory is omitted, the image file is deleted from the directory 
        /// specified by ReleaseData.ImageFilePath. 
        /// </summary>
        /// <param name="directory">Directory to delete the image file from.</param>
	    public void Delete(string directory = "")
	    {
	        throw new NotImplementedException();
	    }

        /// <summary>
        /// Returns a string that specifies the file name of the image file. This string uniquely identifies the image in 
        /// the ImageFiles collection. This string can be used as the Key to retrieve an individual member from the 
        /// collection. 
        /// </summary>
	    public string FileName { get; set; }

        /// <summary>
        /// Integer that identifies the image format of the file. 
        /// </summary>
	    public int ImageType { get; set; }

        /// <summary>
        /// Holds the extension of a non-image file and exposes it to the user. For regular image files, this property 
        /// returns an empty string. 
        /// </summary>
	    public string NonImageFileExtension { get; set; }

        /// <summary>
        /// This string is the raw, original file name. This file name does not include the path or extension. 
        /// </summary>
	    public string RawOriginalFileName { get; set; }

        /// <summary>
        /// The full path name of the final destination file after calling the Copy method on the collection or on this 
        /// object. 
        /// </summary>
	    public string CopiedFileName { get; set; }
	}
}
