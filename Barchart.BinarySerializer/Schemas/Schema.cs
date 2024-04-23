using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Represents a schema for serializing and deserializing objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects serialized and deserialized by this schema.</typeparam>
    public class Schema<T> : ISchema where T : new()
    {
        private const int BufferSize = 256000000;
        
        [ThreadStatic]
        private static byte[]? _buffer;

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

        private readonly IList<IMemberData<T>> _memberDataList;

        public Schema(List<IMemberData<T>> memberDataList)
        {
            _memberDataList = memberDataList;
        }

        /// <summary>
        /// Serialize an object of generic type.
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(T schemaObject)
        {
            return Serialize(schemaObject, Buffer);
        }

        /// <summary>
        /// Serialize an object of generic type.
        /// </summary>
        /// <param name="schemaObject">Object or structure to be serialized.</param>
        /// <param name="buffer">Buffer that will be populated with array of bytes representing result of the serialization.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(T schemaObject, byte[] buffer)
        {
            DataBuffer dataBuffer = new(buffer);
            dataBuffer.ResetByte();

            return Serialize(schemaObject, dataBuffer);
        }

        internal byte[] Serialize(T schemaObject, DataBuffer dataBuffer) {

            if (schemaObject == null)
            {
                throw new ArgumentNullException(nameof(schemaObject), "SchemaObject object cannot be null.");
            }

            foreach (IMemberData<T> memberData in _memberDataList)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                memberData.Encode(schemaObject, dataBuffer);
            }

            return dataBuffer.ToBytes();
        }

        /// <summary>
        /// Serialize only a difference between the new and the old object.
        /// </summary>
        /// <param name="oldObject">Old object of generic type.</param>
        /// <param name="newObject">New object of generic type.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(T oldObject, T newObject)
        {
            return Serialize(oldObject, newObject, Buffer);
        }

        /// <summary>
        /// Serialize only a difference between the new and the old object.
        /// </summary>
        /// <param name="oldObject">Old object of generic type.</param>
        /// <param name="newObject">New object of generic type.</param>
        /// <param name="buffer">Buffer that will be populated with array of bytes representing result of the serialization.</param>
        /// <returns> Array of bytes that represents a result of binary serialization. </returns>
        public byte[] Serialize(T oldObject, T newObject, byte[] buffer)
        {
            DataBuffer dataBuffer = new(buffer);
            dataBuffer.ResetByte();

            return Serialize(oldObject, newObject, dataBuffer);
        }

        internal byte[] Serialize(T oldObject, T newObject, DataBuffer dataBuffer)
        {
            if (oldObject == null)
            {
                return Serialize(newObject, dataBuffer);
            }

            foreach (IMemberData<T> memberData in _memberDataList)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }
                memberData.EncodeCompare(newObject, oldObject, dataBuffer);
            }

            return dataBuffer.ToBytes();
        }

        /// <summary>
        /// Deserialize array of bytes into object.
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized.</param>
        /// <returns> Deserialized object written into newly created object of generic type. </returns>
        public T Deserialize(byte[] buffer)
        {
            DataBuffer dataBuffer  = new(buffer);
            return Deserialize(dataBuffer);
        }

        internal T Deserialize(DataBuffer dataBuffer) {
            T existing = new();

            foreach (IMemberData<T> memberData in _memberDataList)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                memberData.Decode(existing, dataBuffer);     
            }

            return existing;
        }

        /// <summary>
        /// Deserialize array of bytes into object.
        /// </summary>
        /// <param name="buffer">Array oy bytes which will be deserialized.</param>
        /// <param name="existing">Existing generic object.</param>
        /// <returns> Deserialized object written into existing object of generic type. </returns>
        public T Deserialize(byte[] buffer, T existing)
        {
            DataBuffer dataBuffer = new(buffer); 
            return Deserialize(existing, dataBuffer);
        }

        internal T Deserialize(T existing, DataBuffer dataBuffer)
        {
            foreach (IMemberData<T> memberData in _memberDataList)
            {
                if (!memberData.IsIncluded)
                {
                    continue;
                }

                memberData.Decode(existing, dataBuffer);
            }

            return existing;
        }

        /// <summary>
        /// Calculates the total length of the binary representation of the provided schema object in bytes.
        /// </summary>
        /// <param name="schemaObject">The schema object to calculate the length for.</param>
        /// <returns> The total length of the binary representation of the schema object in bytes. </returns>
        public int GetLengthInBytes(T schemaObject)
        {
            return (int)Math.Ceiling((double)GetLengthInBits(schemaObject) / 8);
        }

        /// <summary>
        /// Calculates the total length of the binary representation of the difference between the provided schema objects in bytes.
        /// </summary>
        /// <param name="oldObject">The old schema object.</param>
        /// <param name="newObject">The new schema object.</param>
        /// <returns> The total length of the binary representation of the difference between the provided schema objects in bytes. </returns>
        public int GetLengthInBytes(T oldObject, T newObject)
        {
            return (int)Math.Ceiling((double)GetLengthInBits(oldObject, newObject) / 8);
        }

        /// <summary>
        /// Calculates the total length of the binary representation of the provided schema object in bits.
        /// </summary>
        /// <param name="schemaObject">The schema object to calculate the length for.</param>
        /// <returns> The total length of the binary representation of the schema object in bits. </returns>
        public int GetLengthInBits(T schemaObject)
        {
            int lengthInBits = 0;

            foreach (IMemberData<T> memberData in _memberDataList)
            {
                lengthInBits += memberData.GetLengthInBits(schemaObject);           
            }

            return lengthInBits;
        }

        /// <summary>
        /// Calculates the total length of the binary representation of the difference between the provided schema objects in bits.
        /// </summary>
        /// <param name="oldObject">The old schema object to calculate the length for.</param>
        /// <param name="newObject">The new schema object to calculate the length for.</param>
        /// <returns> The total length of the binary representation of the difference between the provided schema objects in bits. </returns>
        public int GetLengthInBits(T oldObject, T newObject)
        {
            int lengthInBits = 0;

            foreach (IMemberData<T> memberData in _memberDataList)
            {
                lengthInBits += memberData.GetLengthInBits(oldObject, newObject);
            }

            return lengthInBits;
        }

        /// <summary>
        /// Compares two objects of type T by iterating through the list of member data.
        /// </summary>
        /// <param name="firstObject">The first object to compare.</param>
        /// <param name="secondObject">The second object to compare.</param>
        /// <returns>True if all member data of the two objects are equal; otherwise, false.</returns>
        public bool CompareObjects(T firstObject, T secondObject)
        {
            foreach (IMemberData<T> memberData in _memberDataList)
            {
                if (memberData.CompareObjects(firstObject, secondObject) == false) return false;
            }

            return true;
        }

        /// <summary>
        /// Compares and updates the properties of an object of type T with corresponding properties of another object.
        /// </summary>
        /// <param name="objectToUpdate">The object to update.</param>
        /// <param name="newObject">The object containing the new values.</param>
        public void CompareAndUpdateObject(T objectToUpdate, T newObject)
        {
            foreach (IMemberData<T> memberData in _memberDataList)
            {
                memberData.CompareAndUpdateObject(objectToUpdate, newObject);
            }
        }

        #region ISchema implementation
        byte[] ISchema.Serialize(object schemaObject)
        {
            return Serialize((T)schemaObject);
        }

        byte[] ISchema.Serialize(object schemaObject, byte[] buffer)
        {
            return Serialize((T)schemaObject, buffer);
        }

        byte[] ISchema.Serialize(object oldObject, object newObject)
        {
            return Serialize((T)oldObject, (T)newObject);
        }

        byte[] ISchema.Serialize(object oldObject, object newObject, byte[] buffer)
        {
            return Serialize((T)oldObject, (T)newObject, buffer);
        }

        byte[] ISchema.Serialize(object schemaObject, DataBuffer dataBuffer)
        {
            return Serialize((T)schemaObject, dataBuffer);
        }

        byte[] ISchema.Serialize(object oldObject, object newObject, DataBuffer dataBuffer)
        {
            return Serialize((T)oldObject, (T)newObject, dataBuffer);
        }

        object? ISchema.Deserialize(byte[] buffer)
        {
            return Deserialize(buffer);
        }

        object? ISchema.Deserialize(byte[] buffer, object existing)
        {
            return Deserialize(buffer, (T)existing);
        }

        object? ISchema.Deserialize(DataBuffer dataBuffer)
        {
            return Deserialize(dataBuffer);
        }

        object? ISchema.Deserialize(object existing, DataBuffer dataBuffer)
        {
            return Deserialize((T)existing, dataBuffer);
        }

        int ISchema.GetLengthInBytes(object? schemaObject)
        {
            if (schemaObject == null)
            {
                return 0;
            }
            return GetLengthInBytes((T)schemaObject);
        }

        int ISchema.GetLengthInBytes(object? oldObject, object? newObject)
        {
            if (oldObject == null && newObject == null)
            {
                return 0;
            }

            if (oldObject != null && newObject == null)
            {
                return GetLengthInBits((T)oldObject);
            }

            if (oldObject == null && newObject != null)
            {
                return GetLengthInBytes((T)newObject);
            }

            return GetLengthInBytes((T)oldObject!, (T)newObject!);
        }

        int ISchema.GetLengthInBits(object? schemaObject)
        {
            if (schemaObject == null)
            {
                return 0;
            }
            return GetLengthInBits((T)schemaObject);
        }

        int ISchema.GetLengthInBits(object? oldObject, object? newObject)
        {
            if (oldObject == null && newObject == null)
            {
                return 0;
            }

            if(oldObject != null && newObject == null)
            {
                return GetLengthInBits((T)oldObject);
            }

            if (oldObject == null && newObject != null)
            {
                return GetLengthInBits((T)newObject);
            }

            return GetLengthInBits((T)oldObject!, (T)newObject!);
        }
        #endregion
    }
}