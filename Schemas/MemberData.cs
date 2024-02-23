using JerqAggregatorNew.Types;
using System.Reflection;

namespace JerqAggregatorNew.Schemas
{
    /// <summary>
    ///     Structure that represents information about a field/property fetched from reflection
    /// </summary>
    public class MemberData<T>
    {
        public Type Type { get; set; }
        public MemberInfo MemberInfo { get; set; }
        public string Name { get; set; }
        public bool IsIncluded { get; set; }
        public bool IsKeyAttribute { get; set; }
        public ISerializer BinarySerializer { get; set; }
        public Func<T, object?> GetDelegate { get; set; }
        public Action<T, object?> SetDelegate { get; set; }
    }
}