using System.Runtime.InteropServices;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel2
{
    [Guid("D0221A2F-C75B-4BE4-A303-77E817747238"),
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
