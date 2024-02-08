using JerqAggregatorNew.Types;
using System.Reflection;

namespace JerqAggregatorNew.Schemas
{
    public class Schema<T> where T : new()
    {
        private List<MemberData> _memberData;
        private readonly object _lock = new object();
        public Schema()
        {
            _memberData = new List<MemberData>();
        }

        public void AddMemberData(MemberData memberData)
        {
            lock (_lock)
            {
                _memberData.Add(memberData);
            }
        }

        /// <summary>
        ///     Serialize an object of generic type
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized</param>
        /// <returns>Array of bytes that represents a result of binary serialization</returns>
        public byte[] Serialize(T schemaObject)
        {
            int offset = 0;
            int offsetInLastByte = 0;

            List<byte> buffer = new List<byte>();

            lock (_lock)
            {
                foreach (MemberData memberData in _memberData)
                {
                    if (!memberData.IsIncluded)
                    {
                        continue;
                    }

                    object? value;

                    if (memberData.MemberInfo is FieldInfo)
                    {
                        FieldInfo fieldInfo = ((FieldInfo)memberData.MemberInfo);
                        value = fieldInfo.GetValue(schemaObject);
                    }
                    else
                    {
                        PropertyInfo propertyInfo = ((PropertyInfo)memberData.MemberInfo);
                        value = propertyInfo.GetValue(schemaObject);
                    }

                    memberData.BinarySerializer.Encode(buffer, value, ref offset, ref offsetInLastByte);
                }
            }
            return buffer.ToArray();
        }

        /// <summary>
        ///      Serialize only a difference between the new and the old object
        /// </summary>
        /// <param name="firstObject">Old object of generic type</param>
        /// <param name="secondObject">New object of generic type</param>
        /// <returns>Array of bytes that represents a result of binary serialization</returns>
        public byte[] Serialize(T firstObject, T secondObject)
        {
            int offset = 0;
            int offsetInLastByte = 0;

            List<byte> buffer = new List<byte>();

            lock (_lock)
            {
                foreach (MemberData memberData in _memberData)
                {
                    if (!memberData.IsIncluded)
                    {
                        continue;
                    }

                    object? firstValue, secondValue;

                    if (memberData.MemberInfo is FieldInfo)
                    {
                        FieldInfo fieldInfo = (FieldInfo)memberData.MemberInfo;
                        firstValue = fieldInfo.GetValue(firstObject);
                        secondValue = fieldInfo.GetValue(secondObject);
                    }
                    else
                    {
                        PropertyInfo propertyInfo = (PropertyInfo)memberData.MemberInfo;
                        firstValue = propertyInfo.GetValue(firstObject);
                        secondValue = propertyInfo.GetValue(secondObject);
                    }

                    bool valuesEqual = object.Equals(firstValue, secondValue);

                    if (valuesEqual)
                    {
                        memberData.BinarySerializer.EncodeMissingFlag(buffer, ref offset, ref offsetInLastByte);
                    }
                    else
                    {
                        memberData.BinarySerializer.Encode(buffer, secondValue, ref offset, ref offsetInLastByte);
                    }
                }
            }

            return buffer.ToArray();
        }

        /// <summary>
        ///     Deserialize array of bytes into object
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized</param>
        /// <returns> Deserialized object written into newly created object of generic type </returns>
        public T Deserialize(byte[] buffer) {
            return Deserialize(buffer, new T());
        }

        /// <summary>
        ///     Deserialize array of bytes into object
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized</param>
        /// <param name="existing">Existing generic object</param>
        /// <returns> Deserialized object written into existing object of generic type </returns>
        public T Deserialize(byte[] buffer, T existing) 
        {
            // todo - check flag ismissing is set - then take value from existing object
            int offset = 0;
            int offsetInLastByte = 0;
            List<byte> bytes = buffer.ToList();

            lock (_lock)
            {
                foreach (MemberData memberData in _memberData)
                {
                    if (!memberData.IsIncluded)
                    {
                        continue;
                    }

                    HeaderWithValue value = memberData.BinarySerializer.Decode(bytes, ref offset, ref offsetInLastByte);

                    if (value.Header.IsMissing)
                    {
                        continue;
                    }

                    if (memberData.MemberInfo is FieldInfo)
                    {
                        FieldInfo fieldInfo = ((FieldInfo)memberData.MemberInfo);
                        fieldInfo.SetValue(existing, value.Value);
                    }
                    else
                    {
                        PropertyInfo propertyInfo = ((PropertyInfo)memberData.MemberInfo);
                        propertyInfo.SetValue(existing, value.Value);
                    }
                }
            }
            return existing;
        }
    }
}
