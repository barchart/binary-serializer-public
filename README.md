# @barchart/binary-serialization-net

The Binary Serializer is a .NET library for serializing and deserializing objects in binary format using efficient and fast protocol.

## Structure

- **Scripts**: The [`Barchart.BinarySerializer.Scripts`](./Barchart.BinarySerializer.Scripts) directory contains essential scripts that facilitate the library's operation and maintenance. This includes `test.csx` for running the tests and `publish.csx` for publishing the package.

- **Examples**: To help you get started and to demonstrate the capabilities of the Binary Serializer, example projects can be found in the [`Barchart.BinarySerializer.Examples`](./Barchart.BinarySerializer.Examples) folder. These examples cover a range of use cases, from basic serialization to more complex scenarios.

- **Tests**: The [`Barchart.BinarySerializer.Tests`](./Barchart.BinarySerializer.Tests) directory contains a group of tests ensuring the library's reliability.

## Example Usage

To present you the way `Binary Serializer` can be used, here are simple examples:

Serializing single entity:

```csharp
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Factories;

TestClass testObject = new()
{
    PropertyName = "Name",
    PropertyNumber = 123
};

// Instantiate a new SchemaFactory to create schemas for serialization
SchemaFactory schemaFactory = new();

// Generate a schema for the TestClass
ISchema<TestClass> schema = schemaFactory.Make<TestClass>();

// Create a DataBufferWriter with a predefined byte array size for serialization
DataBufferWriter writer = new(new byte[16]);

// Serialize the object to a binary format
byte[] bytes = schema.Serialize(writer, bryan);

// Create a DataBufferReader with the serialized bytes for deserialization
DataBufferReader reader = new(bytes);

// Deserialize the binary data back into an object
TestClass deserializedTestObject = schema.Deserialize(reader);

Console.WriteLine(deserializedTestObject.PropertyName); // Output: Name
Console.WriteLine(deserializedTestObject.PropertyNumber); // Output: 123
```

Serializing difference between two entities:

```csharp
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Factories;

TestClass testObjectPrevious = new()
{
    PropertyName = "Name",
    PropertyNumber = 123
};

TestClass testObjectCurrent = new()
{
    PropertyName = "Name",
    PropertyNumber = 321
};

// Instantiate a new SchemaFactory to create schemas for serialization
SchemaFactory schemaFactory = new();

// Generate a schema for the TestClass
ISchema<TestClass> schema = schemaFactory.Make<TestClass>();

// Create a DataBufferWriter with a predefined byte array size for serialization
DataBufferWriter writer = new(new byte[16]);

// Serialize the difference between two objects to a binary format
byte[] bytes = schema.Serialize(writer, testObjectCurrent, testObjectPrevious);

// Create a DataBufferReader with the serialized bytes for deserialization
DataBufferReader reader = new(bytes);

// Deserialize the binary data back into an object using the existing object
TestClass deserializedTestObject = schema.Deserialize(reader, testObjectPrevious);

Console.WriteLine(deserializedTestObject.PropertyName); // Output: Name
Console.WriteLine(deserializedTestObject.PropertyNumber); // Output: 321
```