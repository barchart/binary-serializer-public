namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Class that specifies attributes that needs to be set on the property or field of the class or structure
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BinarySerializeAttribute : Attribute
    {
        private readonly bool _include;
        private readonly bool _key;

        public bool Include { get { return _include; } }
        public bool Key { get { return _key; } }

        public BinarySerializeAttribute(bool include, bool key)
        {
            _include = include;
            _key = key;
        }
    }
}