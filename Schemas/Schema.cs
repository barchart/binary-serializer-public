using JerqAggregatorNew.Types;
using System.Reflection;

namespace JerqAggregatorNew.Schemas
{
    public class Schema<T> : ISchema where T : new()
    {
        static int BUFFER_SIZE = 256000000;

        [ThreadStatic]
        static byte[] _buffer = new byte[BUFFER_SIZE];

        private List<MemberData<T>> _memberData;

        public Schema()
        {
            _memberData = new List<MemberData<T>>();
        }

        internal void AddMemberData(MemberData<T> memberData)
        {
            _memberData.Add(memberData);
        }

        /// <summary>
        ///     Serialize an object of generic type.
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(T schemaObject)
        {
            return Serialize(schemaObject, _buffer);
        }

        /// <summary>
        ///     Serialize an object of generic type.
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized.</param>
        /// <param name="buffer">Buffer that will be populated with array of bytes representing result of the serialization.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(T schemaObject, byte[] buffer)
        {
            BufferHelper bufferHelper = new BufferHelper(buffer);
            bufferHelper._buffer[bufferHelper._offset] = 0;

            return Serialize(schemaObject, bufferHelper);
        }

        internal byte[] Serialize(T schemaObject, BufferHelper bufferHelper) {
            foreach (MemberData<T> memberData in _memberData)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                object? value = memberData.GetDelegate(schemaObject);

                memberData.BinarySerializer.Encode(bufferHelper, value);
            }

            return bufferHelper._buffer.Take(bufferHelper._offset + 1).ToArray();
        }
        /// <summary>
        ///      Serialize only a difference between the new and the old object.
        /// </summary>
        /// <param name="oldObject">Old object of generic type.</param>
        /// <param name="newObject">New object of generic type.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(T oldObject, T newObject)
        {
            return Serialize(oldObject, newObject, _buffer);
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
            BufferHelper bufferHelper = new BufferHelper(buffer);
            bufferHelper._buffer[bufferHelper._offset] = 0;

            return Serialize(oldObject, newObject, bufferHelper);
        }

        internal byte[] Serialize(T oldObject, T newObject, BufferHelper bufferHelper)
        {
            foreach (MemberData<T> memberData in _memberData)
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
                        ((ObjectBinarySerializer)memberData.BinarySerializer).Encode(bufferHelper, oldValue, newValue);
                    }
                    else
                    {
                        memberData.BinarySerializer.Encode(bufferHelper, newValue);
                    }
                }
                else
                {
                    EncodeMissingFlag(bufferHelper);
                }
            }

            return bufferHelper._buffer.Take(bufferHelper._offset + 1).ToArray();
        }

        /// <summary>
        ///     Deserialize array of bytes into object.
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized.</param>
        /// <returns> Deserialized object written into newly created object of generic type. </returns>
        public T Deserialize(byte[] buffer)
        {
            BufferHelper bufferHelper  = new BufferHelper(buffer);
            return Deserialize(bufferHelper);
        }

        internal T Deserialize(BufferHelper bufferHelper) {
            T existing = new T();

            foreach (MemberData<T> memberData in _memberData)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                HeaderWithValue value = new HeaderWithValue();

                value = memberData.BinarySerializer.Decode(bufferHelper);

                if (value.Header.IsMissing)
                {
                    continue;
                }

                if (memberData.MemberInfo is FieldInfo)
                {
                    FieldInfo fieldInfo = ((FieldInfo)memberData.MemberInfo);
                    memberData.SetDelegate(existing, value.Value);
                }
                else
                {
                    PropertyInfo propertyInfo = ((PropertyInfo)memberData.MemberInfo);
                    memberData.SetDelegate(existing, value.Value);
                }
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
            BufferHelper bufferHelper = new BufferHelper(buffer); 
            return Deserialize(existing, bufferHelper);
        }

        internal T Deserialize(T existing, BufferHelper bufferHelper)
        {
            foreach (MemberData<T> memberData in _memberData)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                HeaderWithValue value = new HeaderWithValue();

                if (memberData.BinarySerializer is ObjectBinarySerializer)
                {
                    object? currentObject = memberData.GetDelegate(existing);
                    value = ((ObjectBinarySerializer)memberData.BinarySerializer).Decode(bufferHelper, currentObject);
                }
                else
                {
                    value = memberData.BinarySerializer.Decode(bufferHelper);
                }

                if (value.Header.IsMissing)
                {
                    continue;
                }

                memberData.SetDelegate(existing, value.Value);
            }

            return existing;
        }

        private void EncodeMissingFlag(BufferHelper bufferHelper)
        {
            bufferHelper.WriteBit(1);
        }

        #region ISchema implementation
        public byte[] Serialize(object schemaObject)
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

        byte[] ISchema.Serialize(object schemaObject, BufferHelper bufferHelper)
        {
            return Serialize((T)schemaObject, bufferHelper);
        }

        byte[] ISchema.Serialize(object oldObject, object newObject, BufferHelper bufferHelper)
        {
            return Serialize((T)oldObject, (T)newObject, bufferHelper);
        }

        object? ISchema.Deserialize(byte[] buffer)
        {
            return Deserialize(buffer);
        }

        object? ISchema.Deserialize(byte[] buffer, object existing)
        {
            return Deserialize(buffer, (T)existing);
        }

        object? ISchema.Deserialize(BufferHelper bufferHelper)
        {
            return Deserialize(bufferHelper);
        }

        object? ISchema.Deserialize(object existing, BufferHelper bufferHelper)
        {
            return Deserialize((T)existing, bufferHelper);
        }

        #endregion
    }

    public class BufferHelper
    {
        public byte[] _buffer;
        public int _offset;
        public int _offsetInLastByte;

        public BufferHelper(byte[] buffer)
        {
            _buffer = buffer;
            _offset = 0;
            _offsetInLastByte = 0;
        }

        public void WriteBit(byte bit)
        {
            _buffer[_offset] |= (byte)(bit << (7 - _offsetInLastByte));
            _offsetInLastByte = (_offsetInLastByte + 1) % 8;

            if (_offsetInLastByte == 0)
            {
                _offset++;

                if (_offset >= _buffer.Length)
                {
                    throw new Exception($"Object is larger then {_buffer.Length} bytes.");
                }

                _buffer[_offset] = 0;
            }
        }

        public byte ReadBit()
        {
            byte bit = (byte)((_buffer[_offset] >> (7 - _offsetInLastByte)) & 1);
            _offsetInLastByte = (_offsetInLastByte + 1) % 8;

            if (_offsetInLastByte == 0)
            {
                _offset++;
            }

            return bit;
        }

        public void WriteByte(byte valueByte)
        {
            for (int j = 7; j >= 0; j--)
            {
                WriteBit((byte)((valueByte >> j) & 1));
            }
        }

        public byte ReadByte()
        {
            byte byteToAdd = 0;

            for (int j = 7; j >= 0; j--)
            {
                byte bit = ReadBit();
                byteToAdd |= (byte)(bit << j);
            }

            return byteToAdd;
        }
    }

    //public static class BufferHelper
    //{
    //    public static void WriteBit(this byte[] buffer, byte bit, ref int offset, ref int offsetInLastByte)
    //    {
    //        buffer[offset] |= (byte)(bit << (7 - offsetInLastByte));
    //        offsetInLastByte = (offsetInLastByte + 1) % 8;

    //        if (offsetInLastByte == 0)
    //        {
    //            offset++;

    //            if (offset >= buffer.Length)
    //            {
    //                throw new Exception($"Object is larger then {buffer.Length} bytes.");
    //            }

    //            buffer[offset] = 0;
    //        }
    //    }

    //    public static byte ReadBit(this byte[] buffer, ref int offset, ref int offsetInLastByte)
    //    {
    //        byte bit = (byte)((buffer[offset] >> (7 - offsetInLastByte)) & 1);
    //        offsetInLastByte = (offsetInLastByte + 1) % 8;

    //        if (offsetInLastByte == 0)
    //        {
    //            offset++;
    //        }

    //        return bit;
    //    }

    //    public static void WriteByte(this byte[] buffer, byte valueByte, ref int offset, ref int offsetInLastByte)
    //    {
    //        for (int j = 7; j >= 0; j--)
    //        {
    //            buffer.WriteBit((byte)((valueByte >> j) & 1), ref offset, ref offsetInLastByte);
    //        }
    //    }
    //    public static byte ReadByte(this byte[] buffer, ref int offset, ref int offsetInLastByte)
    //    {
    //        byte byteToAdd = 0;

    //        for (int j = 7; j >= 0; j--)
    //        {
    //            byte bit = buffer.ReadBit(ref offset, ref offsetInLastByte);
    //            byteToAdd |= (byte)(bit << j);
    //        }

    //        return byteToAdd;
    //    }
    //}
}