using System.Runtime.InteropServices;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel2
{
    [Guid("4FAFC279-B52B-478C-ACE6-1BCE4A5867D5"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch),
    ComVisible(true)]
    public interface IKfxReleaseSetupScript
    {
        ReleaseSetupData SetupData { get; set; }
        KfxReturnValue CloseScript();
        KfxReturnValue ActionEvent(ref KfxActionValue actionId, ref string data1, ref string data2);
        KfxReturnValue OpenScript();
        // ReSharper disable once InconsistentNaming
        KfxReturnValue RunUI();
    }
}
