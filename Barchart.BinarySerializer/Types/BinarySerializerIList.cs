using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;

namespace Barchart.BinarySerializer.Types
{
    public class BinarySerializerIList<T, V> : IBinaryTypeObjectSerializer<T?> where T: IList<V>, new()
    {
        public const int NumberOfHeaderBitsNumeric = 2;
        private readonly IBinaryTypeSerializer<V> _serializer;

        public BinarySerializerIList(IBinaryTypeSerializer<V> serializer)
        {
            _serializer = serializer;
        }

        public void Encode(DataBuffer dataBuffer, T? value)
        {
            Header header = new() { IsMissing = false, IsNull = value == null };
            BufferHelper.WriteHeader(dataBuffer, header);

            if (value != null)
            {
                int length = value.Count;
                BufferHelper.WriteLength(dataBuffer, length);

                foreach (var item in value)
                {
                    _serializer.Encode(dataBuffer, item);
                }
            }
        }

        public void Encode(DataBuffer dataBuffer, T? oldValue, T? newValue)
        {
            Header header = new() { IsMissing = false, IsNull = newValue == null };
            BufferHelper.WriteHeader(dataBuffer, header);

            if (newValue != null)
            {
                int length = newValue.Count;
                BufferHelper.WriteLength(dataBuffer, length);

                for (int i = 0; i < newValue.Count; i++)
                {
                    if (oldValue != null && i < oldValue.Count && Equals(oldValue[i], newValue[i]))
                    {
                        BufferHelper.EncodeMissingFlag(dataBuffer);
                    }
                    else
                    {
                        _serializer.Encode(dataBuffer, newValue[i]);
                    }
                }
            }
        }

        public HeaderWithValue<T?> Decode(DataBuffer dataBuffer, T? existing)
        {
            Header header = BufferHelper.ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<T?>(header, default);
            }

            int length = BufferHelper.ReadLength(dataBuffer);
            T list = ReadList(dataBuffer, length, existing);

            return new HeaderWithValue<T?>(header, list);
        }

        public HeaderWithValue<T?> Decode(DataBuffer dataBuffer)
        {
            Header header = BufferHelper.ReadHeader(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new HeaderWithValue<T?>(header, default);
            }

            int length = BufferHelper.ReadLength(dataBuffer);
            T list = ReadList(dataBuffer, length, default);

            return new HeaderWithValue<T?>(header, list);
        }

        public int GetLengthInBits(T? value)
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

        protected T ReadList(DataBuffer dataBuffer, int length, T? existing)
        {
            T list = new();

            for (int i = 0; i < length; i++)
            {
                var headerWithValue = _serializer.Decode(dataBuffer);
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