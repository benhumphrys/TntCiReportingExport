using System;
using Tnt.KofaxCapture.TntCiReportingExport;
using Tnt.KofaxCapture.TntCiReportingExportUnitTests;

namespace Tnt.KofaxCapture.TntCiReportingExportTestHarness
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var setupData = UnitTestUtility.GetValidSetupData();
            //var setupData = UnitTestUtility.GetDefaultSetupData();
            var setupScript = new KfxReleaseSetupScript { SetupData = setupData };

            setupScript.OpenScript();
            setupScript.RunUI();
            setupScript.CloseScript();
        }
    }
}
