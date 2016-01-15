﻿namespace Tnt.KofaxCapture.A6.TntExportPacsRel
{
    /// <summary>
    /// Holds MIS audit data.
    /// </summary>
    internal sealed class MisAuditData
    {
        /// <summary>
        /// Gets or sets the AuditFilePath property.
        /// </summary>
        public string AuditFilePath { get; set; }

        /// <summary>
        /// Gets or sets the ScanDepot property.
        /// </summary>
        public string ScanDepot { get; set; }

        /// <summary>
        /// Gets or sets the ScanDate property.
        /// </summary>
        public string ScanDate { get; set; }

        /// <summary>
        /// Gets or sets the AuditFilePath property.
        /// </summary>
        public string BatchType { get; set; }

        /// <summary>
        /// Gets or sets the RoundID property.
        /// </summary>
        public string RoundId { get; set; }

        /// <summary>
        /// Gets or sets the TotalImageCount property.
        /// </summary>
        public int TotalImageCount { get; set; }
    }
}
