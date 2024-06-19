#region Using Statements

using Barchart.BinarySerializer.DataSerialization.Headers;
using Barchart.BinarySerializer.Schemas;

#endregion

namespace Barchart.BinarySerializer.DataSerialization.Types
{
    /// <summary>
    ///     Provides methods for encoding and decoding values of type <typeparamref name="TMember"/> to and from a <see cref="DataBuffer"/>.
    /// </summary>
    /// <typeparam name="TMember">The type of the value to be serialized.</typeparam>
    public interface IBinaryTypeSerializer<TMember>
    {
        #region Methods

        /// <summary>
        ///     Encodes the specified value into the provided <see cref="DataBuffer"/>.
        /// </summary>
        /// <param name="dataBuffer">The buffer to write the encoded value to.</param>
        /// <param name="value">The value to be encoded.</param>
        public void Encode(DataBuffer dataBuffer, TMember value);

        /// <summary>
        ///     Decodes a value from the provided <see cref="DataBuffer"/>.
        /// </summary>
        /// <param name="dataBuffer">The buffer to read the encoded value from.</param>
        /// <returns>A <see cref="HeaderWithValue{TMember}"/> containing the decoded value and its length in bits.</returns>
        public HeaderWithValue<TMember> Decode(DataBuffer dataBuffer);

        /// <summary>
        ///     Calculates the length of the specified value in bits.
        /// </summary>
        /// <param name="value">The value to calculate the length for.</param>
        /// <returns>The length of the value in bits.</returns>
        public int GetLengthInBits(TMember value);

        #endregion
    }
}