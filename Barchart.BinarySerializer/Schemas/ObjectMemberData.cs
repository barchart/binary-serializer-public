#region Using Statements

using Barchart.BinarySerializer.Headers;
using Barchart.BinarySerializer.Types;
using System.Reflection;
using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Represents metadata about an object member of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="TContainer">The type of the class or structure.</typeparam>
    /// <typeparam name="TMember">The type of the member.</typeparam>
    public class ObjectMemberData<TContainer, TMember> : MemberData<TContainer, TMember> where TMember : new()
    {
        #region Constructor(s)

        public ObjectMemberData(Type type, string name, bool isKeyAttribute, MemberInfo memberInfo,
            Func<TContainer, TMember> getDelegate, Action<TContainer, TMember?>? setDelegate, IBinaryTypeObjectSerializer<TMember> binarySerializer)
            : base(type, name, isKeyAttribute, memberInfo, getDelegate, setDelegate, binarySerializer) { }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void EncodeCompare(TContainer newObject, TContainer oldObject, IDataBuffer buffer)
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
                buffer.WriteBit(true); // missing ...
            }
        }

        /// <inheritdoc />
        public override void Decode(TContainer existing, IDataBuffer buffer)
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

            if (headerWithValue.Value != null) SetDelegate?.Invoke(existing, headerWithValue.Value);
        }

        /// <inheritdoc />
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
                return DataBuffer.NumberOfBitsIsMissing;
            }
        }

        #endregion
    }
}