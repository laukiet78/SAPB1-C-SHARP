﻿// <copyright filename="LayoutKeyEventHandlerAttribute.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Forms.Events.LayoutKeyEvents
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class LayoutKeyEventHandlerAttribute : Attribute, IEquatable<LayoutKeyEventHandlerAttribute>
    {
        public bool Before { get; }

        public LayoutKeyEventHandlerAttribute(bool before = false)
        {
            Before = before;
        }

        #region Auto-generated Equality Members

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(LayoutKeyEventHandlerAttribute other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return base.Equals(other) && (Before == other.Before);
        }

        /// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
        /// <returns>true if <paramref name="obj" /> equals the type and value of this instance; otherwise, false.</returns>
        /// <param name="obj">An <see cref="T:System.Object" /> to compare with this instance or null. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((LayoutKeyEventHandlerAttribute) obj);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ Before.GetHashCode();
            }
        }

        public static bool operator ==(LayoutKeyEventHandlerAttribute left, LayoutKeyEventHandlerAttribute right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LayoutKeyEventHandlerAttribute left, LayoutKeyEventHandlerAttribute right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}