using JerqAggregatorNew.Types;
using System.Reflection;

namespace JerqAggregatorNew.Schemas
{
    public class Schema<T> where T : new()
    {
        static int BUFFER_SIZE = 256000000;

        [ThreadStatic]
        static byte[] _buffer = new byte[BUFFER_SIZE];

        private List<MemberData> _memberData;
       
        public Schema()
        {
            _memberData = new List<MemberData>();
        }

        public void AddMemberData(MemberData memberData)
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
            int offset = 0;
            _buffer[offset] = 0;
            int offsetInLastByte = 0;

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

                memberData.BinarySerializer.Encode(_buffer, value, ref offset, ref offsetInLastByte);
            }

            return _buffer.Take(offset + 1).ToArray();
        }

        /// <summary>
        ///      Serialize only a difference between the new and the old object
        /// </summary>
        /// <param name="oldObject">Old object of generic type</param>
        /// <param name="newObject">New object of generic type</param>
        /// <returns>Array of bytes that represents a result of binary serialization</returns>
        public byte[] Serialize(T oldObject, T newObject)
        {
            int offset = 0;
            _buffer[offset] = 0;
            int offsetInLastByte = 0;

            foreach (MemberData memberData in _memberData)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                object? oldValue, newValue;

                if (memberData.MemberInfo is FieldInfo)
                {
                    FieldInfo fieldInfo = (FieldInfo)memberData.MemberInfo;
                    oldValue = fieldInfo.GetValue(oldObject);
                    newValue = fieldInfo.GetValue(newObject);
                }
                else
                {
                    PropertyInfo propertyInfo = (PropertyInfo)memberData.MemberInfo;
                    oldValue = propertyInfo.GetValue(oldObject);
                    newValue = propertyInfo.GetValue(newObject);
                }

                bool valuesEqual = object.Equals(oldValue, newValue);

                if (valuesEqual)
                {
                    memberData.BinarySerializer.EncodeMissingFlag(_buffer, ref offset, ref offsetInLastByte);
                }
                else
                {
                    memberData.BinarySerializer.Encode(_buffer, newValue, ref offset, ref offsetInLastByte);
                }
            }

            return _buffer.Take(offset + 1).ToArray();
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
       
            int offset = 0;
            int offsetInLastByte = 0;

            foreach (MemberData memberData in _memberData)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                HeaderWithValue value = memberData.BinarySerializer.Decode(buffer, ref offset, ref offsetInLastByte);

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

            return existing;
        }
    }
}
