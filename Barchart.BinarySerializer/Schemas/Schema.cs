﻿#region Using Statements

using Barchart.BinarySerializer.Buffers;

#endregion

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Represents a schema for serializing and deserializing objects of type <typeparamref name="TContainer"/>.
    /// </summary>
    /// <typeparam name="TContainer">The type of objects serialized and deserialized by this schema.</typeparam>
    public class Schema<TContainer> : ISchema where TContainer : new()
    {
        #region Fields

        private const int BufferSize = 1000000;
        
        [ThreadStatic]
        private static byte[]? _buffer;
        
        private readonly IList<IMemberData<TContainer>> _memberDataContainer;

        #endregion

        #region Properties

        private static byte[] Buffer
        {
            get
            {
                if (_buffer == null)
                {
                    return _buffer = new byte[BufferSize];
                }

                return _buffer;
            }
        }

        #endregion

        #region Constructor(s)

        public Schema(List<IMemberData<TContainer>> memberDataContainer)
        {
            _memberDataContainer = memberDataContainer;
        }

        #endregion

        #region Methods

        /// <inheritdoc cref="ISchema.Serialize(object)" />
        public byte[] Serialize(TContainer schemaObject)
        {
            return Serialize(schemaObject, Buffer);
        }

        /// <inheritdoc cref="ISchema.Serialize(object, byte[])" />
        public byte[] Serialize(TContainer schemaObject, byte[] buffer)
        {
            DataBufferWriter dataBuffer = new(buffer);

            return Serialize(schemaObject, dataBuffer);
        }

        public byte[] Serialize(TContainer schemaObject, IDataBufferWriter dataBuffer) {

            if (schemaObject == null)
            {
                throw new ArgumentNullException(nameof(schemaObject), "SchemaObject object cannot be null.");
            }

            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                memberData.Encode(schemaObject, dataBuffer);
            }

            return dataBuffer.ToBytes();
        }

        /// <inheritdoc cref="ISchema.Serialize(object, object)" />
        public byte[] Serialize(TContainer oldObject, TContainer newObject)
        {
            return Serialize(oldObject, newObject, Buffer);
        }

        /// <inheritdoc cref="ISchema.Serialize(object, object, byte[])" />
        public byte[] Serialize(TContainer oldObject, TContainer newObject, byte[] buffer)
        {
            DataBufferWriter dataBuffer = new(buffer);

            return Serialize(oldObject, newObject, dataBuffer);
        }

        /// <inheritdoc cref="ISchema.Serialize(object, object, IDataBufferWriter)" />
        public byte[] Serialize(TContainer oldObject, TContainer newObject, IDataBufferWriter dataBuffer)
        {
            if (oldObject == null)
            {
                return Serialize(newObject, dataBuffer);
            }
           
            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                memberData.EncodeCompare(newObject, oldObject, dataBuffer);
            }

            return dataBuffer.ToBytes();
        }

        /// <inheritdoc cref="ISchema.Deserialize(byte[])" />        
        public TContainer Deserialize(byte[] buffer)
        {
            DataBufferReader dataBuffer  = new(buffer);
            return Deserialize(dataBuffer);
        }

        /// <inheritdoc cref="ISchema.Deserialize(byte[], object)" />
        public TContainer Deserialize(byte[] buffer, TContainer existing)
        {
            DataBufferReader dataBuffer = new(buffer); 
            return Deserialize(existing, dataBuffer);
        }

        /// <inheritdoc cref="ISchema.Deserialize(IDataBufferReader)" />
        public TContainer Deserialize(IDataBufferReader dataBuffer) {
            TContainer existing = new();

            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                memberData.Decode(existing, dataBuffer);
            }

            return existing;
        }

        /// <inheritdoc cref="ISchema.Deserialize(object, IDataBufferReader)" />
        public TContainer Deserialize(TContainer existing, IDataBufferReader dataBuffer)
        {
            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                memberData.Decode(existing, dataBuffer);
            }

            return existing;
        }

        /// <inheritdoc cref="ISchema.GetLengthInBytes(object)" />
        public int GetLengthInBytes(TContainer schemaObject)
        {
            if (schemaObject == null)
            {
                return 0;
            }

            return (int)Math.Ceiling((double)GetLengthInBits(schemaObject) / 8);
        }

        /// <inheritdoc cref="ISchema.GetLengthInBytes(object, object)" />
        public int GetLengthInBytes(TContainer oldObject, TContainer newObject)
        {
            if (oldObject == null && newObject == null)
            {
                return 0;
            }

            if (oldObject != null && newObject == null)
            {
                return GetLengthInBits(oldObject);
            }

            if (oldObject == null && newObject != null)
            {
                return GetLengthInBytes(newObject);
            }

            return (int)Math.Ceiling((double)GetLengthInBits(oldObject!, newObject!) / 8);
        }

        /// <inheritdoc cref="ISchema.GetLengthInBits(object)" />
        public int GetLengthInBits(TContainer schemaObject)
        {
            if (schemaObject == null)
            {
                return 0;
            }

            int lengthInBits = 0;

            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                lengthInBits += memberData.GetLengthInBits(schemaObject);           
            }

            return lengthInBits;
        }

        /// <inheritdoc cref="ISchema.GetLengthInBits(object, object)" />
        public int GetLengthInBits(TContainer oldObject, TContainer newObject)
        {
            if (oldObject == null && newObject == null)
            {
                return 0;
            }

            if (oldObject != null && newObject == null)
            {
                return GetLengthInBits(oldObject);
            }

            if (oldObject == null && newObject != null)
            {
                return GetLengthInBits(newObject);
            }

            int lengthInBits = 0;

            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                lengthInBits += memberData.GetLengthInBits(oldObject!, newObject!);
            }

            return lengthInBits;
        }

        /// <inheritdoc cref="ISchema.CompareObjects(object, object)" />
        public bool CompareObjects(TContainer firstObject, TContainer secondObject)
        {
            if (ReferenceEquals(firstObject, secondObject))
            {
                return true;
            }

            if (firstObject == null || secondObject == null)
            {
                return false;
            }
            
            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                if (memberData.CompareObjects(firstObject, secondObject) == false) return false;
            }

            return true;
        }

        /// <inheritdoc cref="ISchema.CompareAndUpdateObject(object, object)" />
        public void CompareAndUpdateObject(TContainer objectToUpdate, TContainer newObject)
        {
            if (objectToUpdate == null || newObject == null)
            {
                return;
            }

            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                memberData.CompareAndUpdateObject(objectToUpdate, newObject);
            }
        }

        #region ISchema implementation

        /// <inheritdoc />
        byte[] ISchema.Serialize(object schemaObject)
        {
            return Serialize((TContainer)schemaObject);
        }

        /// <inheritdoc />
        byte[] ISchema.Serialize(object schemaObject, IDataBufferWriter dataBuffer)
        {
            return Serialize((TContainer)schemaObject, dataBuffer);
        }

        /// <inheritdoc />
        byte[] ISchema.Serialize(object schemaObject, byte[] buffer)
        {
            return Serialize((TContainer)schemaObject, buffer);
        }

        /// <inheritdoc />
        byte[] ISchema.Serialize(object oldObject, object newObject, IDataBufferWriter dataBuffer)
        {
            return Serialize((TContainer)oldObject, (TContainer)newObject, dataBuffer);
        }

        /// <inheritdoc />
        byte[] ISchema.Serialize(object oldObject, object newObject)
        {
            return Serialize((TContainer)oldObject, (TContainer)newObject);
        }

        /// <inheritdoc />
        byte[] ISchema.Serialize(object oldObject, object newObject, byte[] buffer)
        {
            return Serialize((TContainer)oldObject, (TContainer)newObject, buffer);
        }

        /// <inheritdoc />
        object? ISchema.Deserialize(byte[] buffer)
        {
            return Deserialize(buffer);
        }

        /// <inheritdoc />
        object? ISchema.Deserialize(IDataBufferReader dataBuffer)
        {
            return Deserialize(dataBuffer);
        }

        /// <inheritdoc />
        object? ISchema.Deserialize(byte[] buffer, object existing)
        {
            return Deserialize(buffer, (TContainer)existing);
        }

        /// <inheritdoc />
        object? ISchema.Deserialize(object existing, IDataBufferReader dataBuffer)
        {
            return Deserialize((TContainer)existing, dataBuffer);
        }

        /// <inheritdoc />
        int ISchema.GetLengthInBytes(object schemaObject)
        {
            return GetLengthInBytes((TContainer)schemaObject);
        }

        /// <inheritdoc />
        int ISchema.GetLengthInBytes(object oldObject, object newObject)
        {
            return GetLengthInBytes((TContainer)oldObject, (TContainer)newObject);
        }

        /// <inheritdoc />
        int ISchema.GetLengthInBits(object schemaObject)
        {
            return GetLengthInBits((TContainer)schemaObject);
        }

        /// <inheritdoc />
        int ISchema.GetLengthInBits(object oldObject, object newObject)
        {
            return GetLengthInBits((TContainer)oldObject, (TContainer)newObject);
        }

        /// <inheritdoc />
        bool ISchema.CompareObjects(object firstObject, object secondObject)
        {
            return CompareObjects((TContainer)firstObject, (TContainer)secondObject);
        }

        /// <inheritdoc />
        void ISchema.CompareAndUpdateObject(object objectToUpdate, object newObject)
        {
            CompareAndUpdateObject((TContainer)objectToUpdate, (TContainer)newObject);
        }

        #endregion

        #endregion
    }
}