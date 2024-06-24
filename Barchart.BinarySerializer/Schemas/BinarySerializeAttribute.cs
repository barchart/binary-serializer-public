namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Class that specifies attributes that needs to be set on the property or field of the class or structure
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