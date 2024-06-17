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
        
        /// <summary>
        ///     Serialize an object of generic type.
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(TContainer schemaObject)
        {
            return Serialize(schemaObject, Buffer);
        }

        /// <summary>
        ///     Serialize an object of generic type.
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized.</param>
        /// <param name="buffer">Buffer that will be populated with array of bytes representing result of the serialization.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(TContainer schemaObject, byte[] buffer)
        {
            DataBuffer dataBuffer = new(buffer);
            dataBuffer.ResetByte();

            return Serialize(schemaObject, dataBuffer);
        }

        internal byte[] Serialize(TContainer schemaObject, DataBuffer dataBuffer) {

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

        /// <summary>
        ///     Serialize only a difference between the new and the old object.
        /// </summary>
        /// <param name="oldObject">Old object of generic type.</param>
        /// <param name="newObject">New object of generic type.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(TContainer oldObject, TContainer newObject)
        {
            return Serialize(oldObject, newObject, Buffer);
        }

        /// <summary>
        ///     Serialize only a difference between the new and the old object.
        /// </summary>
        /// <param name="oldObject">Old object of generic type.</param>
        /// <param name="newObject">New object of generic type.</param>
        /// <param name="buffer">Buffer that will be populated with array of bytes representing result of the serialization.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(TContainer oldObject, TContainer newObject, byte[] buffer)
        {
            DataBuffer dataBuffer = new(buffer);
            dataBuffer.ResetByte();

            return Serialize(oldObject, newObject, dataBuffer);
        }

        internal byte[] Serialize(TContainer oldObject, TContainer newObject, DataBuffer dataBuffer)
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

        /// <summary>
        ///     Deserialize array of bytes into object.
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized.</param>
        /// <returns> Deserialized object written into newly created object of generic type. </returns>
        public TContainer Deserialize(byte[] buffer)
        {
            DataBuffer dataBuffer  = new(buffer);
            return Deserialize(dataBuffer);
        }

        internal TContainer Deserialize(DataBuffer dataBuffer) {
            TContainer existing = new();

            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                memberData.Decode(existing, dataBuffer);
            }

            return existing;
        }

        /// <summary>
        ///     eserialize array of bytes into object.
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized.</param>
        /// <param name="existing">Existing generic object.</param>
        /// <returns> Deserialized object written into existing object of generic type. </returns>
        public TContainer Deserialize(byte[] buffer, TContainer existing)
        {
            DataBuffer dataBuffer = new(buffer); 
            return Deserialize(existing, dataBuffer);
        }

        internal TContainer Deserialize(TContainer existing, DataBuffer dataBuffer)
        {
            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                memberData.Decode(existing, dataBuffer);
            }

            return existing;
        }

        /// <summary>
        ///     Calculates the total length of the binary representation of the provided schema object in bytes.
        /// </summary>
        /// <param name="schemaObject">The schema object to calculate the length for.</param>
        /// <returns> The total length of the binary representation of the schema object in bytes. </returns>
        public int GetLengthInBytes(TContainer schemaObject)
        {
            return (int)Math.Ceiling((double)GetLengthInBits(schemaObject) / 8);
        }

        /// <summary>
        ///     Calculates the total length of the binary representation of the difference between the provided schema objects in bytes.
        /// </summary>
        /// <param name="oldObject">The old schema object.</param>
        /// <param name="newObject">The new schema object.</param>
        /// <returns> The total length of the binary representation of the difference between the provided schema objects in bytes. </returns>
        public int GetLengthInBytes(TContainer oldObject, TContainer newObject)
        {
            return (int)Math.Ceiling((double)GetLengthInBits(oldObject, newObject) / 8);
        }

        /// <summary>
        ///     Calculates the total length of the binary representation of the provided schema object in bits.
        /// </summary>
        /// <param name="schemaObject">The schema object to calculate the length for.</param>
        /// <returns> The total length of the binary representation of the schema object in bits. </returns>
        public int GetLengthInBits(TContainer schemaObject)
        {
            int lengthInBits = 0;

            foreach (IMemberData<TContainer> memberData in _memberDataContainer)
            {
                lengthInBits += memberData.GetLengthInBits(schemaObject);           
            }

            return lengthInBits;
        }

        /// <summary>
        ///     Calculates the total length of the binary representation of the difference between the provided schema objects in bits.
        /// </summary>
        /// <param name="oldObject">The old schema object to calculate the length for.</param>
        /// <param name="newObject">The new schema object to calculate the length for.</param>
        /// <returns> The total length of the binary representation of the difference between the provided schema objects in bits. </returns>
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

        byte[] ISchema.Serialize(object schemaObject, DataBuffer dataBuffer)
        {
            return Serialize((TContainer)schemaObject, dataBuffer);
        }

        byte[] ISchema.Serialize(object oldObject, object newObject, DataBuffer dataBuffer)
        {
            return Serialize((TContainer)oldObject, (TContainer)newObject, dataBuffer);
        }

        object? ISchema.Deserialize(byte[] buffer)
        {
            return Deserialize(buffer);
        }

        object? ISchema.Deserialize(byte[] buffer, object existing)
        {
            return Deserialize(buffer, (TContainer)existing);
        }

        object? ISchema.Deserialize(DataBuffer dataBuffer)
        {
            return Deserialize(dataBuffer);
        }

        object? ISchema.Deserialize(object existing, DataBuffer dataBuffer)
        {
            return Deserialize((TContainer)existing, dataBuffer);
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