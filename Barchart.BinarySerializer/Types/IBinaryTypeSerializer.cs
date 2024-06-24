#region Using Statements

using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Headers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Provides methods for encoding and decoding values of type <typeparamref name="T"/> to and from a <see cref="DataBuffer"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to be serialized.</typeparam>
    public interface IBinaryTypeSerializer<T>
    {
        #region Methods

        /// <summary>
        ///     Encodes the specified value into the provided <see cref="DataBuffer"/>.
        /// </summary>
        /// <param name="dataBuffer">The buffer to write the encoded value to.</param>
        /// <param name="value">The value to be encoded.</param>
        public void Encode(IDataBuffer dataBuffer, T value);

        /// <summary>
        ///     Decodes a value from the provided <see cref="DataBuffer"/>.
        /// </summary>
        /// <param name="dataBuffer">The buffer to read the encoded value from.</param>
        /// <returns>A <see cref="HeaderWithValue{T}"/> containing the decoded value and its length in bits.</returns>
        public HeaderWithValue<T> Decode(IDataBuffer dataBuffer);

        /// <summary>
        ///     Calculates the length of the specified value in bits.
        /// </summary>
        /// <param name="value">The value to calculate the length for.</param>
        /// <returns>The length of the value in bits.</returns>
        public int GetLengthInBits(T value);

        #endregion
    }
}