using Barchart.BinarySerializer.Schemas;
using Google.Protobuf;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerList<T> : IBinaryTypeSerializer<List<T>?>
    {
        public const int NumberOfHeaderBitsNumeric = 2;
        private readonly IBinaryTypeSerializer<T> _serializer;

        public BinarySerializerList(IBinaryTypeSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        public HeaderWithValue<List<T>> Decode(DataBuffer dataBuffer)
        {
            Header header = new()
            {
                IsMissing = dataBuffer.ReadBit() == 1
            };

            if (header.IsMissing)
            {
                return new HeaderWithValue<List<T>>(header, default);
            }

            header.IsNull = dataBuffer.ReadBit() == 1;

            if (header.IsNull)
            {
                return new HeaderWithValue<List<T>>(header, default);
            }

            byte[] lengthBytes = new byte[sizeof(int)];

            for (int i = lengthBytes.Length - 1; i >= 0; i--)
            {
                lengthBytes[i] = dataBuffer.ReadByte();
            }

            int length = BitConverter.ToInt32(lengthBytes, 0);

            List<T> list = new();

            for (int i = 0; i < length; i++)
            {
                T? value = _serializer.Decode(dataBuffer).Value;
                if(value != null) list.Add(value);
            }

            return new HeaderWithValue<List<T>>(header, list);
        }

        public void Encode(DataBuffer dataBuffer, List<T>? value)
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

            int length = value.Count;
            byte[] lengthBytes = BitConverter.GetBytes(length);

            for (int i = lengthBytes.Length - 1; i >= 0; i--)
            {
                dataBuffer.WriteByte(lengthBytes[i]);
            }

            foreach (var item in value)
            {
                _serializer.Encode(dataBuffer, item);
            }
        }

        public int GetLengthInBits(List<T>? value)
        {
            if (value == null)
            {
                return NumberOfHeaderBitsNumeric;
            }

            int length = 0;

            length += NumberOfHeaderBitsNumeric;

            foreach (object? item in value)
            {
                length += NumberOfHeaderBitsNumeric;

                if (item != null)
                {
                    length += _serializer.GetLengthInBits((T)item);
                }
            }

            return length;
        }
    }
}