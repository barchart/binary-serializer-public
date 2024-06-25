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
    public class BinarySerializerObject<TContainer> : IBinaryTypeObjectSerializer<TContainer> where TContainer : new()
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
        public Attribute<TContainer> Decode(IDataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new Attribute<TContainer>(header, default);
            }

            TContainer? deserializedObject = Schema.Deserialize(dataBuffer);

            return new Attribute<TContainer>(header, deserializedObject);
        }

        /// <inheritdoc />
        public Attribute<TContainer> Decode(IDataBuffer dataBuffer, TContainer existing)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsMissing || header.IsNull)
            {
                return new Attribute<TContainer>(header, default);
            }

            TContainer? deserializedObject = existing != null ? Schema.Deserialize(existing, dataBuffer) : default;

            return new Attribute<TContainer>(header, deserializedObject);
        }

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, TContainer? value)
        {
            Header.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = value == null });

            if (value != null)
            {
                Schema.Serialize(value, dataBuffer);
            }
        }

        /// <inheritdoc />
        public void Encode(IDataBuffer dataBuffer, TContainer? oldObject, TContainer? newObject)
        {
            Header.WriteToBuffer(dataBuffer, new() { IsMissing = false, IsNull = newObject == null });

            if (newObject != null)
            {
                Schema.Serialize(oldObject!, newObject!, dataBuffer);
            }
        }

        public int GetLengthInBytes(TContainer value)
        {
            return Schema.GetLengthInBytes(value);
        }
    
        /// <inheritdoc />
        public int GetLengthInBits(TContainer value)
        {
            return ((ISchema)Schema).GetLengthInBits(value);
        }

        /// <inheritdoc />
        public int GetLengthInBits(TContainer oldValue, TContainer newValue)
        {
            return ((ISchema)Schema).GetLengthInBits(oldValue, newValue);
        }

        #endregion
    }
}