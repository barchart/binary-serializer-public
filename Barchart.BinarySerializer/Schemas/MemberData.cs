#region Using Statements

using System.Reflection;

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Represents metadata about a member of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="TContainer">The type of the class or structure.</typeparam>
    /// <typeparam name="T">The type of the member.</typeparam>
    public class MemberData<TContainer, T> : IMemberData<TContainer>
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
        public Func<TContainer, T> GetDelegate { get; }

        /// <inheritdoc />
        public Action<TContainer, T?>? SetDelegate { get; }
        
        /// <inheritdoc />
        public IBinaryTypeSerializer<T> BinarySerializer { get; }

        #endregion

        #region Constructor(s)

        public MemberData(Type type, string name, bool isKeyAttribute, MemberInfo memberInfo,
            Func<TContainer, T> getDelegate, Action<TContainer, T?>? setDelegate, IBinaryTypeSerializer<T> binarySerializer)
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
        public void Encode(TContainer value, IDataBufferWriter dataBuffer) {
            BinarySerializer.Encode(dataBuffer, GetDelegate(value));
        }

        /// <inheritdoc />
        public virtual void EncodeCompare(TContainer newObject, TContainer oldObject, IDataBufferWriter dataBuffer) {
            T oldValue = GetDelegate(oldObject);
            T newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                 BinarySerializer.Encode(dataBuffer, newValue);        
            }
            else
            {
                dataBuffer.WriteBit(true); // missing ...
            }
        }

        /// <inheritdoc />
        public void Decode(TContainer existing, IDataBufferReader dataBuffer)
        {
            bool missing = dataBuffer.ReadBit();

            if (missing)
            {
                return;
            }

            SetDelegate?.Invoke(existing, BinarySerializer.Decode(dataBuffer));
        }
        
        /// <inheritdoc />
        public bool CompareObjects(TContainer firstObject, TContainer secondObject)
        {
            T oldValue = GetDelegate(firstObject);
            T newValue = GetDelegate(secondObject);

            return Equals(oldValue, newValue);
        }

        /// <inheritdoc />
        public void CompareAndUpdateObject(TContainer firstObject, TContainer secondObject)
        {
            T oldValue = GetDelegate(firstObject);
            T newValue = GetDelegate(secondObject);

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
                return Header.NUMBER_OF_BITS_IS_MISSING;
            }
        }
        
        #endregion
    }
}