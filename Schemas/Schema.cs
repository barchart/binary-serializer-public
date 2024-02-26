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

        public void AddMemberData(MemberData<T> memberData)
        {
            _memberData.Add(memberData);
        }

        /// <summary>
        ///     Serialize an object of generic type
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized</param>
        /// <returns>Array of bytes that represents a result of binary serialization</returns>
        public byte[] Serialize(T schemaObject)
        {
            return Serialize(schemaObject, _buffer);
        }

        /// <summary>
        ///     Serialize an object of generic type
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized</param>
        /// <param name="buffer">Buffer that will be populated with array of bytes representing result of the serialization</param>
        /// <returns>Array of bytes that represents a result of binary serialization</returns>
        public byte[] Serialize(T schemaObject, byte[] buffer)
        {
            int offset = 0;
            int offsetInLastByte = 0;
            buffer[offset] = 0;
            return Serialize(schemaObject, buffer, ref offset, ref offsetInLastByte);
        }

        byte[] Serialize(T schemaObject, byte[] buffer, ref int offset, ref int offsetInLastByte) {
            foreach (MemberData<T> memberData in _memberData)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                object? value;

                if (memberData.MemberInfo is FieldInfo)
                {
                    FieldInfo fieldInfo = ((FieldInfo)memberData.MemberInfo);
                    value = memberData.GetDelegate(schemaObject);
                }
                else
                {
                    PropertyInfo propertyInfo = ((PropertyInfo)memberData.MemberInfo);
                    value = memberData.GetDelegate(schemaObject);
                }

                memberData.BinarySerializer.Encode(buffer, value, ref offset, ref offsetInLastByte);
            }

            return buffer.Take(offset + 1).ToArray();
        }
        /// <summary>
        ///      Serialize only a difference between the new and the old object
        /// </summary>
        /// <param name="oldObject">Old object of generic type</param>
        /// <param name="newObject">New object of generic type</param>
        /// <returns>Array of bytes that represents a result of binary serialization</returns>
        public byte[] Serialize(T oldObject, T newObject)
        {
            return Serialize(oldObject, newObject, _buffer);
        }

        /// <summary>
        ///     Serialize only a difference between the new and the old object
        /// </summary>
        /// <param name="oldObject">Old object of generic type</param>
        /// <param name="newObject">New object of generic type</param>
        /// <param name="buffer"></param>
        /// <returns>Array of bytes that represents a result of binary serialization</returns>
        public byte[] Serialize(T oldObject, T newObject, byte[] buffer)
        {
            int offset = 0;
            int offsetInLastByte = 0;
            buffer[offset] = 0;
            return Serialize(oldObject, newObject, _buffer, ref offset, ref offsetInLastByte);
        }

        public byte[] Serialize(T oldObject, T newObject, byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            foreach (MemberData<T> memberData in _memberData)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                object? oldValue, newValue;

                if (memberData.MemberInfo is FieldInfo)
                {
                    FieldInfo fieldInfo = (FieldInfo)memberData.MemberInfo;
                    oldValue = memberData.GetDelegate(oldObject);
                    newValue = memberData.GetDelegate(newObject);
                }
                else
                {
                    PropertyInfo propertyInfo = (PropertyInfo)memberData.MemberInfo;
                    oldValue = memberData.GetDelegate(oldObject);
                    newValue = memberData.GetDelegate(newObject);
                }

                bool valuesEqual = Equals(oldValue, newValue);

                if (!valuesEqual || memberData.IsKeyAttribute)
                {
                    if (memberData.BinarySerializer is ObjectBinarySerializer)
                    {
                        ((ObjectBinarySerializer)memberData.BinarySerializer).Encode(buffer, oldValue, newValue, ref offset, ref offsetInLastByte);
                    }
                    else
                    {
                        memberData.BinarySerializer.Encode(buffer, newValue, ref offset, ref offsetInLastByte);
                    }
                }
                else
                {
                    EncodeMissingFlag(buffer, ref offset, ref offsetInLastByte);
                }
            }

            return buffer.Take(offset + 1).ToArray();
        }

        /// <summary>
        ///     Deserialize array of bytes into object
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized</param>
        /// <returns> Deserialized object written into newly created object of generic type </returns>
        public T Deserialize(byte[] buffer)
        {
            int offset = 0;
            int offsetInLastByte = 0;
            return Deserialize(buffer, ref offset, ref offsetInLastByte);
        }

        public T Deserialize(byte[] buffer, ref int offset, ref int offsetInLastByte) {
            T existing = new T();

            foreach (MemberData<T> memberData in _memberData)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                HeaderWithValue value = new HeaderWithValue();

                value = memberData.BinarySerializer.Decode(buffer, ref offset, ref offsetInLastByte);

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
        ///     Deserialize array of bytes into object
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized</param>
        /// <param name="existing">Existing generic object</param>
        /// <returns> Deserialized object written into existing object of generic type </returns>
        public T Deserialize(byte[] buffer, T existing)
        {
            int offset = 0;
            int offsetInLastByte = 0;

            return Deserialize(buffer, existing, ref offset, ref offsetInLastByte);
        }

        public T Deserialize(byte[] buffer, T existing, ref int offset, ref int offsetInLastByte)
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
                    object? currentObject;

                    if (memberData.MemberInfo is FieldInfo)
                    {
                        FieldInfo fieldInfo = (FieldInfo)memberData.MemberInfo;
                        currentObject = memberData.GetDelegate(existing);
                    }
                    else
                    {
                        PropertyInfo propertyInfo = (PropertyInfo)memberData.MemberInfo;
                        currentObject = memberData.GetDelegate(existing);
                    }

                    value = ((ObjectBinarySerializer)memberData.BinarySerializer).Decode(buffer, currentObject, ref offset, ref offsetInLastByte);
                }
                else
                {
                    value = memberData.BinarySerializer.Decode(buffer, ref offset, ref offsetInLastByte);
                }

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


        private void EncodeMissingFlag(byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            buffer.WriteBit(1, ref offset, ref offsetInLastByte);
        }

        #region ISchema
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

        byte[] ISchema.Serialize(object schemaObject, byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            return Serialize((T)schemaObject, buffer, ref offset, ref offsetInLastByte);
        }

        byte[] ISchema.Serialize(object oldObject, object newObject, byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            return Serialize((T)oldObject, (T)newObject, buffer, ref offset, ref offsetInLastByte);
        }

        object ISchema.Deserialize(byte[] buffer)
        {
            return Deserialize(buffer);
        }

        object ISchema.Deserialize(byte[] buffer, object existing)
        {
            return Deserialize(buffer, (T)existing);
        }

        object ISchema.Deserialize(byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            return Deserialize(buffer, ref offset, ref offsetInLastByte);
        }

        object ISchema.Deserialize(byte[] buffer, object existing, ref int offset, ref int offsetInLastByte)
        {
            return Deserialize(buffer, (T)existing, ref offset, ref offsetInLastByte);
        }

        #endregion
    }

    public static class BufferHelper
    {
        public static void WriteBit(this byte[] buffer, byte bit, ref int offset, ref int offsetInLastByte)
        {
            buffer[offset] |= (byte)(bit << (7 - offsetInLastByte));
            offsetInLastByte = (offsetInLastByte + 1) % 8;

            if (offsetInLastByte == 0)
            {
                offset++;

                if (offset >= buffer.Length)
                {
                    throw new Exception($"Object is larger then {buffer.Length} bytes.");
                }

                buffer[offset] = 0;
            }
        }

        public static byte ReadBit(this byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            byte bit = (byte)((buffer[offset] >> (7 - offsetInLastByte)) & 1);
            offsetInLastByte = (offsetInLastByte + 1) % 8;

            if (offsetInLastByte == 0)
            {
                offset++;
            }

            return bit;
        }

        public static void WriteByte(this byte[] buffer, byte valueByte, ref int offset, ref int offsetInLastByte)
        {
            for (int j = 7; j >= 0; j--)
            {
                buffer.WriteBit((byte)((valueByte >> j) & 1), ref offset, ref offsetInLastByte);
            }
        }
        public static byte ReadByte(this byte[] buffer, ref int offset, ref int offsetInLastByte)
        {
            byte byteToAdd = 0;

            for (int j = 7; j >= 0; j--)
            {
                byte bit = buffer.ReadBit(ref offset, ref offsetInLastByte);
                byteToAdd |= (byte)(bit << j);
            }

            return byteToAdd;
        }
    }
}