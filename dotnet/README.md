# @barchart/binary-serializer-net

[![NuGet](https://img.shields.io/nuget/v/Barchart.BinarySerializer)](https://www.nuget.org/packages/binary-serializer-net)

## Structure

- **BinarySerializer**: All source code for the Binary Serializer library is in the [`Barchart.BinarySerializer`](./Barchart.BinarySerializer) directory. This includes the core serialization logic, attribute definitions, buffer management, schema factories, and any other utilities required for the serialization and deserialization processes.

- **Examples**: To help you get started and to demonstrate the capabilities of the Binary Serializer, example projects can be found in the [`Barchart.BinarySerializer.Examples`](./Barchart.BinarySerializer.Examples) directory. These examples cover a range of use cases, from basic serialization to more complex scenarios.

- **Scripts**: The [`Barchart.BinarySerializer.Scripts`](./Barchart.BinarySerializer.Scripts) directory contains essential scripts that facilitate the library's operation and maintenance. This includes `test.csx` for running the tests and `publish.csx` for publishing the package.

- **Tests**: The [`Barchart.BinarySerializer.Tests`](./Barchart.BinarySerializer.Tests) directory contains a group of tests ensuring the library's reliability.

## Example Usage

Here are some simple examples of the library's usage:

**Serialize a Snapshot:**

```csharp
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Factories;

public class TestEntity
{
    [Serialize(true)]
    public string? PropertyName { get; set; }
    
    [Serialize(false)]
    public int PropertyNumber { get; set; }
}

TestEntity entity = new()
{
    PropertyName = "Name",
    PropertyNumber = 123
};

const byte entityId = 1;

// Creates a instance of Serializer class for the specified class with provided entity id
Serializer<TestEntity> serializer = new(entityId);

// Serialize the entity into a byte array
byte[] bytes = serializer.Serialize(entity);

// Deserialize the byte array back into an object
TestEntity deserializedEntity = serializer.Deserialize(bytes);

Console.WriteLine(deserializedEntity.PropertyName); // Output: Name
Console.WriteLine(deserializedEntity.PropertyNumber); // Output: 123
```

**Serialize Changes:**

```csharp
using Barchart.BinarySerializer.Buffers;
using Barchart.BinarySerializer.Schemas.Factories;

public class TestEntity
{
    [Serialize(true)]
    public string? PropertyName { get; set; }
    
    [Serialize(false)]
    public int PropertyNumber { get; set; }
}

TestClass previousEntity = new()
{
    PropertyName = "Name",
    PropertyNumber = 123
};

TestClass currentEntity = new()
{
    PropertyName = "Name",
    PropertyNumber = 321
};

const byte entityId = 1;

// Creates an Serializer instance for the specified class with provided entity id
Serializer<TestEntity> serializer = new(entityId);

// Serialize the changes into a byte array
byte[] changes = serializer.Serialize(currentEntity, previousEntity);

// Deserialize the changes back into an object
TestEntity deserializedEntity = serializer.Deserialize(changes, previousEntity);

Console.WriteLine(deserializedEntity.PropertyName); // Output: Name
Console.WriteLine(deserializedEntity.PropertyNumber); // Output: 321
```

> [!NOTE]  
> The `Serialize` attribute is used to mark properties or fields for binary serialization. When `true` is passed to the `Serialize` attribute, it indicates that the data member is part of the unique key for the object. Multiple data members can be marked as key components, allowing for compound keys in complex objects.
