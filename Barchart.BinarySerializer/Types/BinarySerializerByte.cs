#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerByte : IBinaryTypeSerializer<byte>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH = 2;
        private const int ENCODED_VALUE_LENGTH = sizeof(byte);
        
        private const int ENCODED_LENGTH = ENCODED_HEADER_LENGTH + ENCODED_VALUE_LENGTH;
        
        #endregion

        #region Methods
        
        public void Encode(IDataBuffer dataBuffer, byte value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            dataBuffer.WriteByte(value);
        }

        public Attribute<byte> Decode(IDataBuffer dataBuffer)
        {
            return new Attribute<byte>(Header.ReadFromBuffer(dataBuffer), dataBuffer.ReadByte());
        }

        public int GetLengthInBits(byte value)
        {
            return ENCODED_LENGTH;
        }
        
        #endregion
    }
}