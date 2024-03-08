namespace Barchart.BinarySerializer.Schemas
{
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