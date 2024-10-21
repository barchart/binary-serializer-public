# @barchart/binary-serializer-ts

## Structure

- **BinarySerializer**: All source code for the Binary Serializer library is in the [`src`](./src) folder. This includes the core serialization logic, attribute definitions, buffer management, schema factories, and any other utilities required for the serialization and deserialization processes.

- **Tests**: The [`tests`](./tests) directory contains a group of tests ensuring the library's reliability.

## Example Usage

Here are some simple examples of the library's usage:

### Serialize an Object

```typescript
import { Serializer, SchemaField, DataType } from '@barchart/binary-serializer-ts';

class TestClass {
    propertyName: string;
    propertyNumber: number;
}

const entityId: number = 1;

const schemaFields: SchemaField[] = [
    { name: 'propertyName', type: DataType.string, isKey: true },
    { name: 'propertyNumber', type: DataType.int, isKey: false }
];

const testObject = new TestClass();
testObject.propertyName = 'Name';
testObject.propertyNumber = 123;

// Creates a instance of Serializer class for the specified class
const serializer = new Serializer<TestClass>(entityId, schemaFields);

// Serialize the object into binary data
const serialized: Uint8Array = serializer.serialize(testObject);

// Deserialize the binary data back into an object
const deserialized = serializer.deserialize(serialized);

console.log(deserialized.propertyName); // Output: Name
console.log(deserialized.propertyNumber); // Output: 123
```

**Serialize Changes:**

```typescript
import { Serializer, SchemaField, DataType } from '@barchart/binary-serializer-ts';

class TestClass {
    propertyName: string;
    propertyNumber: number;
}

const entityId: number = 1;

const schemaFields: SchemaField[] = [
    { name: 'propertyName', type: DataType.string, isKey: true },
    { name: 'propertyNumber', type: DataType.int, isKey: false }
];

const previousObject = new TestClass();
previousObject.propertyName = 'Name';
previousObject.propertyNumber = 123;

const currentObject = new TestClass();
currentObject.propertyName = 'Name';
currentObject.propertyNumber = 321;

// Creates a instance of Serializer class for the specified class
const serializer = new Serializer<TestClass>(entityId, schemaFields);

// Serialize the changes into a binary format
const changes: Uint8Array = serializer.serializeWithPrevious(currentObject, previousObject);

// Deserialize the binary data back into the existing object
const deserializedObject = serializer.deserializeInto(changes, previousObject);

console.log(deserializedObject.propertyName); // Output: Name
console.log(deserializedObject.propertyNumber); // Output: 321
```
