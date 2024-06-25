#region Using Statements

using System.Reflection;

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Types;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Represents metadata about an object member of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="TContainer">The type of the class or structure.</typeparam>
    /// <typeparam name="T">The type of the member.</typeparam>
    public class ObjectMemberData<TContainer, T> : MemberData<TContainer, T> where T : new()
    {
        #region Constructor(s)

        public ObjectMemberData(Type type, string name, bool isKeyAttribute, MemberInfo memberInfo,
            Func<TContainer, T> getDelegate, Action<TContainer, T?>? setDelegate, IBinaryTypeObjectSerializer<T> binarySerializer)
            : base(type, name, isKeyAttribute, memberInfo, getDelegate, setDelegate, binarySerializer) { }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void EncodeCompare(TContainer newObject, TContainer oldObject, IDataBuffer buffer)
        {
            T oldValue = GetDelegate(oldObject);
            T newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                ((IBinaryTypeObjectSerializer<T>)BinarySerializer).Encode(buffer, oldValue, newValue);
            }
            else
            {
                buffer.WriteBit(true); // missing ...
            }
        }

        /// <inheritdoc />
        public override void Decode(TContainer existing, IDataBuffer buffer)
        {
            Attribute<T> attribute;

            T currentObject = GetDelegate(existing);

            if (currentObject == null)
            {
                attribute = ((IBinaryTypeObjectSerializer<T>)BinarySerializer).Decode(buffer);
            }
            else
            {
                attribute = ((IBinaryTypeObjectSerializer<T>)BinarySerializer).Decode(buffer, currentObject);
            }

            if (attribute.Header.IsMissing)
            {
                return;
            }

            if (attribute.Value != null) SetDelegate?.Invoke(existing, attribute.Value);
        }

        /// <inheritdoc />
        public override int GetLengthInBits(TContainer oldObject, TContainer newObject)
        {
            var oldValue = GetDelegate(oldObject);
            var newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                return ((IBinaryTypeObjectSerializer<T>)BinarySerializer).GetLengthInBits(newValue);
            }
            else
            {
                return DataBuffer.NumberOfBitsIsMissing;
            }
        }

        #endregion
    }
}