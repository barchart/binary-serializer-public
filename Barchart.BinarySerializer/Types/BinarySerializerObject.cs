/*
#region Using Statements

using Barchart.BinarySerializer.Attributes;
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    ///     Represents a binary serializer for objects of type <typeparamref name="TContainer"/> using a specified schema.
    /// </summary>
    /// <typeparam name="TContainer">The type of objects to be serialized.</typeparam>
    public class BinarySerializerObject<TContainer> : IBinaryTypeObjectSerializer<TContainer?> where TContainer : new()
    {
        #region Properties
        public Schema<TContainer> Schema { get; }

        #endregion

        #region Constructor(s)

        public BinarySerializerObject(Schema<TContainer> schema)
        {
            Schema = schema;
        }

        #endregion

        #region Methods

         /// <inheritdoc />
        public void Encode(IDataBufferWriter buffer, TContainer? value)
        {
            Header.WriteToBuffer(buffer, false, value == null);

            if (value != null)
            {
                Schema.Serialize(value, buffer);
            }
        }

        /// <inheritdoc />
        public void Encode(IDataBufferWriter buffer, TContainer? oldObject, TContainer? newObject)
        {
            Header.WriteToBuffer(buffer, false, newObject == null);

            if (newObject != null)
            {
                Schema.Serialize(oldObject!, newObject!, buffer);
            }
        }

        /// <inheritdoc />
        public Attribute<TContainer?> Decode(IDataBufferReader buffer)
        {
            Header.ReadFromBuffer(buffer, out bool valueIsMissing, out bool valueIsNull);
            TContainer? deserializedObject = default;
            
            if (!valueIsMissing && !valueIsNull)
            {
                deserializedObject = Schema.Deserialize(buffer);
            }

            return new Attribute<TContainer?>(valueIsMissing, deserializedObject);
        }

        /// <inheritdoc />
        public Attribute<TContainer?> Decode(IDataBufferReader buffer, TContainer? existing)
        {
            Header.ReadFromBuffer(buffer, out bool valueIsMissing, out bool valueIsNull);
            TContainer? deserializedObject= default;
            
            if (!valueIsMissing && !valueIsNull)
            {
                deserializedObject = existing != null ? Schema.Deserialize(existing, buffer) : default;
            }

            return new Attribute<TContainer?>(valueIsMissing, deserializedObject);
        }

        /// <inheritdoc cref="ISchema.GetLengthInBits(object)" path="/Schemas/ISchema"/>
        public int GetLengthInBytes(TContainer value)
        {
            return Schema.GetLengthInBytes(value);
        }
    
        /// <inheritdoc cref="ISchema.GetLengthInBits(object)" path="/Schemas/ISchema"/>
        public int GetLengthInBits(TContainer? value)
        {
            if (value == null)
            {
                return 0;
            }

            return Schema.GetLengthInBits(value);
        }

        /// <inheritdoc cref="ISchema.GetLengthInBits(object, object)" path="/Schemas/ISchema"/>
        public int GetLengthInBits(TContainer oldValue, TContainer newValue)
        {
            return Schema.GetLengthInBits(oldValue, newValue);
        }

        #endregion
    }
}
*/