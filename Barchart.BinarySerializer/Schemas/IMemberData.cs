using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Represents the interface for collecting metadata about members of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="TContainer"></typeparam>
    public interface IMemberData<TContainer>
    {
        public string Name { get; }
        public Type Type { get; }
        public bool IsKeyAttribute { get; }
        public MemberInfo MemberInfo { get; }

        void Encode(TContainer value, DataBuffer buffer);
        void EncodeCompare(TContainer newValue, TContainer oldValue, DataBuffer buffer);
        void Decode(TContainer value, DataBuffer buffer);

        public bool CompareObjects(TContainer firstObject, TContainer secondObject);
        public void CompareAndUpdateObject(TContainer firstObject, TContainer secondObject);

        public int GetLengthInBits(TContainer schemaObject);
        public int GetLengthInBits(TContainer oldObject, TContainer newObject);
    }
}