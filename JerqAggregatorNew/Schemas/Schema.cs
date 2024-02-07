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
        ///     serialize an object or structure of generic type
        /// </summary>
        /// <param name="schemaObject">object that needs to be serialized</param>
        /// <returns>array of bytes that represents a result of binary serializatiom</returns>
        public byte[] Serialize(T schemaObject)
        {
            int offset = 0;
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
        ///     deserialize an object or structure of generic type
        /// </summary>
        /// <param name="firstObject"></param>
        /// <param name="secondObject"></param>
        /// <returns></returns>
        public byte[] Serialize(T firstObject, T secondObject)
        {
            return new byte[1];
        }

        /// <summary>
        ///     deserialize array of bytes into object or structure
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns> deserialized object written into newly createdS object of generic type </returns>
        public T Deserialize(byte[] buffer) {
            return Deserialize(buffer, new T());
        }

        /// <summary>
        ///     deserialize array of bytes into object or structure
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="existing"></param>
        /// <returns> deserialized object written into existing object of generic type </returns>
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
