using System.Runtime.InteropServices;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.TntCiReportingExport
{
    [Guid("2B6F64E2-55C3-4B30-BEC0-7E70855F9DE6"),
    InterfaceType(ComInterfaceType.InterfaceIsIDispatch),
    ComVisible(true)]
    public interface IKfxReleaseScript
    {
        ReleaseData DocumentData { get; set; }
        KfxReturnValue CloseScript();
        KfxReturnValue OpenScript();
        KfxReturnValue ReleaseDoc();
    }
}
