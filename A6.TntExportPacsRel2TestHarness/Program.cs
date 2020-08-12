using System;
using Tnt.KofaxCapture.A6.TntExportPacsRel2;
using Tnt.KofaxCapture.A6.TntExportPacsRel2UnitTests;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel2TestHarness
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
