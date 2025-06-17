namespace BBT.MaybePattern
{
    using System;

    /// <summary>
    /// Used in context of <see cref="Maybe{T}"/>. Used to provide <see cref="DoIfNone"/> method.
    /// </summary>
    public struct NoneCase : IEquatable<NoneCase>
    {
        private readonly bool isNone;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoneCase"/> struct.
        /// </summary>
        /// <param name="isNone">True if representing the none case, false if value is set.</param>
        internal NoneCase(bool isNone)
        {
            this.isNone = isNone;
        }

        /// <summary>
        /// Checks whether the operands are equal.
        /// </summary>
        /// <param name="a">Maybe to compare.</param>
        /// <param name="b">Maybe to compare.</param>
        public static bool operator ==(NoneCase a, NoneCase b)
        {
            return a.isNone.Equals(b.isNone);
        }

        /// <summary>
        /// Checks whether the operands are unequal.
        /// </summary>
        /// <param name="a">Maybe to compare.</param>
        /// <param name="b">Maybe to compare.</param>
        public static bool operator !=(NoneCase a, NoneCase b)
        {
            return !(a == b);
        }

        /// <summary>
        /// The <paramref name="doAction"/> is called in none case.
        /// </summary>
        /// <param name="doAction">The action which is performed if maybe is none.</param>
        public void DoIfNone(Action doAction)
        {
            if (doAction == null)
            {
                throw new ArgumentNullException(nameof(doAction));
            }

            if (this.isNone)
            {
                doAction();
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is NoneCase))
            {
                return false;
            }

            return this.Equals((NoneCase)obj);
        }

        /// <inheritdoc/>
        public bool Equals(NoneCase other)
        {
            return this == other;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => this.isNone.GetHashCode();
    }
}