using Barchart.BinarySerializer.Types;
using Newtonsoft.Json.Linq;

namespace Barchart.BinarySerializer.Schemas
{
    public class Schema<T> : ISchema where T : new()
    {
        readonly private static int BUFFER_SIZE = 256000000;
        readonly private static int IS_MISSING_NUMBER_OF_BITS = 1;

        [ThreadStatic]
        private static byte[]? _buffer;

        private static byte[] Buffer
        {
            get
            {
                if (_buffer == null)
                {
                    return _buffer = new byte[BUFFER_SIZE];
                }

                return _buffer;
            }
        }

        private List<MemberData<T>> _memberDataList;

        internal Schema(List<MemberData<T>> memberDataList)
        {
            _memberDataList = memberDataList;
        }

        /// <summary>
        ///     Serialize an object of generic type.
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(T schemaObject)
        {
            return Serialize(schemaObject, Buffer);
        }

        /// <summary>
        ///     Serialize an object of generic type.
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized.</param>
        /// <param name="buffer">Buffer that will be populated with array of bytes representing result of the serialization.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(T schemaObject, byte[] buffer)
        {
            DataBuffer dataBuffer = new DataBuffer(buffer);
            dataBuffer.ResetByte();

            return Serialize(schemaObject, dataBuffer);
        }

        private byte[] Serialize(T schemaObject, DataBuffer dataBuffer) {
            foreach (MemberData<T> memberData in _memberDataList)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                object? value = memberData.GetDelegate(schemaObject);

                memberData.BinarySerializer.Encode(dataBuffer, value);
            }

            return dataBuffer.ToBytes();
        }
        /// <summary>
        ///      Serialize only a difference between the new and the old object.
        /// </summary>
        /// <param name="oldObject">Old object of generic type.</param>
        /// <param name="newObject">New object of generic type.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(T oldObject, T newObject)
        {
            return Serialize(oldObject, newObject, Buffer);
        }

        /// <summary>
        ///     Serialize only a difference between the new and the old object.
        /// </summary>
        /// <param name="oldObject">Old object of generic type.</param>
        /// <param name="newObject">New object of generic type.</param>
        /// <param name="buffer">Buffer that will be populated with array of bytes representing result of the serialization.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(T oldObject, T newObject, byte[] buffer)
        {
            DataBuffer dataBuffer = new DataBuffer(buffer);
            dataBuffer.ResetByte();

            return Serialize(oldObject, newObject, dataBuffer);
        }

        private byte[] Serialize(T oldObject, T newObject, DataBuffer dataBuffer)
        {
            foreach (MemberData<T> memberData in _memberDataList)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                object? oldValue = memberData.GetDelegate(oldObject);
                object? newValue = memberData.GetDelegate(newObject);

                bool valuesEqual = Equals(oldValue, newValue);

                if (!valuesEqual || memberData.IsKeyAttribute)
                {
                    if (memberData.BinarySerializer is ObjectBinarySerializer)
                    {
                        ((ObjectBinarySerializer)memberData.BinarySerializer).Encode(dataBuffer, oldValue, newValue);
                    }
                    else
                    {
                        memberData.BinarySerializer.Encode(dataBuffer, newValue);
                    }
                }
                else
                {
                    EncodeMissingFlag(dataBuffer);
                }
            }

            return dataBuffer.ToBytes();
        }

        /// <summary>
        ///     Deserialize array of bytes into object.
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized.</param>
        /// <returns> Deserialized object written into newly created object of generic type. </returns>
        public T Deserialize(byte[] buffer)
        {
            DataBuffer dataBuffer  = new DataBuffer(buffer);
            return Deserialize(dataBuffer);
        }

        private T Deserialize(DataBuffer dataBuffer) {
            T existing = new T();

            foreach (MemberData<T> memberData in _memberDataList)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                HeaderWithValue value = memberData.BinarySerializer.Decode(dataBuffer);

                if (value.Header.IsMissing)
                {
                    continue;
                }

                memberData.SetDelegate(existing, value.Value);     
            }

            return existing;
        }

        /// <summary>
        ///     Deserialize array of bytes into object.
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized.</param>
        /// <param name="existing">Existing generic object.</param>
        /// <returns> Deserialized object written into existing object of generic type. </returns>
        public T Deserialize(byte[] buffer, T existing)
        {
            DataBuffer dataBuffer = new DataBuffer(buffer); 
            return Deserialize(existing, dataBuffer);
        }

        private T Deserialize(T existing, DataBuffer dataBuffer)
        {
            foreach (MemberData<T> memberData in _memberDataList)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                HeaderWithValue value = new HeaderWithValue();

                if (memberData.BinarySerializer is ObjectBinarySerializer)
                {
                    object? currentObject = memberData.GetDelegate(existing);
                    value = ((ObjectBinarySerializer)memberData.BinarySerializer).Decode(dataBuffer, currentObject);
                }
                else
                {
                    value = memberData.BinarySerializer.Decode(dataBuffer);
                }

                if (value.Header.IsMissing)
                {
                    continue;
                }

                memberData.SetDelegate(existing, value.Value);
            }

            return existing;
        }

        /// <summary>
        ///     Calculates the total length of the binary representation of the provided schema object in bytes.
        /// </summary>
        /// <param name="schemaObject">The schema object to calculate the length for.</param>
        /// <returns> The total length of the binary representation of the schema object in bytes. </returns>
        public int GetLengthInBytes(T schemaObject)
        {
            return (int)Math.Ceiling((double)GetLengthInBits(schemaObject) / 8);
        }

        /// <summary>
        ///     Calculates the total length of the binary representation of the difference between the provided schema objects in bytes.
        /// </summary>
        /// <param name="oldObject">The old schema object.</param>
        /// <param name="newObject">The new schema object.</param>
        /// <returns> The total length of the binary representation of the difference between the provided schema objects in bytes. </returns>
        public int GetLengthInBytes(T oldObject, T newObject)
        {
            return (int)Math.Ceiling((double)GetLengthInBits(oldObject, newObject) / 8);
        }

        /// <summary>
        ///     Calculates the total length of the binary representation of the provided schema object in bits.
        /// </summary>
        /// <param name="schemaObject">The schema object to calculate the length for.</param>
        /// <returns> The total length of the binary representation of the schema object in bits. </returns>
        public int GetLengthInBits(T schemaObject)
        {
            int lengthInBits = 0;
            foreach (MemberData<T> memberData in _memberDataList)
            {
                object? value = memberData.GetDelegate(schemaObject);
                lengthInBits += memberData.BinarySerializer.GetLengthInBits(value);             
            }

            return lengthInBits;
        }

        /// <summary>
        ///     Calculates the total length of the binary representation of the difference between the provided schema objects in bits.
        /// </summary>
        /// <param name="oldObject">The old schema object to calculate the length for.</param>
        /// <param name="newObject">The new schema object to calculate the length for.</param>
        /// <returns> The total length of the binary representation of the difference between the provided schema objects in bits. </returns>
        public int GetLengthInBits(T oldObject, T newObject)
        {
            int lengthInBits = 0;
            foreach (MemberData<T> memberData in _memberDataList)
            {
                object? oldValue = memberData.GetDelegate(oldObject);
                object? newValue = memberData.GetDelegate(newObject);

                bool valuesEqual = Equals(oldValue, newValue);

                if (!valuesEqual || memberData.IsKeyAttribute)
                {
                    lengthInBits += memberData.BinarySerializer.GetLengthInBits(newValue);
                }
                else
                {
                    lengthInBits += IS_MISSING_NUMBER_OF_BITS;
                }
            }

            return lengthInBits;
        }

        private void EncodeMissingFlag(DataBuffer dataBuffer)
        {
            dataBuffer.WriteBit(1);
        }

        #region ISchema implementation
        byte[] ISchema.Serialize(object schemaObject)
        {
            return Serialize((T)schemaObject);
        }

        byte[] ISchema.Serialize(object schemaObject, byte[] buffer)
        {
            return Serialize((T)schemaObject, buffer);
        }

        byte[] ISchema.Serialize(object oldObject, object newObject)
        {
            return Serialize((T)oldObject, (T)newObject);
        }

        byte[] ISchema.Serialize(object oldObject, object newObject, byte[] buffer)
        {
            return Serialize((T)oldObject, (T)newObject, buffer);
        }

        byte[] ISchema.Serialize(object schemaObject, DataBuffer dataBuffer)
        {
            return Serialize((T)schemaObject, dataBuffer);
        }

        byte[] ISchema.Serialize(object oldObject, object newObject, DataBuffer dataBuffer)
        {
            return Serialize((T)oldObject, (T)newObject, dataBuffer);
        }

        object? ISchema.Deserialize(byte[] buffer)
        {
            return Deserialize(buffer);
        }

        object? ISchema.Deserialize(byte[] buffer, object existing)
        {
            return Deserialize(buffer, (T)existing);
        }

        object? ISchema.Deserialize(DataBuffer dataBuffer)
        {
            return Deserialize(dataBuffer);
        }

        object? ISchema.Deserialize(object existing, DataBuffer dataBuffer)
        {
            return Deserialize((T)existing, dataBuffer);
        }

        int ISchema.GetLengthInBytes(object? schemaObject)
        {
            if (schemaObject == null)
            {
                return 0;
            }
            return GetLengthInBytes((T)schemaObject);
        }

        int ISchema.GetLengthInBits(object? schemaObject)
        {
            if (schemaObject == null)
            {
                return 0;
            }
            return GetLengthInBits((T)schemaObject);
        }
        #endregion
    }
}