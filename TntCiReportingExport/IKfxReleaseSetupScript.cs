using System.Runtime.InteropServices;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.TntCiReportingExport
{
    [Guid("51228419-F3B8-40A0-8626-19E5BB2603EF"),
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
