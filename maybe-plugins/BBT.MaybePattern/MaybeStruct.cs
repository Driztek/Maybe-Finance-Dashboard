namespace BBT.MaybePattern
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a nullable value type which may be null or not null.
    /// </summary>
    /// <remarks>
    /// Implementation of the "maybe monad" pattern / Optional-pattern.
    /// https://smellegantcode.wordpress.com/2008/12/11/the-maybe-monad-in-c/.
    /// </remarks>
    /// <typeparam name="T">The value type.</typeparam>
    [Serializable]
    public struct MaybeStruct<T> : ISerializable, IEquatable<MaybeStruct<T>>
        where T : struct
    {
        private readonly T? value;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaybeStruct{T}"/> struct.
        /// </summary>
        /// <param name="value">The potentially nullable value.</param>
        internal MaybeStruct(T? value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaybeStruct{T}"/> struct.
        /// Constructor used for deserialization.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        internal MaybeStruct(SerializationInfo info, StreamingContext context)
        {
            this.value = MaybeUtils.GetDeserializedValue<T?>(info, nameof(this.value));
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Maybe"/> has a value (true)
        /// or is representing the null case (false).
        /// </summary>
        public bool HasValue => this.value.HasValue;

        /// <summary>
        /// Implicit conversion from struct to maybe.
        /// </summary>
        /// <param name="value">the struct or null.</param>
        public static implicit operator MaybeStruct<T>(T? value) => new MaybeStruct<T>(value);

        /// <summary>
        /// Checks whether the operands are equal.
        /// </summary>
        /// <param name="a">Maybe to compare.</param>
        /// <param name="b">Maybe to compare.</param>
        public static bool operator ==(MaybeStruct<T> a, MaybeStruct<T> b)
        {
            return object.Equals(a.value, b.value);
        }

        /// <summary>
        /// Checks whether the operands are unequal.
        /// </summary>
        /// <param name="a">Maybe to compare.</param>
        /// <param name="b">Maybe to compare.</param>
        public static bool operator !=(MaybeStruct<T> a, MaybeStruct<T> b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Starts <paramref name="doAction"/> if value is not null.
        /// </summary>
        /// <param name="doAction">The action which is performed if maybe is not none.</param>
        /// <returns>The none case.</returns>
        public NoneCase Do(Action<T> doAction)
        {
            MaybeUtils.CheckArgumentNotNull(doAction, nameof(doAction));

            if (this.value.HasValue)
            {
                doAction(this.value.Value);
                return new NoneCase(false);
            }

            return new NoneCase(true);
        }

        /// <summary>
        /// Gets the <see cref="Maybe{TResult}"/> of the projection function <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the projected result.</typeparam>
        /// <param name="func">The projection function.</param>
        /// <returns>The projected maybe.</returns>
        public Maybe<TResult> Some<TResult>(Func<T, TResult> func)
            where TResult : class
        {
            MaybeUtils.CheckArgumentNotNull(func, nameof(func));

            var lMaybe = Maybe.None<TResult>();
            this.Do(x => lMaybe = Maybe.Some(func(x)));

            return lMaybe;
        }

        /// <summary>
        /// Gets the <see cref="Maybe{TResult}"/> of the projection function <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the projected result.</typeparam>
        /// <param name="func">The projection function.</param>
        /// <returns>The projected maybe.</returns>
        public Maybe<TResult> Some<TResult>(Func<T, Maybe<TResult>> func)
            where TResult : class
        {
            MaybeUtils.CheckArgumentNotNull(func, nameof(func));

            var lMaybe = Maybe.None<TResult>();
            this.Do(x => lMaybe = func(x));

            return lMaybe;
        }

        /// <summary>
        /// Gets the <see cref="MaybeStruct{TResult}"/> of the projection function <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the projected result.</typeparam>
        /// <param name="func">The projection function.</param>
        /// <returns>The projected maybe.</returns>
        public MaybeStruct<TResult> SomeStruct<TResult>(Func<T, TResult?> func)
            where TResult : struct
        {
            MaybeUtils.CheckArgumentNotNull(func, nameof(func));

            var lMaybe = Maybe.NoneStruct<TResult>();
            this.Do(x => lMaybe = Maybe.SomeStruct(func(x)));

            return lMaybe;
        }

        /// <summary>
        /// Gets the <see cref="MaybeStruct{TResult}"/> of the projection function <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the projected result.</typeparam>
        /// <param name="func">The projection function.</param>
        /// <returns>The projected maybe.</returns>
        public MaybeStruct<TResult> SomeStruct<TResult>(Func<T, MaybeStruct<TResult>> func)
            where TResult : struct
        {
            MaybeUtils.CheckArgumentNotNull(func, nameof(func));

            var lMaybe = Maybe.NoneStruct<TResult>();
            this.Do(x => lMaybe = func(x));

            return lMaybe;
        }

        /// <summary>
        /// Returns the value in case it is initialized.
        /// Otherwise <paramref name="defaultValue"/> is returned.
        /// </summary>
        /// <param name="defaultValue">The value returned in case maybe is not initialized with a value.</param>
        /// <returns>The value or <paramref name="defaultValue"/>.</returns>
        public T ValueOrDefault(
            T defaultValue)
        {
            var returnValue = defaultValue;
            this.Do(x => returnValue = x);
            return returnValue;
        }

        /// <summary>
        /// Returns the value in case it is initialized.
        /// Otherwise throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="maybeParameterName">The maybe reference used in error message.</param>
        /// <param name="additionalMessage">Additional error message.</param>
        /// <returns>The value.</returns>
        public T ValueOrException(
            string maybeParameterName = "",
            string additionalMessage = "")
        {
            var parameterName = maybeParameterName;
            if (string.IsNullOrEmpty(maybeParameterName))
            {
                parameterName = typeof(T).Name;
            }

            return MaybeUtils.CheckParameterNotNull(this.value, parameterName, additionalMessage)
                .Value;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is MaybeStruct<T>))
            {
                return false;
            }

            return this.Equals((MaybeStruct<T>)obj);
        }

        /// <inheritdoc/>
        public bool Equals(MaybeStruct<T> other)
        {
            return this == other;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (this.value != null)
            {
                return this.value.GetHashCode();
            }

            return base.GetHashCode();
        }

        /// <inheritdoc/>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            MaybeUtils.SetData(info, nameof(this.value), this.value);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (this.HasValue)
            {
                return this.value.ToString();
            }

            return string.Empty;
        }
    }
}
