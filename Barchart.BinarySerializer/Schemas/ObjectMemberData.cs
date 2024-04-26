using Barchart.BinarySerializer.Types;
using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Represents metadata about an object member of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="TContainer">The type of the class or structure.</typeparam>
    /// <typeparam name="TMember">The type of the member.</typeparam>
    public class ObjectMemberData<TContainer, TMember> : MemberData<TContainer, TMember> where TMember : new()
    {
        public ObjectMemberData(Type type, string name, bool isKeyAttribute, MemberInfo memberInfo,
            Func<TContainer, TMember> getDelegate, Action<TContainer, TMember?>? setDelegate, IBinaryTypeObjectSerializer<TMember> binarySerializer)
            : base(type, name, isKeyAttribute, memberInfo, getDelegate, setDelegate, binarySerializer) { }

        public override void EncodeCompare(TContainer newObject, TContainer oldObject, DataBuffer buffer)
        {
            TMember oldValue = GetDelegate(oldObject);
            TMember newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                ((IBinaryTypeObjectSerializer<TMember>)BinarySerializer).Encode(buffer, oldValue, newValue);
            }
            else
            {
                EncodeMissingFlag(buffer);
            }
        }

        public override void Decode(TContainer existing, DataBuffer buffer)
        {
            HeaderWithValue<TMember> headerWithValue;

            TMember currentObject = GetDelegate(existing);

            if (currentObject == null)
            {
                headerWithValue = ((IBinaryTypeObjectSerializer<TMember>)BinarySerializer).Decode(buffer);
            }
            else
            {
                headerWithValue = ((IBinaryTypeObjectSerializer<TMember>)BinarySerializer).Decode(buffer, currentObject);
            }

            if (headerWithValue.Header.IsMissing)
            {
                return;
            }

            SetDelegate?.Invoke(existing, headerWithValue.Value);
        }

        public override int GetLengthInBits(TContainer oldObject, TContainer newObject)
        {
            var oldValue = GetDelegate(oldObject);
            var newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                return ((IBinaryTypeObjectSerializer<TMember>)BinarySerializer).GetLengthInBits(newValue);
            }
            else
            {
                return NumberOfBitsIsMissing;
            }
        }
    }
}

