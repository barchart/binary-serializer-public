using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Represents the interface for collecting metadata about members of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMemberData<T>
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsIncluded { get; set; }
        public bool IsKeyAttribute { get; set; }
        public MemberInfo MemberInfo { get; set; }

        void Encode(T value, DataBuffer buffer);
        void EncodeCompare(T newValue, T oldValue, DataBuffer buffer);

        void Decode(T value, DataBuffer buffer);

        public int GetLengthInBits(T schemaObject);
        public int GetLengthInBits(T oldObject, T newObject);
    }
}

