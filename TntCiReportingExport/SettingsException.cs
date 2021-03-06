using System;
using System.Runtime.Serialization;

namespace Tnt.KofaxCapture.TntCiReportingExport
{
    /// <summary>
    /// Settings exception (type-agnostic).
    /// </summary>
    [Serializable]
    public class SettingsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// Tnt.KofaxCapture.TntCiReportingExport.SettingsException class.
        /// </summary>
        public SettingsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// Tnt.KofaxCapture.TntCiReportingExport.SettingsException class 
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SettingsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// Tnt.KofaxCapture.TntCiReportingExport.SettingsException class 
        /// with serialized data.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object 
        /// data about the exception being thrown.</param>
        /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual 
        /// information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="SerializationException">The class name is null or System.Exception.HResult 
        /// is zero (0).</exception>
        protected SettingsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// Tnt.KofaxCapture.TntCiReportingExport.SettingsException class with a specified 
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference 
        /// (Nothing in Visual Basic) if no inner exception is specified.</param>
        public SettingsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}