#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Provides binary serialization functionality for objects implementing the IList interface.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container implementing the IList interface.</typeparam>
    /// <typeparam name="T">The type of elements contained in the IList.</typeparam>
    public class BinarySerializerIList<TContainer, T> : IBinaryTypeObjectSerializer<TContainer?> where TContainer: IList<T>, new()
    {
        #region Fields

        private readonly IBinaryTypeSerializer<T> _serializer;

        #endregion

        #region Constructor(s)

        public BinarySerializerIList(IBinaryTypeSerializer<T> serializer)
        {
            _serializer = serializer;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, TContainer? value)
        {
            Header.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = value == null });

            if (value != null)
            {
                int length = value.Count;
                
                dataBuffer.WriteBytes(BitConverter.GetBytes(length));

                foreach (var item in value)
                {
                    _serializer.Encode(dataBuffer, item);
                }
            }
        }

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, TContainer? oldValue, TContainer? newValue)
        {
            Header.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = newValue == null });

            if (newValue != null)
            {
                int length = newValue.Count;
                
                dataBuffer.WriteBytes(BitConverter.GetBytes(length));

                for (int i = 0; i < newValue.Count; i++)
                {
                    if (oldValue != null && i < oldValue.Count && Equals(oldValue[i], newValue[i]))
                    {
                        dataBuffer.WriteBit(true); // missing ...
                    }
                    else
                    {
                        _serializer.Encode(dataBuffer, newValue[i]);
                    }
                }
            }
        }

        /// <inheritdoc />
        public Attribute<TContainer?> Decode(IDataBuffer dataBuffer, TContainer? existing)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new Attribute<TContainer?>(header, default);
            }

            int length = BitConverter.ToInt32(dataBuffer.ReadBytes(sizeof(int)));
            
            TContainer list = ReadList(dataBuffer, length, existing);

            return new Attribute<TContainer?>(header, list);
        }

        /// <inheritdoc />
        public Attribute<TContainer?> Decode(IDataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new Attribute<TContainer?>(header, default);
            }

            int length = BitConverter.ToInt32(dataBuffer.ReadBytes(sizeof(int)));
            
            TContainer list = ReadList(dataBuffer, length, default);

            return new Attribute<TContainer?>(header, list);
        }

        /// <inheritdoc />
        public int GetLengthInBits(TContainer? value)
        {
            if (value == null)
            {
                return Header.NumberOfHeaderBitsNonString;
            }

            int length = Header.NumberOfHeaderBitsNonString;

            foreach (var item in value)
            {
                length += Header.NumberOfHeaderBitsNonString;

                if (item != null)
                {
                    length += _serializer.GetLengthInBits(item);
                }
            }

            return length;
        }

        protected TContainer ReadList(IDataBuffer dataBuffer, int length, TContainer? existing)
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