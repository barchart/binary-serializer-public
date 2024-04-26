using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Provides binary serialization functionality for objects implementing the IList interface.
    /// This class implements the IBinaryTypeObjectSerializer interface for IList types.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container implementing the IList interface.</typeparam>
    /// <typeparam name="TMember">The type of elements contained in the IList.</typeparam>
    public class BinarySerializerIList<TContainer, TMember> : IBinaryTypeObjectSerializer<TContainer?> where TContainer: IList<TMember>, new()
    {
        private readonly IBinaryTypeSerializer<TMember> _serializer;

        public BinarySerializerIList(IBinaryTypeSerializer<TMember> serializer)
        {
            _serializer = serializer;
        }

        public void Encode(DataBuffer dataBuffer, TContainer? value)
        {
            Header header = new() { IsMissing = false, IsNull = value == null };
            UtilityKit.WriteHeader(dataBuffer, header);

            if (value != null)
            {
                int length = value.Count;
                UtilityKit.WriteLength(dataBuffer, length);

                foreach (var item in value)
                {
                    _serializer.Encode(dataBuffer, item);
                }
            }
        }

        public void Encode(DataBuffer dataBuffer, TContainer? oldValue, TContainer? newValue)
        {
            Header header = new() { IsMissing = false, IsNull = newValue == null };
            UtilityKit.WriteHeader(dataBuffer, header);

            if (newValue != null)
            {
                int length = newValue.Count;
                UtilityKit.WriteLength(dataBuffer, length);

                for (int i = 0; i < newValue.Count; i++)
                {
                    if (oldValue != null && i < oldValue.Count && Equals(oldValue[i], newValue[i]))
                    {
                        UtilityKit.EncodeMissingFlag(dataBuffer);
                    }
                    else
                    {
                        _serializer.Encode(dataBuffer, newValue[i]);
                    }
                }
            }
        }

        public HeaderWithValue<TContainer?> Decode(DataBuffer dataBuffer, TContainer? existing)
        {
            Header header = UtilityKit.ReadHeader(dataBuffer);

            if (UtilityKit.IsHeaderMissingOrNull(header))
            {
                return new HeaderWithValue<TContainer?>(header, default);
            }

            int length = UtilityKit.ReadLength(dataBuffer);
            TContainer list = ReadList(dataBuffer, length, existing);

            return new HeaderWithValue<TContainer?>(header, list);
        }

        public HeaderWithValue<TContainer?> Decode(DataBuffer dataBuffer)
        {
            Header header = UtilityKit.ReadHeader(dataBuffer);

            if (UtilityKit.IsHeaderMissingOrNull(header))
            {
                return new HeaderWithValue<TContainer?>(header, default);
            }

            int length = UtilityKit.ReadLength(dataBuffer);
            TContainer list = ReadList(dataBuffer, length, default);

            return new HeaderWithValue<TContainer?>(header, list);
        }

        public int GetLengthInBits(TContainer? value)
        {
            if (value == null)
            {
                return UtilityKit.NumberOfHeaderBitsNonString;
            }

            int length = UtilityKit.NumberOfHeaderBitsNonString;

            foreach (var item in value)
            {
                length += UtilityKit.NumberOfHeaderBitsNonString;

                if (item != null)
                {
                    length += _serializer.GetLengthInBits(item);
                }
            }

            return length;
        }

        protected TContainer ReadList(DataBuffer dataBuffer, int length, TContainer? existing)
        {
            TContainer list = new();

            for (int i = 0; i < length; i++)
            {
                var headerWithValue = _serializer.Decode(dataBuffer);
                var value = headerWithValue.Value;
                var header = headerWithValue.Header;

                if (!header.IsMissing && value != null)
                {
                    list.Add(value);
                }
                else if (existing != null)
                {
                    list.Add(existing[i]);
                }
            }

            return list;
        }
    }
}