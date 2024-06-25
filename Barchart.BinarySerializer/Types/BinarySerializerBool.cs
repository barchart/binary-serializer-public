#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerBool : IBinaryTypeSerializer<bool>
    {
        #region Constants
        
        private const int ENCODED_HEADER_LENGTH = 2;
        private const int ENCODED_VALUE_LENGTH = 1;
        
        private const int ENCODED_LENGTH = ENCODED_HEADER_LENGTH + ENCODED_VALUE_LENGTH;
        
        #endregion

        #region Methods

        public void Encode(IDataBuffer dataBuffer, bool value)
        {
            Header.WriteToBuffer(dataBuffer, false, false);
            
            dataBuffer.WriteBit(value);
        }

        public Attribute<bool> Decode(IDataBuffer dataBuffer)
        {
            return new Attribute<bool>(Header.ReadFromBuffer(dataBuffer), dataBuffer.ReadBit());
        }

        public int GetLengthInBits(bool value)
        {
            return ENCODED_LENGTH;
        }

        #endregion
    }
}