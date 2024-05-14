namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    /// Class that specifies attributes that needs to be set on the property or field of the class or structure
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BinarySerializeAttribute : Attribute
    {
        private readonly bool _key;
        private readonly bool _include;

        public bool Key => _key;
        public bool Include => _include;

        public BinarySerializeAttribute(bool key = false, bool include = true)
        {
            _key = key;
            _include = include;
        }
    }
}