# @barchart/binary-serializer-ts

## Structure

- **BinarySerializer**: All source code for the Binary Serializer library is in the [`src`](./src) folder. This includes the core serialization logic, attribute definitions, buffer management, schema factories, and any other utilities required for the serialization and deserialization processes.

- **Scripts**: The [`scripts`](./scripts) directory includes useful scripts for running tests, building the package, and publishing it to npm. You can use `test.ts` to run unit tests and `build.ts` to generate the final bundle.

- **Tests**: The [`tests`](./tests) directory contains a group of tests ensuring the library's reliability.

## Example Usage

Here are some simple examples of the library's usage:

### Serialize an Object

```typescript
import { Serializer } from '@barchart/binary-serializer-ts';

class TestClass {
    propertyName?: string;
    propertyNumber!: number;
}

const testObject = new TestClass();
testObject.propertyName = 'Name';
testObject.propertyNumber = 123;

// Creates a instance of Serializer class for the specified class
const serializer = new Serializer<TestClass>();

// Serialize the object into binary data
const serialized: Uint8Array = serializer.serialize(testObject);

// Deserialize the binary data back into an object
const deserialized = serializer.deserialize(serialized);

console.log(deserialized.propertyName); // Output: Name
console.log(deserialized.propertyNumber); // Output: 123
```

**Serialize Changes:**

```typescript
import {Serializer} from '@barchart/binary-serializer-ts';

class TestClass {
    propertyName: string;
    propertyNumber: number;
}

const previousObject = new TestClass();
previousObject.propertyName = 'Name';
previousObject.propertyNumber = 123;

const currentObject = new TestClass();
currentObject.propertyName = 'Name';
currentObject.propertyNumber = 321;

// Creates a instance of Serializer class for the specified class
const serializer = new Serializer<TestClass>();

// Serialize the changes into a binary format
const changes: Uint8Array = serializer.serializeWithPrevious(currentObject, previousObject);

// Deserialize the binary data back into the existing object
const deserializedObject = serializer.deserializeInto(changes, previousObject);

console.log(deserializedObject.propertyName); // Output: Name
console.log(deserializedObject.propertyNumber); // Output: 321
```
