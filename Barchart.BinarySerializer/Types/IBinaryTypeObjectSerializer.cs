﻿using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Types
{
    /// <summary>
    /// Provides additional methods for encoding and decoding values of type <typeparamref name="TMember"/> to and from a <see cref="DataBuffer"/>.
    /// </summary>
    /// <typeparam name="TMember">The type of the value to be serialized.</typeparam>
    public interface IBinaryTypeObjectSerializer<TMember> : IBinaryTypeSerializer<TMember>
    {
        public void Encode(DataBuffer dataBuffer, TMember oldValue, TMember newValue);
        public HeaderWithValue<TMember> Decode(DataBuffer dataBuffer, TMember existing);
    }
}