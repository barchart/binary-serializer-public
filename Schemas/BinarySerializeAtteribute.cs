namespace Barchart.BinarySerializer.Schemas
{
    public class BinarySerializeAttribute : Attribute
    {
        public bool Include { get; }
        public bool Key { get; }

        public BinarySerializeAttribute(bool include, bool key)
        {
            Include = include;
            Key = key;
        }
    }
}