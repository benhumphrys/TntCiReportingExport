using System;
using System.Collections.Generic;
using Kofax.ReleaseLib;

namespace Tnt.KofaxCapture.TntCiReportingExport
{
    /// <summary>
    /// Holds details of fields for display and selection during setup.
    /// </summary>
    internal  sealed class SetupField : IEquatable<SetupField>
    {
        /// <summary>
        /// Gets or sets the friendly field type value for display to user.
        /// </summary>
        public string FriendlyFieldType { get; set; }

        /// <summary>
        /// Gets or sets the Kofax field type.
        /// </summary>
        public KfxLinkSourceType KofaxFieldType { get; set; }

        /// <summary>
        /// Gets or sets the field name.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(SetupField other)
        {
            if (other == null) return false;
            
            return (string.Equals(FieldName, other.FieldName, StringComparison.CurrentCultureIgnoreCase) &&
                    EqualityComparer<KfxLinkSourceType>.Default.Equals(KofaxFieldType, other.KofaxFieldType));
        }

        /// <summary>
        /// Determines whether the specified System.Object is equal to the current System.Object.
        /// </summary>
        /// <param name="o">The System.Object to compare with the current System.Object.</param>
        /// <returns>true if the specified System.Object is equal to the current System.Object; 
        /// otherwise, false.</returns>
        public override bool Equals(object o)
        {
            return Equals(o as SetupField);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current System.Object.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<string>.Default.GetHashCode(FieldName) * 37 +
                   EqualityComparer<KfxLinkSourceType>.Default.GetHashCode(KofaxFieldType);
        }
    }
}