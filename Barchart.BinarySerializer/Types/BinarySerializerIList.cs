#region Using Statements

using Barchart.BinarySerializer.Schemas;

#endregion

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
        #region Fields

        private readonly IBinaryTypeSerializer<TMember> _serializer;

        #endregion

        #region Constructor(s)

        public BinarySerializerIList(IBinaryTypeSerializer<TMember> serializer)
        {
            _serializer = serializer;
        }

        #endregion

        #region Methods

        public void Encode(DataBuffer dataBuffer, TContainer? value)
        {
            Header header = new() { IsMissing = false, IsNull = value == null };
            
            header.WriteToBuffer(dataBuffer);

            if (value != null)
            {
                int length = value.Count;
                dataBuffer.WriteLength(length);

                foreach (var item in value)
                {
                    _serializer.Encode(dataBuffer, item);
                }
            }
        }

        public void Encode(DataBuffer dataBuffer, TContainer? oldValue, TContainer? newValue)
        {
            Header header = new() { IsMissing = false, IsNull = newValue == null };
            
            header.WriteToBuffer(dataBuffer);

            if (newValue != null)
            {
                int length = newValue.Count;
                dataBuffer.WriteLength(length);

                for (int i = 0; i < newValue.Count; i++)
                {
                    if (oldValue != null && i < oldValue.Count && Equals(oldValue[i], newValue[i]))
                    {
                        dataBuffer.EncodeMissingFlag();
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
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsValueMissingOrNull())
            {
                return new HeaderWithValue<TContainer?>(header, default);
            }

            int length = dataBuffer.ReadLength();
            TContainer list = ReadList(dataBuffer, length, existing);

            return new HeaderWithValue<TContainer?>(header, list);
        }

        public HeaderWithValue<TContainer?> Decode(DataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsValueMissingOrNull())
            {
                return new HeaderWithValue<TContainer?>(header, default);
            }

            int length = dataBuffer.ReadLength();
            TContainer list = ReadList(dataBuffer, length, default);

            return new HeaderWithValue<TContainer?>(header, list);
        }

        public int GetLengthInBits(TContainer? value)
        {
            if (value == null)
            {
                return DataBuffer.NumberOfHeaderBitsNonString;
            }

            int length = DataBuffer.NumberOfHeaderBitsNonString;

            foreach (var item in value)
            {
                length += DataBuffer.NumberOfHeaderBitsNonString;

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

        #endregion
    }
}