namespace BBT.MaybePattern.Tests
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// Provides utility method for test purposes.
    /// </summary>
    public class TestUtils
    {
        /// <summary>
        /// Serializes the given object into memory stream.
        /// </summary>
        /// <param name="objectType">the object to be serialized.</param>
        /// <returns>The serialized object as memory stream.</returns>
        public static MemoryStream SerializeToStream(object objectType)
        {
            var stream = new MemoryStream();
            try
            {
                var lFormatter = new BinaryFormatter();
                lFormatter.Serialize(stream, objectType);
                return stream;
            }
            catch
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Deserializes as an object.
        /// </summary>
        /// <param name="stream">the stream to deserialize.</param>
        /// <returns>the deserialized object.</returns>
        public static object DeserializeFromStream(MemoryStream stream)
        {
            var formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            var objectType = formatter.Deserialize(stream);
            return objectType;
        }
    }
}
