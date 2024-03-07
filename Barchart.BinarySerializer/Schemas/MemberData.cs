﻿using Barchart.BinarySerializer.Types;
using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    public interface IMemberData<T>
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsIncluded { get; set; }
        public bool IsKeyAttribute { get; set; }
        public MemberInfo? MemberInfo { get; set; }

        void Encode(T value, DataBuffer buffer);
        void EncodeCompare(T newValue, T oldValue, DataBuffer buffer);

        void Decode(T value, DataBuffer buffer);

        public int GetLengthInBits(T schemaObject);
        public int GetLengthInBits(T oldObject, T newObject);
    }

    /// <summary>
    ///     Structure that represents information about a field/property fetched from reflection
    /// </summary>
    public class MemberData<T, V> : IMemberData<T> 
    {
        protected const int IS_MISSING_NUMBER_OF_BITS = 1;

        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsIncluded { get; set; }
        public bool IsKeyAttribute { get; set; }
        public MemberInfo MemberInfo { get; set; }
        public Func<T, V> GetDelegate { get; set; }
        public Action<T, V> SetDelegate { get; set; }
        public IBinaryTypeSerializer<V> BinarySerializer { get; set; }

        public MemberData(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public void Encode(T value, DataBuffer buffer) {
            V member = GetDelegate(value);
            BinarySerializer.Encode(buffer, member);
        }

        public virtual void EncodeCompare(T newObject, T oldObject, DataBuffer buffer) {
            V oldValue = GetDelegate(oldObject);
            V newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                 BinarySerializer.Encode(buffer, newValue);        
            }
            else
            {
                EncodeMissingFlag(buffer);
            }
        }

        public virtual void Decode(T existing, DataBuffer buffer) {

            HeaderWithValue<V> header;
            header = BinarySerializer.Decode(buffer);
           
            if (header.Header.IsMissing)
            {
                return;
            }

            SetDelegate(existing, header.Value);
        }

        public int GetLengthInBits(T schemaObject)
        {
            var value = GetDelegate(schemaObject);
            return BinarySerializer.GetLengthInBits(value);
        }

        public virtual int GetLengthInBits(T oldObject, T newObject)
        {
            var oldValue = GetDelegate(oldObject);
            var newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                return BinarySerializer.GetLengthInBits(newValue);              
            }
            else
            {
                return IS_MISSING_NUMBER_OF_BITS;
            }
        }

        protected void EncodeMissingFlag(DataBuffer dataBuffer)
        {
            dataBuffer.WriteBit(1);
        }
    }

    public class ObjectMemberData<T, V> : MemberData<T, V> where V : new()
    {
        public new ObjectBinarySerializer<V> BinarySerializer { get; set; }

        public ObjectMemberData(Type type, string name) : base(type, name) {}

        public override void EncodeCompare(T newObject, T oldObject, DataBuffer buffer)
        {
            V oldValue = GetDelegate(oldObject);
            V newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
                BinarySerializer.Encode(buffer, oldValue, newValue);               
            }
            else
            {
                EncodeMissingFlag(buffer);
            }
        }

        public override void Decode(T existing, DataBuffer buffer)
        {
            HeaderWithValue<V> header;

            V currentObject = GetDelegate(existing);
            header = BinarySerializer.Decode(buffer, currentObject);

            if (header.Header.IsMissing)
            {
                return;
            }

            SetDelegate(existing, header.Value);
        }

        public override int GetLengthInBits(T oldObject, T newObject)
        {
            var oldValue = GetDelegate(oldObject);
            var newValue = GetDelegate(newObject);

            bool valuesEqual = Equals(oldValue, newValue);

            if (!valuesEqual || IsKeyAttribute)
            {
              return BinarySerializer.GetLengthInBits(oldValue, newValue);
            }
            else
            {
                return IS_MISSING_NUMBER_OF_BITS;
            }
        }
    }
}