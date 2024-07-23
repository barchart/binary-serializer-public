# @barchart/binary-serialization-net

The Binary Serializer is a .NET library for serializing objects (into a binary format) and deserializing objects (from the binary format).

## Structure

- **BinarySerializer**: All source code for the Binary Serializer library is located in the [`Barchart.BinarySerializer`](./Barchart.BinarySerializer) folder. This includes the core serialization logic, attribute definitions, buffer management, schema factories, and any other utilities required for the serialization and deserialization processes.

- **Examples**: To help you get started and to demonstrate the capabilities of the Binary Serializer, example projects can be found in the [`Barchart.BinarySerializer.Examples`](./Barchart.BinarySerializer.Examples) folder. These examples cover a range of use cases, from basic serialization to more complex scenarios.

- **Scripts**: The [`Barchart.BinarySerializer.Scripts`](./Barchart.BinarySerializer.Scripts) directory contains essential scripts that facilitate the library's operation and maintenance. This includes `test.csx` for running the tests and `publish.csx` for publishing the package.

- **Tests**: The [`Barchart.BinarySerializer.Tests`](./Barchart.BinarySerializer.Tests) directory contains a group of tests ensuring the library's reliability.

## Example Usage

Here are some simple examples of the library's usage:

**Serialize a Snapshot:**

```csharp
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Factories;

public class TestClass
{
    [Serialize(true)]
    public string? PropertyName { get; set; }
    
    [Serialize(false)]
    public int PropertyNumber { get; set; }
}

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
byte[] serialized = schema.Serialize(writer, testObject);

// Create a DataBufferReader with the serialized bytes for deserialization
DataBufferReader reader = new(serialized);

// Deserialize the binary data back into an object
TestClass deserialized = schema.Deserialize(reader);

Console.WriteLine(deserialized.PropertyName); // Output: Name
Console.WriteLine(deserialized.PropertyNumber); // Output: 123
```

**Serialize Changes:**

```csharp
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Factories;

public class TestClass
{
    [Serialize(true)]
    public string? PropertyName { get; set; }
    
    [Serialize(false)]
    public int PropertyNumber { get; set; }
}

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
byte[] changes = schema.Serialize(writer, testObjectCurrent, testObjectPrevious);

// Create a DataBufferReader with the serialized bytes for deserialization
DataBufferReader reader = new(changes);

// Deserialize the binary data back into the existing object
schema.Deserialize(reader, testObjectPrevious);

Console.WriteLine(deserializedTestObject.PropertyName); // Output: Name
Console.WriteLine(deserializedTestObject.PropertyNumber); // Output: 321
```

> [!NOTE]  
> The `Serialize` attribute is used to mark properties or fields for binary serialization. When `true` is passed to the `Serialize` attribute, it indicates that the data member is part of the unique key for the object. Multiple data members can be marked as key components, allowing for compound keys in complex objects.