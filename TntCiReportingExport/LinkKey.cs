using System;
using System.Collections.Generic;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.TntCiReportingExport
{
    /// <summary>
    /// Holds data that can be used as a key for a link.
    /// </summary>
    internal sealed class LinkKey : IEquatable<LinkKey>
    {

        /// <summary>
        /// Gets or sets the Name part of the LinkKey.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the Type part of the LinkKey.
        /// </summary>
        public KfxLinkSourceType Type { get; }

        /// <summary>
        /// Initializes a new instance of the Tnt.KofaxCapture.TntCiReportingExport.LinkKey class.
        /// </summary>
        /// <param name="name">Name of the LinkKey.</param>
        /// <param name="type">Type of the LinkKey.</param>
        public LinkKey(string name, KfxLinkSourceType type)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (!Enum.IsDefined(typeof (KfxLinkSourceType), type)) throw new ArgumentOutOfRangeException(nameof(type));

            Name = name;
            Type = type;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(LinkKey other)
        {
            if (other == null) return false;
            
            return string.Equals(Name, other.Name, StringComparison.CurrentCultureIgnoreCase) &&
                   Type == other.Type;
        }

        /// <summary>
        /// Determines whether the specified System.Object is equal to the current System.Object.
        /// </summary>
        /// <param name="o">The System.Object to compare with the current System.Object.</param>
        /// <returns>true if the specified System.Object is equal to the current System.Object; 
        /// otherwise, false.</returns>
        public override bool Equals(object o)
        {
            return Equals(o as LinkKey);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current System.Object.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<string>.Default.GetHashCode(Name.ToUpperInvariant()) * 37 +
                   EqualityComparer<KfxLinkSourceType>.Default.GetHashCode(Type);
        }

        /// <summary>
        /// Generate a LinkKey from a given mnemonic (as found in XML template files).
        /// </summary>
        /// <param name="mnemonic">Mnemonic to interpret (typically the field name surrounded
        /// by some magic numbers).</param>
        /// <returns>Derived LinkKey.</returns>
        public static LinkKey FromMnemonic(string mnemonic)
        {
            if (mnemonic == null) throw new ArgumentNullException("mnemonic");

            mnemonic = mnemonic.Trim();

            if (mnemonic.StartsWith("{$", StringComparison.Ordinal) && 
                mnemonic.EndsWith("}", StringComparison.Ordinal))
            {
                var name = mnemonic.Substring(2, mnemonic.Length - 3);
                return new LinkKey(name, KfxLinkSourceType.KFX_REL_BATCHFIELD);
            }

            if (mnemonic.StartsWith("{@", StringComparison.Ordinal) &&
                mnemonic.EndsWith("}", StringComparison.Ordinal))
            {
                var name = mnemonic.Substring(2, mnemonic.Length - 3);
                return new LinkKey(name, KfxLinkSourceType.KFX_REL_INDEXFIELD);
            }

            if (mnemonic.StartsWith("{", StringComparison.Ordinal) &&
                mnemonic.EndsWith("}", StringComparison.Ordinal))
            {
                var name = mnemonic.Substring(1, mnemonic.Length - 2);
                return new LinkKey(name, KfxLinkSourceType.KFX_REL_VARIABLE);
            }

            return null;
        }
    }

}
