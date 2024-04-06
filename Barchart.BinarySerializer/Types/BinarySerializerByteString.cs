using Barchart.BinarySerializer.Schemas;
using Google.Protobuf;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerByteString : IBinaryTypeSerializer<ByteString>
    {
        public const int NumberOfHeaderBitsNumeric = 2;

        public HeaderWithValue<ByteString> Decode(DataBuffer dataBuffer)
        {
            Header header = new()
            {
                IsMissing = dataBuffer.ReadBit() == 1
            };

            if (header.IsMissing)
            {
                return new HeaderWithValue<ByteString>(header, default);
            }

            header.IsNull = dataBuffer.ReadBit() == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue<ByteString>(header, default);
            }

            byte[] lengthBytes = new byte[sizeof(int)];

            for (int i = lengthBytes.Length - 1; i >= 0; i--)
            {
                lengthBytes[i] = dataBuffer.ReadByte();
            }

            int length = BitConverter.ToInt32(lengthBytes, 0);

            byte[] valueBytes = new byte[length];

            for (int i = length - 1; i >= 0; i--)
            {
                valueBytes[i] = dataBuffer.ReadByte();
            }

            ByteString result = ByteString.CopyFrom(valueBytes);

            return new HeaderWithValue<ByteString>(header, result);
        }

        public void Encode(DataBuffer dataBuffer, ByteString? value)
        {
            Header header = new()
            {
                IsMissing = false,
                IsNull = value == null
            };

            dataBuffer.WriteBit(0);
            dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));

            if (value == null)
            {
                return;
            }

            byte[] valueBytes = value.ToByteArray();
            int length = valueBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(length);

            for (int i = lengthBytes.Length - 1; i >= 0; i--)
            {
                dataBuffer.WriteByte(lengthBytes[i]);
            }

            for(int i = valueBytes.Length - 1; i >= 0; i--)
            {
                dataBuffer.WriteByte(valueBytes[i]);
            }
        }

        public int GetLengthInBits(ByteString? value)
        {
            if (value == null) {
                return NumberOfHeaderBitsNumeric;
            }

            return value.Length * 8 + sizeof(int) + NumberOfHeaderBitsNumeric;
        }
    }
}
