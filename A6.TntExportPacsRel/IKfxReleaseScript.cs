using System.Runtime.InteropServices;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel
{
    [Guid("D5885F50-34FC-43F5-B3D2-530DB9742A93"),
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
