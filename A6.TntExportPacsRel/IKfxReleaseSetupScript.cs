using System.Runtime.InteropServices;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel
{
    [Guid("C8F581E7-1A69-4DC3-852B-F2846C7ED9E1"),
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
