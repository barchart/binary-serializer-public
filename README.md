# @barchart/binary-serializer-public

[![AWS CodeBuild](https://codebuild.us-east-1.amazonaws.com/badges?uuid=eyJlbmNyeXB0ZWREYXRhIjoidWY3K0VOSGJIcVkxU29ERTRDUGs3SVZFZ21IeWVLaHEwdDBDMlZwckJyUGVaaSt1ajZzVk4wSkFFWTlFTlRvK25OZS9HSkxqMmdqNWw3YW0wVk5jYUdRPSIsIml2UGFyYW1ldGVyU3BlYyI6ImYrN2xMY1RTY0lDSllOYm4iLCJtYXRlcmlhbFNldFNlcmlhbCI6MX0%3D&branch=main)](https://github.com/barchart/binary-serializer-public)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

The Binary Serializer is a library designed for serializing objects (into a binary format) and deserializing objects (from the binary format).

## Key Features

- **Easy (De)serialization**: Convert objects into byte arrays and vice versa.
- **Customizable**: Adjust the serialization process to fit your needs.
- **Extensible**: Add new features to the library as needed.
- **Lightweight**: Easy to add to your existing projects.

## Example:

When serializing an object, the data is converted into a binary array. For example, consider the following class:

```csharp   

public class Person
{
    [Serialize]
    public string Name { get; set; } = "";
    
    [Serialize]
    public bool IsProgrammer { get; set; } = false;
    
    [Serialize]
    public ushort Age { get; set; } = 0;
}
``` 

Here is an example of what the byte representation might look like:

```aiignore
10000000 00000001 01000000 00010000 10011100 10011110 01011000 01011011 10000110 00100000 00001000
```
### Byte Representation:

- **10000000**: Represents a `Header` byte. The first bit (1) indicates whether the data is a `Snapshot`. If the bit is set to 1, it means the data is a snapshot; otherwise, it is not. The last four bits (0000) represent the `EntityId`, which helps identify the type of entity the data represents. The `EntityId` can range from 0 to 15 (4 bits).
- **00000001**: Represents the length of the `Name` property value.
- **01000000 00010000 10011100 10011110 01011000 01011011**: Represents the ASCII values of the characters in the `Name` property value.
- **10000110 00100000**: Represents a flag and the boolean value for the `IsProgrammer` property.
- **00001000**: Represents the integer value for the `Age` property.

### License

This software is available for use under the MIT license.
