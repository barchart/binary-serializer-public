using Barchart.BinarySerializer.Types;
using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Represents metadata about an object member of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="T">The type of the class or structure.</typeparam>
    /// <typeparam name="V">The type of the member.</typeparam>
    public class ObjectMemberData<T, V> : MemberData<T, V> where V : new()
    {
        public ObjectMemberData(Type type, string name, bool isIncluded, bool isKeyAttribute, MemberInfo memberInfo,
            Func<T, V> getDelegate, Action<T, V>? setDelegate, IBinaryTypeObjectSerializer<V> binarySerializer)
            : base(type, name, isIncluded, isKeyAttribute, memberInfo, getDelegate, setDelegate, binarySerializer) { }

        public override void EncodeCompare(T newObject, T oldObject, DataBuffer buffer)
        {
            V oldValue = GetDelegate(oldObject);
            V newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                ((IBinaryTypeObjectSerializer<V>)BinarySerializer).Encode(buffer, oldValue, newValue);
            }
            else
            {
                EncodeMissingFlag(buffer);
            }
        }

        public override void Decode(T existing, DataBuffer buffer)
        {
            HeaderWithValue<V> headerWithValue;

            V currentObject = GetDelegate(existing);

            if (currentObject == null)
            {
                headerWithValue = ((IBinaryTypeObjectSerializer<V>)BinarySerializer).Decode(buffer);
            }
            else
            {
                headerWithValue = ((IBinaryTypeObjectSerializer<V>)BinarySerializer).Decode(buffer, currentObject);
            }

            if (headerWithValue.Header.IsMissing)
            {
                return;
            }

            if(headerWithValue.Value != null && SetDelegate != null) SetDelegate(existing, headerWithValue.Value);
        }

        public override int GetLengthInBits(T oldObject, T newObject)
        {
            var oldValue = GetDelegate(oldObject);
            var newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                return ((IBinaryTypeObjectSerializer<V>)BinarySerializer).GetLengthInBits(newValue);
            }
            else
            {
                return NumberOfBitsIsMissing;
            }
        }
    }
}

