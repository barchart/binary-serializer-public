﻿using Barchart.BinarySerializer.Types;
using System.Reflection;

namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Structure that represents information about a field/property fetched from reflection
    /// </summary>
    internal struct MemberData<T>
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsIncluded { get; set; }
        public bool IsKeyAttribute { get; set; }
        public MemberInfo MemberInfo { get; set; }
        internal ISerializer BinarySerializer { get; set; }
        public Func<T, object?> GetDelegate { get; set; }
        public Action<T, object?> SetDelegate { get; set; }
    }
}