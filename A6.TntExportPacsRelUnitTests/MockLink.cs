using System;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.A6.TntExportPacsRelUnitTests
{
    internal sealed class MockLink : Link
    {
        public KfxReturnValue _Init(object absLink, string source, KfxLinkSourceType sourceType, string destination)
        {
            throw new NotImplementedException();
        }

        public string Source { get; set; }
        public KfxLinkSourceType SourceType { get; set; }
        public string Destination { get; set; }
        public object _Link { get; set; }
    }
}
