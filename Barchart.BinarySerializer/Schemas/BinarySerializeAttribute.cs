namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     An attribute which can be used to define properties of a class (or struct)
    ///     which should be serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BinarySerializeAttribute : Attribute
    {
        #region Properties

        public bool Key { get; }

        public bool Include { get; }

        #endregion

        #region Constructor(s)

        public BinarySerializeAttribute(bool key = false, bool include = true)
        {
            Key = key;
            Include = include;
        }

        #endregion
    }
}