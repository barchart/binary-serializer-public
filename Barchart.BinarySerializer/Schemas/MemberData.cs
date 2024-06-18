#region Using Statements

using Barchart.BinarySerializer.DataSerialization.Headers;
using Barchart.BinarySerializer.DataSerialization.Types;
using System.Reflection;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Represents metadata about a member of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="TContainer">The type of the class or structure.</typeparam>
    /// <typeparam name="TMember">The type of the member.</typeparam>
    public class MemberData<TContainer, TMember> : IMemberData<TContainer>
    {
        #region Properties

        /// <inheritdoc />
        public Type Type { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public bool IsKeyAttribute { get; }

        /// <inheritdoc />
        public MemberInfo MemberInfo { get; }

        /// <inheritdoc />
        public Func<TContainer, TMember> GetDelegate { get; }

        /// <inheritdoc />
        public Action<TContainer, TMember?>? SetDelegate { get; }
        
        /// <inheritdoc />
        public IBinaryTypeSerializer<TMember> BinarySerializer { get; }

        #endregion

        #region Constructor(s)

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

        #endregion

        #region Methods
        
        /// <inheritdoc />
        public void Encode(TContainer value, DataBuffer dataBuffer) {
            TMember member = GetDelegate(value);
            BinarySerializer.Encode(dataBuffer, member);
        }

        /// <inheritdoc />
        public virtual void EncodeCompare(TContainer newObject, TContainer oldObject, DataBuffer dataBuffer) {
            TMember oldValue = GetDelegate(oldObject);
            TMember newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                 BinarySerializer.Encode(dataBuffer, newValue);        
            }
            else
            {
                dataBuffer.EncodeMissingFlag();
            }
        }

        /// <inheritdoc />
        public virtual void Decode(TContainer existing, DataBuffer dataBuffer) {

            HeaderWithValue<TMember> headerWithValue;
            headerWithValue = BinarySerializer.Decode(dataBuffer);
           
            if (headerWithValue.Header.IsMissing)
            {
                return;
            }

            if (headerWithValue.Value != null) SetDelegate?.Invoke(existing, headerWithValue.Value);
        }
        
        /// <inheritdoc />
        public bool CompareObjects(TContainer firstObject, TContainer secondObject)
        {
            TMember oldValue = GetDelegate(firstObject);
            TMember newValue = GetDelegate(secondObject);

            return Equals(oldValue, newValue);
        }

        /// <inheritdoc />
        public void CompareAndUpdateObject(TContainer firstObject, TContainer secondObject)
        {
            TMember oldValue = GetDelegate(firstObject);
            TMember newValue = GetDelegate(secondObject);

            if (newValue != null && !Equals(oldValue,newValue) && SetDelegate != null) SetDelegate(firstObject, newValue);
        }

        /// <inheritdoc />
        public int GetLengthInBits(TContainer schemaObject)
        {
            var value = GetDelegate(schemaObject);
            return BinarySerializer.GetLengthInBits(value);
        }

        /// <inheritdoc />
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
                return DataBuffer.NumberOfBitsIsMissing;
            }
        }
        
        #endregion
    }
}