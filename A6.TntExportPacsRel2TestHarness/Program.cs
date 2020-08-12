using System;
using Tnt.KofaxCapture.A6.TntExportPacsRel;
using Tnt.KofaxCapture.A6.TntExportPacsRelUnitTests;

namespace Tnt.KofaxCapture.A6.TntExportPacsRelTestHarness
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
