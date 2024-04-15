using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerList<T> : IBinaryTypeObjectSerializer<List<T>?>
    {
        public const int NumberOfHeaderBitsNumeric = 2;
        private readonly IBinaryTypeSerializer<T> _serializer;

        public BinarySerializerList(IBinaryTypeSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        public void Encode(DataBuffer dataBuffer, List<T>? value)
        {
            Header header = new() { IsMissing = false, IsNull = value == null };
            BinarySerializerList<T>.WriteHeader(dataBuffer, header);

            if (value != null)
            {
                int length = value.Count;
                BinarySerializerList<T>.WriteLength(dataBuffer, length);

                foreach (var item in value)
                {
                    _serializer.Encode(dataBuffer, item);
                }
            }
        }

        public void Encode(DataBuffer dataBuffer, List<T>? oldValue, List<T>? newValue)
        {
            Header header = new() { IsMissing = false, IsNull = newValue == null };
            BinarySerializerList<T>.WriteHeader(dataBuffer, header);

            if (newValue != null)
            {
                int length = newValue.Count;
                BinarySerializerList<T>.WriteLength(dataBuffer, length);

                for (int i = 0; i < newValue.Count; i++)
                {
                    if (oldValue != null && i < oldValue.Count && Equals(oldValue[i], newValue[i]))
                    {
                        BinarySerializerList<T>.EncodeMissingFlag(dataBuffer);
                    }
                    else
                    {
                        _serializer.Encode(dataBuffer, newValue[i]);
                    }
                }
            }
        }

        public HeaderWithValue<List<T>?> Decode(DataBuffer dataBuffer, List<T>? existing)
        {
            Header header = BinarySerializerList<T>.ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<List<T>?>(header, default);
            }

            int length = BinarySerializerList<T>.ReadLength(dataBuffer);
            List<T> list = ReadList(dataBuffer, length, existing);

            return new HeaderWithValue<List<T>?>(header, list);
        }

        public HeaderWithValue<List<T>?> Decode(DataBuffer dataBuffer)
        {
            Header header = BinarySerializerList<T>.ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<List<T>?>(header, default);
            }

            int length = BinarySerializerList<T>.ReadLength(dataBuffer);
            List<T> list = ReadList(dataBuffer, length, null);

            return new HeaderWithValue<List<T>?>(header, list);
        }
        
        public int GetLengthInBits(List<T>? value)
        {
            if (value == null)
            {
                return NumberOfHeaderBitsNumeric;
            }

            int length = NumberOfHeaderBitsNumeric;

            foreach (var item in value)
            {
                length += NumberOfHeaderBitsNumeric;

                if (item != null)
                {
                    length += _serializer.GetLengthInBits(item);
                }
            }

            return length;
        }

        private static void EncodeMissingFlag(DataBuffer dataBuffer)
        {
            dataBuffer.WriteBit(1);
        }

        private static Header ReadHeader(DataBuffer dataBuffer)
        {
            Header header = new() { IsMissing = dataBuffer.ReadBit() == 1 };

            if (!header.IsMissing)
            {
                header.IsNull = dataBuffer.ReadBit() == 1;
            }

            return header;
        }

        private static void WriteHeader(DataBuffer dataBuffer, Header header)
        {
            dataBuffer.WriteBit((byte)(header.IsMissing ? 1 : 0));

            if (!header.IsMissing)
            {
                dataBuffer.WriteBit((byte)(header.IsNull ? 1 : 0));
            }
        }

        private static int ReadLength(DataBuffer dataBuffer)
        {
            byte[] lengthBytes = new byte[sizeof(int)];

            for (int i = lengthBytes.Length - 1; i >= 0; i--)
            {
                lengthBytes[i] = dataBuffer.ReadByte();
            }

            return BitConverter.ToInt32(lengthBytes, 0);
        }

        private static void WriteLength(DataBuffer dataBuffer, int length)
        {
            byte[] lengthBytes = BitConverter.GetBytes(length);

            for (int i = lengthBytes.Length - 1; i >= 0; i--)
            {
                dataBuffer.WriteByte(lengthBytes[i]);
            }
        }

        private List<T> ReadList(DataBuffer dataBuffer, int length, List<T>? existing = null)
        {
            List<T> list = new();

            for (int i = 0; i < length; i++)
            {
                var headerWithValue  = _serializer.Decode(dataBuffer);
                var value = headerWithValue.Value;
                var header = headerWithValue.Header;

                if (header.IsMissing)
                {
                    if (existing != null)
                    {
                        list.Add(existing[i]);
                        continue;
                    }
                }

                if (value != null) list.Add(value);
            }

            return list;
        }
    }
}