#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Provides additional methods for encoding and decoding values of type <typeparamref name="T"/> to and from a <see cref="DataBuffer"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to be serialized.</typeparam>
    public interface IBinaryTypeObjectSerializer<T> : IBinaryTypeSerializer<T>
    {
        #region Methods

        /// <summary>
        ///     Encodes the specified oldValue and newValue to the given DataBuffer.
        /// </summary>
        /// <param name="dataBuffer">The DataBuffer to which the values will be encoded.</param>
        /// <param name="oldValue">The old value to be encoded.</param>
        /// <param name="newValue">The new value to be encoded.</param>
        public void Encode(IDataBuffer dataBuffer, T oldValue, T newValue);

        /// <summary>
        ///     Decodes the specified existing value from the given DataBuffer.
        /// </summary>
        /// <param name="dataBuffer">The DataBuffer from which the value will be decoded.</param>
        /// <param name="existing">The existing value to be decoded.</param>
        /// <returns>A HeaderWithValue&lt;T&gt; containing the decoded value and its header information.</returns>
        public HeaderWithValue<T> Decode(IDataBuffer dataBuffer, T existing);

        #endregion
    }
}