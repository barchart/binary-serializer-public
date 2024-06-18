#region Using Statements

using Barchart.BinarySerializer.DataSerialization.Headers;
using Barchart.BinarySerializer.Schemas;

#endregion

namespace Barchart.BinarySerializer.DataSerialization.Types
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
        public HeaderWithValue<TContainer> Decode(DataBuffer dataBuffer)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsValueMissingOrNull())
            {
                return new HeaderWithValue<TContainer>(header, default);
            }

            TContainer? deserializedObject = Schema.Deserialize(dataBuffer);

            return new HeaderWithValue<TContainer>(header, deserializedObject);
        }

        /// <inheritdoc />
        public HeaderWithValue<TContainer> Decode(DataBuffer dataBuffer, TContainer existing)
        {
            Header header = Header.ReadFromBuffer(dataBuffer);

            if (header.IsValueMissingOrNull())
            {
                return new HeaderWithValue<TContainer>(header, default);
            }

            TContainer? deserializedObject = existing != null ? Schema.Deserialize(existing, dataBuffer) : default;

            return new HeaderWithValue<TContainer>(header, deserializedObject);
        }

        /// <inheritdoc />
        public void Encode(DataBuffer dataBuffer, TContainer? value)
        {
            Header header = new() { IsMissing = false, IsNull = value == null };
            
            header.WriteToBuffer(dataBuffer);

            if (value != null)
            {
                Schema.Serialize(value, dataBuffer);
            }
        }

        /// <inheritdoc />
        public void Encode(DataBuffer dataBuffer, TContainer? oldObject, TContainer? newObject)
        {
            Header header = new() { IsMissing = false, IsNull = newObject == null };
            
            header.WriteToBuffer(dataBuffer);

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