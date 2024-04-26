using Barchart.BinarySerializer.Types;
using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Represents metadata about a member of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="TContainer">The type of the class or structure.</typeparam>
    /// <typeparam name="TMember">The type of the member.</typeparam>
    public class MemberData<TContainer, TMember> : IMemberData<TContainer>
    {
        protected const int NumberOfBitsIsMissing = 1;

        public Type Type { get; }
        public string Name { get; }
        public bool IsKeyAttribute { get; }
        public MemberInfo MemberInfo { get; }

        public Func<TContainer, TMember> GetDelegate { get; }
        public Action<TContainer, TMember?>? SetDelegate { get; }
        public IBinaryTypeSerializer<TMember> BinarySerializer { get; }

        public MemberData(Type type, string name, bool isKeyAttribute, MemberInfo memberInfo,
            Func<TContainer, TMember> getDelegate, Action<TContainer, TMember?>? setDelegate, IBinaryTypeSerializer<TMember> binarySerializer)
        {
            Type = type;
            Name = name;
            IsKeyAttribute = isKeyAttribute;
            MemberInfo = memberInfo;
            GetDelegate = getDelegate;
            SetDelegate = setDelegate;
            BinarySerializer = binarySerializer;
        }

        public void Encode(TContainer value, DataBuffer buffer) {
            TMember member = GetDelegate(value);
            BinarySerializer.Encode(buffer, member);
        }

        public virtual void EncodeCompare(TContainer newObject, TContainer oldObject, DataBuffer buffer) {
            TMember? oldValue = oldObject == null ? default : GetDelegate(oldObject);
            TMember? newValue = newObject == null ? default : GetDelegate(newObject);

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

        public virtual void Decode(TContainer existing, DataBuffer buffer) {

            HeaderWithValue<TMember> headerWithValue;
            headerWithValue = BinarySerializer.Decode(buffer);
           
            if (headerWithValue.Header.IsMissing)
            {
                return;
            }

            SetDelegate?.Invoke(existing, headerWithValue.Value);
        }

        public bool CompareObjects(TContainer firstObject, TContainer secondObject)
        {
            TMember oldValue = GetDelegate(firstObject);
            TMember newValue = GetDelegate(secondObject);

            return Equals(oldValue, newValue);
        }

        public void CompareAndUpdateObject(TContainer firstObject, TContainer secondObject)
        {
            TMember oldValue = GetDelegate(firstObject);
            TMember newValue = GetDelegate(secondObject);

            if(newValue != null && !Equals(oldValue,newValue) && SetDelegate != null) SetDelegate(firstObject, newValue);
        }

        public int GetLengthInBits(TContainer schemaObject)
        {
            var value = GetDelegate(schemaObject);
            return BinarySerializer.GetLengthInBits(value);
        }

        public virtual int GetLengthInBits(TContainer oldObject, TContainer newObject)
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