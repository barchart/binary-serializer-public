﻿using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Represents the interface for collecting metadata about members of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMemberData<T>
    {
        public string Name { get; }
        public Type Type { get; }
        public bool IsIncluded { get; }
        public bool IsKeyAttribute { get; }
        public MemberInfo MemberInfo { get; }

        void Encode(T value, DataBuffer buffer);
        void EncodeCompare(T newValue, T oldValue, DataBuffer buffer);
        void Decode(T value, DataBuffer buffer);

        public bool CompareObjects(T firstObject, T secondObject);
        public void CompareAndUpdateObject(T firstObject, T secondObject);

        public int GetLengthInBits(T schemaObject);
        public int GetLengthInBits(T oldObject, T newObject);
    }
}

