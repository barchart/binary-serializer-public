#region Using Statements

using Barchart.BinarySerializer.Schemas;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Provides methods for encoding and decoding values of type <typeparamref name="TMember"/> to and from a <see cref="DataBuffer"/>.
    /// </summary>
    /// <typeparam name="TMember">The type of the value to be serialized.</typeparam>
    public interface IBinaryTypeSerializer<TMember>
    {
        #region Methods

        public void Encode(DataBuffer dataBuffer, TMember value);
        public HeaderWithValue<TMember> Decode(DataBuffer dataBuffer);
        public int GetLengthInBits(TMember value);

        #endregion
    }
}