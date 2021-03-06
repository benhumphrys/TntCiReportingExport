using System;
using System.Runtime.Serialization;

namespace Tnt.KofaxCapture.TntCiReportingExport
{
    /// <summary>
    /// Export exception (type-agnostic).
    /// </summary>
    [Serializable]
    public class ExportException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// Tnt.KofaxCapture.TntCiReportingExport.ExportException class.
        /// </summary>
        public ExportException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// Tnt.KofaxCapture.TntCiReportingExport.ExportException class 
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ExportException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// Tnt.KofaxCapture.TntCiReportingExport.ExportException class 
        /// with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
        /// data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
        /// information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="SerializationException">The class name is null or System.Exception.HResult 
        /// is zero (0).</exception>
        protected ExportException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// Tnt.KofaxCapture.TntCiReportingExport.ExportException class with a specified 
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference 
        /// (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ExportException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}