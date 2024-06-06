using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Represents the interface for collecting metadata about members of a class or structure with encoding/decoding functionality.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container object that holds the members.</typeparam>
    public interface IMemberData<TContainer>
    {
        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the member.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Indicates whether the member is marked with a key attribute.
        /// </summary>
        public bool IsKeyAttribute { get; }

        /// <summary>
        /// Gets the metadata information of the member.
        /// </summary>
        public MemberInfo MemberInfo { get; }

        /// <summary>
        /// Encodes the member value into the provided DataBuffer.
        /// </summary>
        /// <param name="value">The container object whose member value is to be encoded.</param>
        /// <param name="buffer">The DataBuffer to write the encoded data to.</param>
        void Encode(TContainer value, DataBuffer buffer);

        /// <summary>
        /// Encodes the differences between the new and old member values into the provided DataBuffer.
        /// </summary>
        /// <param name="newValue">The new container object whose member value is to be compared and encoded.</param>
        /// <param name="oldValue">The old container object whose member value is to be compared.</param>
        /// <param name="buffer">The DataBuffer to write the encoded data to.</param>
        void EncodeCompare(TContainer newValue, TContainer oldValue, DataBuffer buffer);

        /// <summary>
        /// Decodes the member value from the provided DataBuffer and sets it to the container object.
        /// </summary>
        /// <param name="value">The container object where the decoded member value will be set.</param>
        /// <param name="buffer">The DataBuffer to read the encoded data from.</param>
        void Decode(TContainer value, DataBuffer buffer);

        /// <summary>
        /// Compares the member values of two container objects.
        /// </summary>
        /// <param name="firstObject">The first container object to compare.</param>
        /// <param name="secondObject">The second container object to compare.</param>
        /// <returns>True if the member values are equal; otherwise, false.</returns>
        public bool CompareObjects(TContainer firstObject, TContainer secondObject);

        /// <summary>
        /// Compares the member values of two container objects and updates the first object with the values from the second object if they differ.
        /// </summary>
        /// <param name="firstObject">The first container object to update.</param>
        /// <param name="secondObject">The second container object to compare and copy values from.</param>
        public void CompareAndUpdateObject(TContainer firstObject, TContainer secondObject);

        /// <summary>
        /// Gets the length in bits of the encoded member value for the specified container object.
        /// </summary>
        /// <param name="schemaObject">The container object whose member value length is to be calculated.</param>
        /// <returns>The length in bits of the encoded member value.</returns>
        public int GetLengthInBits(TContainer schemaObject);

        /// <summary>
        /// Gets the length in bits of the encoded differences between the member values of two container objects.
        /// </summary>
        /// <param name="oldObject">The old container object to compare.</param>
        /// <param name="newObject">The new container object to compare and encode the differences.</param>
        /// <returns>The length in bits of the encoded differences.</returns>
        public int GetLengthInBits(TContainer oldObject, TContainer newObject);
    }
}