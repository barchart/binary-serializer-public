#region Using Statements

using Barchart.BinarySerializer.DataSerialization.Headers;
using Barchart.BinarySerializer.Schemas;

#endregion

namespace Barchart.BinarySerializer.DataSerialization.Types
{
    /// <summary>
    ///     Provides additional methods for encoding and decoding values of type <typeparamref name="TMember"/> to and from a <see cref="DataBuffer"/>.
    /// </summary>
    /// <typeparam name="TMember">The type of the value to be serialized.</typeparam>
    public interface IBinaryTypeObjectSerializer<TMember> : IBinaryTypeSerializer<TMember>
    {
        #region Methods

        /// <summary>
        ///     Encodes the specified oldValue and newValue to the given DataBuffer.
        /// </summary>
        /// <param name="dataBuffer">The DataBuffer to which the values will be encoded.</param>
        /// <param name="oldValue">The old value to be encoded.</param>
        /// <param name="newValue">The new value to be encoded.</param>
        public void Encode(DataBuffer dataBuffer, TMember oldValue, TMember newValue);

        /// <summary>
        ///     Decodes the specified existing value from the given DataBuffer.
        /// </summary>
        /// <param name="dataBuffer">The DataBuffer from which the value will be decoded.</param>
        /// <param name="existing">The existing value to be decoded.</param>
        /// <returns>A HeaderWithValue&lt;TMember&gt; containing the decoded value and its header information.</returns>
        public HeaderWithValue<TMember> Decode(DataBuffer dataBuffer, TMember existing);

        #endregion
    }
}