#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Writes (and reads) values of type <typeparam name="T" /> to (and from)
    ///     a binary data source.
    /// </summary>
    /// <typeparam name="T">
    ///     The type to serialize to binary (and deserialize from binary).
    /// </typeparam>
    public interface IBinaryTypeSerializer<T>
    {
        #region Methods

        /// <summary>
        ///     Writes a value to a binary data source.
        /// </summary>
        /// <param name="dataBuffer">
        ///     The write target.
        /// </param>
        /// <param name="value">
        ///     The value to write.
        /// </param>
        public void Encode(IDataBufferWriter dataBuffer, T value);

        /// <summary>
        ///     Reads a value from a binary data source.
        /// </summary>
        /// <param name="dataBuffer">
        ///     The binary data source.
        /// </param>
        /// <returns>
        ///     The value.
        /// </returns>
        public Attribute<T> Decode(IDataBufferReader dataBuffer);

        /// <summary>
        ///     Calculates the number of bits needed to encode a value (or the
        ///     number of bits required to read a value from the binary data source).
        /// </summary>
        /// <param name="value">
        ///     The value to be serialized.
        /// </param>
        /// <returns>
        ///     The number of bits required to serialize the value.
        /// </returns>
        public int GetLengthInBits(T value);

        #endregion
    }
}