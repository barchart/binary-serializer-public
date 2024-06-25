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

        /// <summary>
        ///     Indicates whether the property or field is a key.
        ///     A key property or field is used to uniquely identify an object instance.
        /// </summary>
        public bool Key { get; }

        /// <summary>
        ///     Indicates whether the property or field should be included in serialization.
        ///     If set to true, the property or field will be included in the serialized output.
        ///     If set to false, the property or field will be excluded from the serialized output.
        ///     Default value is true.
        /// </summary>
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