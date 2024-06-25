#region Using Statements

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

        /// <inheritdoc /> 
        public byte[] Serialize(TContainer schemaObject)
        {
            return Serialize(schemaObject, Buffer);
        }

        /// <inheritdoc />
        public byte[] Serialize(TContainer schemaObject, byte[] buffer)
        {
            DataBuffer dataBuffer = new(buffer);

            return Serialize(schemaObject, dataBuffer);
        }

        internal byte[] Serialize(TContainer schemaObject, IDataBuffer dataBuffer) {

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

        /// <inheritdoc />
        public byte[] Serialize(TContainer oldObject, TContainer newObject)
        {
            return Serialize(oldObject, newObject, Buffer);
        }

        /// <inheritdoc />
        public byte[] Serialize(TContainer oldObject, TContainer newObject, byte[] buffer)
        {
            DataBuffer dataBuffer = new(buffer);

            return Serialize(oldObject, newObject, dataBuffer);
        }

        internal byte[] Serialize(TContainer oldObject, TContainer newObject, IDataBuffer dataBuffer)
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

        /// <inheritdoc />        
        public TContainer Deserialize(byte[] buffer)
        {
            DataBuffer dataBuffer  = new(buffer);
            return Deserialize(dataBuffer);
        }

        /// <inheritdoc />
        public TContainer Deserialize(byte[] buffer, TContainer existing)
        {
            DataBuffer dataBuffer = new(buffer); 
            return Deserialize(existing, dataBuffer);
        }

        internal TContainer Deserialize(IDataBuffer dataBuffer) {
            TContainer existing = new();

            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                memberData.Decode(existing, dataBuffer);
            }

            return existing;
        }

        internal TContainer Deserialize(TContainer existing, IDataBuffer dataBuffer)
        {
            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                memberData.Decode(existing, dataBuffer);
            }

            return existing;
        }

        /// <inheritdoc />
        public int GetLengthInBytes(TContainer schemaObject)
        {
            return (int)Math.Ceiling((double)GetLengthInBits(schemaObject) / 8);
        }

        /// <inheritdoc />
        public int GetLengthInBytes(TContainer oldObject, TContainer newObject)
        {
            return (int)Math.Ceiling((double)GetLengthInBits(oldObject, newObject) / 8);
        }

        /// <inheritdoc />
        public int GetLengthInBits(TContainer schemaObject)
        {
            int lengthInBits = 0;

            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                lengthInBits += memberData.GetLengthInBits(schemaObject);           
            }

            return lengthInBits;
        }

        /// <inheritdoc />
        public int GetLengthInBits(TContainer oldObject, TContainer newObject)
        {
            int lengthInBits = 0;

            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                lengthInBits += memberData.GetLengthInBits(oldObject, newObject);
            }

            return lengthInBits;
        }

        /// <summary>
        ///     ompares two objects of type T by iterating through the list of member data.
        /// </summary>
        /// <param name="firstObject">The first object to compare.</param>
        /// <param name="secondObject">The second object to compare.</param>
        /// <returns>True if all member data of the two objects are equal; otherwise, false.</returns>
        public bool CompareObjects(TContainer firstObject, TContainer secondObject)
        {
            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                if (memberData.CompareObjects(firstObject, secondObject) == false) return false;
            }

            return true;
        }

        /// <summary>
        ///     Compares and updates the properties of an object of type T with corresponding properties of another object.
        /// </summary>
        /// <param name="objectToUpdate">The object to update.</param>
        /// <param name="newObject">The object containing the new values.</param>
        public void CompareAndUpdateObject(TContainer? objectToUpdate, TContainer? newObject)
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

        byte[] ISchema.Serialize(object schemaObject)
        {
            return Serialize((TContainer)schemaObject);
        }

        byte[] ISchema.Serialize(object schemaObject, byte[] buffer)
        {
            return Serialize((TContainer)schemaObject, buffer);
        }

        byte[] ISchema.Serialize(object oldObject, object newObject)
        {
            return Serialize((TContainer)oldObject, (TContainer)newObject);
        }

        byte[] ISchema.Serialize(object oldObject, object newObject, byte[] buffer)
        {
            return Serialize((TContainer)oldObject, (TContainer)newObject, buffer);
        }

        object? ISchema.Deserialize(byte[] buffer)
        {
            return Deserialize(buffer);
        }

        object? ISchema.Deserialize(byte[] buffer, object existing)
        {
            return Deserialize(buffer, (TContainer)existing);
        }

        int ISchema.GetLengthInBytes(object? schemaObject)
        {
            if (schemaObject == null)
            {
                return 0;
            }
            return GetLengthInBytes((TContainer)schemaObject);
        }

        int ISchema.GetLengthInBytes(object? oldObject, object? newObject)
        {
            if (oldObject == null && newObject == null)
            {
                return 0;
            }

            if (oldObject != null && newObject == null)
            {
                return GetLengthInBits((TContainer)oldObject);
            }

            if (oldObject == null && newObject != null)
            {
                return GetLengthInBytes((TContainer)newObject);
            }

            return GetLengthInBytes((TContainer)oldObject!, (TContainer)newObject!);
        }

        int ISchema.GetLengthInBits(object? schemaObject)
        {
            if (schemaObject == null)
            {
                return 0;
            }
            return GetLengthInBits((TContainer)schemaObject);
        }

        int ISchema.GetLengthInBits(object? oldObject, object? newObject)
        {
            if (oldObject == null && newObject == null)
            {
                return 0;
            }

            if(oldObject != null && newObject == null)
            {
                return GetLengthInBits((TContainer)oldObject);
            }

            if (oldObject == null && newObject != null)
            {
                return GetLengthInBits((TContainer)newObject);
            }

            return GetLengthInBits((TContainer)oldObject!, (TContainer)newObject!);
        }

        #endregion
    }
}