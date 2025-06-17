namespace BBT.MaybePattern
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides utility methods for maybe.
    /// </summary>
    internal static class MaybeUtils
    {
        /// <summary>
        /// Asserts that <paramref name="arg"/> is not null.
        /// In case of null an <see cref="ArgumentNullException"/> is thrown.
        /// </summary>
        /// <typeparam name="T">The argument's type.</typeparam>
        /// <param name="arg">The argument.</param>
        /// <param name="argName">The arguments name.</param>
        /// <returns>The argument.</returns>
        internal static T CheckArgumentNotNull<T>(T arg, string argName)
            where T : class
        {
            if (arg == null)
            {
                throw new ArgumentNullException(argName);
            }

            return arg;
        }

        /// <summary>
        /// Asserts that <paramref name="parameter"/> is not null.
        /// In case of null an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <typeparam name="T">The parameter's type.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterName">The parameter's name.</param>
        /// <param name="additionalMessage">An optional additional error message.</param>
        /// <returns>The parameter.</returns>
        internal static T CheckParameterNotNull<T>(
            T parameter, string parameterName, string additionalMessage = "")
        {
            if (parameter == null)
            {
                var msg = $"({parameterName}) may not be null.".ToString(CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(additionalMessage))
                {
                    msg = msg + " " + additionalMessage;
                }

                throw new InvalidOperationException(msg);
            }

            return parameter;
        }

        /// <summary>
        /// Gets the casted value from <paramref name="info"/>
        /// corresponding to <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The value's type.</typeparam>
        /// <param name="info">The serialization info.</param>
        /// <param name="key">The key.</param>
        /// <returns>The corresponding value.</returns>
        internal static T GetDeserializedValue<T>(
            SerializationInfo info,
            string key)
        {
            CheckArgumentNotNull(info, nameof(info));

            return (T)info.GetValue(key, typeof(T));
        }

        /// <summary>
        /// Sets <paramref name="data"/> to serialization info.
        /// </summary>
        /// <typeparam name="T">The data's type.</typeparam>
        /// <param name="info">The serialization info.</param>
        /// <param name="key">The data's key.</param>
        /// <param name="data">The data.</param>
        internal static void SetData<T>(SerializationInfo info, string key, T data)
        {
            MaybeUtils.CheckArgumentNotNull(info, nameof(info));

            info.AddValue(key, data, typeof(T));
        }
    }
}
