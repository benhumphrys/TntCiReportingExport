namespace Tnt.KofaxCapture.TntCiReportingExport
{
    /// <summary>
    /// Holds standard Audit data.
    /// </summary>
    internal sealed class StandardAuditData
    {
        /// <summary>
        /// Gets or sets the  property.
        /// </summary>
        public string DomainAndUserName { get; set; }

        /// <summary>
        /// Gets or sets the MachineName property.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the Date property.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the Time property.
        /// </summary>
        public string Time { get; set; }
    }
}
