# @barchart/binary-serializer-ts

[![NPM](https://img.shields.io/npm/v/@barchart/binary-serializer-ts)](https://www.npmjs.com/package/@barchart/binary-serializer-ts)

## Structure

- **BinarySerializer**: All source code for the Binary Serializer library is in the [`src`](./src) directory. This includes the core serialization logic, attribute definitions, buffer management, schema factories, and any other utilities required for the serialization and deserialization processes.

- **Tests**: The [`tests`](./tests) directory contains a group of tests ensuring the library's reliability.

## Example Usage

Here are some simple examples of the library's usage:

### Serialize a Snapshot

```typescript
import {Serializer, SchemaField, DataType} from '@barchart/binary-serializer-ts';

class TestEntity {
    propertyName: string;
    propertyNumber: number;
}

const entityId: number = 1;

const schemaFields: SchemaField[] = [
    {name: 'propertyName', type: DataType.string, isKey: true},
    {name: 'propertyNumber', type: DataType.int, isKey: false}
];

const entity = new TestEntity();
entity.propertyName = 'Name';
entity.propertyNumber = 123;

// Creates a instance of Serializer class for the specified class with provided entity id and schema fields
const serializer = new Serializer<TestEntity>(entityId, schemaFields);

// Serialize the entity into a byte array
const bytes: Uint8Array = serializer.serialize(entity);

// Deserialize the byte array back into an object
const deserializedEntity = serializer.deserialize(bytes);

console.log(deserializedEntity.propertyName); // Output: Name
console.log(deserializedEntity.propertyNumber); // Output: 123
```

**Serialize Changes:**

```typescript
import {Serializer, SchemaField, DataType} from '@barchart/binary-serializer-ts';

class TestEntity {
    propertyName: string;
    propertyNumber: number;
}

const entityId: number = 1;

const schemaFields: SchemaField[] = [
    {name: 'propertyName', type: DataType.string, isKey: true},
    {name: 'propertyNumber', type: DataType.int, isKey: false}
];

const previousEntity = new TestEntity();
previousEntity.propertyName = 'Name';
previousEntity.propertyNumber = 123;

const currentEntity = new TestEntity();
currentEntity.propertyName = 'Name';
currentEntity.propertyNumber = 321;

// Creates a Serializer instance for the specified class with provided entity id and schema fields
const serializer = new Serializer<TestEntity>(entityId, schemaFields);

// Serialize the changes into a byte array
const changes: Uint8Array = serializer.serializeWithPrevious(currentEntity, previousEntity);

// Deserialize the changes back into an object
const deserializedEntity = serializer.deserializeInto(changes, previousEntity);

console.log(deserializedEntity.propertyName); // Output: Name
console.log(deserializedEntity.propertyNumber); // Output: 321
```
