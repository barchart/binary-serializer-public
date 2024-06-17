﻿#region Using Statements

using Barchart.BinarySerializer.Schemas;
using Barchart.BinarySerializer.Utility;

#endregion

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Represents a binary serializer for objects of type <typeparamref name="TContainer"/> using a specified schema.
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
        public HeaderWithValue<TContainer> Decode(DataBuffer dataBuffer)
        {
            Header header = UtilityKit.ReadHeader(dataBuffer);

            if (header.IsValueMissingOrNull())
            {
                return new HeaderWithValue<TContainer>(header, default);
            }

            TContainer? deserializedObject = Schema.Deserialize(dataBuffer);

            return new HeaderWithValue<TContainer>(header, deserializedObject);
        }

        public HeaderWithValue<TContainer> Decode(DataBuffer dataBuffer, TContainer existing)
        {
            Header header = UtilityKit.ReadHeader(dataBuffer);

            if (header.IsValueMissingOrNull())
            {
                return new HeaderWithValue<TContainer>(header, default);
            }

            TContainer? deserializedObject = existing != null ? Schema.Deserialize(existing, dataBuffer) : default;

            return new HeaderWithValue<TContainer>(header, deserializedObject);
        }

        public void Encode(DataBuffer dataBuffer, TContainer? value)
        {
            Header header = new() { IsMissing = false, IsNull = value == null };
            
            header.WriteToBuffer(dataBuffer);

            if (value != null)
            {
                Schema.Serialize(value, dataBuffer);
            }
        }

        public void Encode(DataBuffer dataBuffer, TContainer? oldObject, TContainer? newObject)
        {
            Header header = new() { IsMissing = false, IsNull = newObject == null };
            
            header.WriteToBuffer(dataBuffer);

            if (newObject != null)
            {
                Schema.Serialize(oldObject!, newObject!, dataBuffer);
            }
        }

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