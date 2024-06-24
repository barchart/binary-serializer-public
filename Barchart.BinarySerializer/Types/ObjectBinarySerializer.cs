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
    public class ObjectBinarySerializer<TContainer> : IBinaryTypeObjectSerializer<TContainer> where TContainer : new()
    {
        #region Properties
        public Schema<TContainer> Schema { get; }

        #endregion

        #region Constructor(s)

        public ObjectBinarySerializer(Schema<TContainer> schema)
        {
            Schema = schema;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public AttributeValue<TContainer> Decode(IDataBuffer dataBuffer)
        {
            AttributeHeader header = AttributeHeader.ReadFromBuffer(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new AttributeValue<TContainer>(header, default);
            }

            TContainer? deserializedObject = Schema.Deserialize(dataBuffer);

            return new AttributeValue<TContainer>(header, deserializedObject);
        }

        /// <inheritdoc />
        public AttributeValue<TContainer> Decode(IDataBuffer dataBuffer, TContainer existing)
        {
            AttributeHeader header = AttributeHeader.ReadFromBuffer(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new AttributeValue<TContainer>(header, default);
            }

            TContainer? deserializedObject = existing != null ? Schema.Deserialize(existing, dataBuffer) : default;

            return new AttributeValue<TContainer>(header, deserializedObject);
        }

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, TContainer? value)
        {
            AttributeHeader.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = value == null });

            if (value != null)
            {
                Schema.Serialize(value, dataBuffer);
            }
        }

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, TContainer? oldObject, TContainer? newObject)
        {
            AttributeHeader.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = newObject == null });

            if (newObject != null)
            {
                Schema.Serialize(oldObject!, newObject!, dataBuffer);
            }
        }

        /// <inheritdoc />
        public int GetLengthInBytes(TContainer value)
        {
            return Schema.GetLengthInBytes(value);
        }
    
        public int GetLengthInBits(TContainer? value)
        {
            return ((ISchema)Schema).GetLengthInBits(value);
        }

        public int GetLengthInBits(TContainer? oldValue, TContainer? newValue)
        {
            return ((ISchema)Schema).GetLengthInBits(oldValue, newValue);
        }

        #endregion
    }
}