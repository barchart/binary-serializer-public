using Barchart.BinarySerializer.Types;
using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Represents metadata about a member of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="T">The type of the class or structure.</typeparam>
    /// <typeparam name="V">The type of the member.</typeparam>
    public class MemberData<T, V> : IMemberData<T>
    {
        protected const int NumberOfBitsIsMissing = 1;

        public Type Type { get; set; }
        public string Name { get; set; }
        public bool IsIncluded { get; set; }
        public bool IsKeyAttribute { get; set; }
        public MemberInfo MemberInfo { get; set; }
        public Func<T, V> GetDelegate { get; set; }
        public Action<T, V>? SetDelegate { get; set; }
        public IBinaryTypeSerializer<V> BinarySerializer { get; set; }

        public MemberData(Type type, string name, bool isIncluded, bool isKeyAttribute, MemberInfo memberInfo,
            Func<T, V> getDelegate, Action<T, V>? setDelegate, IBinaryTypeSerializer<V> binarySerializer)
        {
            Type = type;
            Name = name;
            IsIncluded = isIncluded;
            IsKeyAttribute = isKeyAttribute;
            MemberInfo = memberInfo;
            GetDelegate = getDelegate;
            SetDelegate = setDelegate;
            BinarySerializer = binarySerializer;
        }

        public void Encode(T value, DataBuffer buffer) {
            V member = GetDelegate(value);
            BinarySerializer.Encode(buffer, member);
        }

        public virtual void EncodeCompare(T newObject, T oldObject, DataBuffer buffer) {
            V oldValue = GetDelegate(oldObject);
            V newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                 BinarySerializer.Encode(buffer, newValue);        
            }
            else
            {
                EncodeMissingFlag(buffer);
            }
        }

        public virtual void Decode(T existing, DataBuffer buffer) {

            HeaderWithValue<V> headerWithValue;
            headerWithValue = BinarySerializer.Decode(buffer);
           
            if (headerWithValue.Header.IsMissing)
            {
                return;
            }

            if(headerWithValue.Value != null && SetDelegate != null) SetDelegate(existing, headerWithValue.Value);
        }

        public int GetLengthInBits(T schemaObject)
        {
            var value = GetDelegate(schemaObject);
            return BinarySerializer.GetLengthInBits(value);
        }

        public virtual int GetLengthInBits(T oldObject, T newObject)
        {
            var oldValue = GetDelegate(oldObject);
            var newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                return BinarySerializer.GetLengthInBits(newValue);              
            }
            else
            {
                return NumberOfBitsIsMissing;
            }
        }

        protected void EncodeMissingFlag(DataBuffer dataBuffer)
        {
            dataBuffer.WriteBit(1);
        }
    }
}