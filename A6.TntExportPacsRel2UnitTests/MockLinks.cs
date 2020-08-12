using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel2UnitTests
{
    internal sealed class MockLinks : Links
    {
        private readonly List<MockLink> _links = new List<MockLink>();
            
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _links.GetEnumerator();
        }

        public void Add(string source, KfxLinkSourceType sourceType, string destination)
        {
            _links.Add(new MockLink{Source = source, SourceType = sourceType, Destination = destination});
        }

        public void Remove(ref object key)
        {
            if (key is int)
            {
                _links.RemoveAt((int) key);
            }
            else
            {
                var s = key as string;
                if (s == null) throw new NotImplementedException();
                var keyString = s;
                var link = _links.Single(l => l.Destination.Equals(keyString, StringComparison.CurrentCultureIgnoreCase));
                _links.Remove(link);
            }

            throw new NotImplementedException();
        }

        public void _Init(object relSetup, int readOnly)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            _links.Clear();
        }

        public Link get_Item(ref object key)
        {
            if (key is int)
            {
                return _links[(int)key];
            }

            var keyString = key as string;
            if (keyString != null)
            {
                var link = _links.Single(l => l.Destination.Equals(keyString, StringComparison.CurrentCultureIgnoreCase));
                return link;
            }
            
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _links.Count; }
        }
        public int ReadOnly { get; set; }

        IEnumerator ILinks.GetEnumerator()
        {
            return _links.GetEnumerator();
        }
    }
}
