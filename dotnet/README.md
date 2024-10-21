# @barchart/binary-serializer-net

## Structure

- **BinarySerializer**: All source code for the Binary Serializer library is in the [`Barchart.BinarySerializer`](./Barchart.BinarySerializer) folder. This includes the core serialization logic, attribute definitions, buffer management, schema factories, and any other utilities required for the serialization and deserialization processes.

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

// Creates a instance of Serializer class for the specified class
Serializer<TestClass> serializer = new();

// Serialize the object into binary data
byte[] serialized = serializer.Serialize(testObject);

// Deserialize the binary data back into an object
TestClass deserialized = serializer.Deserialize(serialized);

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

TestClass previousObject = new()
{
    PropertyName = "Name",
    PropertyNumber = 123
};

TestClass currentObject = new()
{
    PropertyName = "Name",
    PropertyNumber = 321
};

// Creates a instance of Serializer class for the specified class
Serializer<TestClass> serializer = new();

// Serialize the changes into a binary format
byte[] changes = serializer.Serialize(currentObject, previousObject);

// Deserialize the binary data back into the existing object
TestClass deserializedObject = serializer.Deserialize(changes, previousObject);

Console.WriteLine(deserializedObject.PropertyName); // Output: Name
Console.WriteLine(deserializedObject.PropertyNumber); // Output: 321
```

> [!NOTE]  
> The `Serialize` attribute is used to mark properties or fields for binary serialization. When `true` is passed to the `Serialize` attribute, it indicates that the data member is part of the unique key for the object. Multiple data members can be marked as key components, allowing for compound keys in complex objects.
