namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     Class that specifies attributes that needs to be set on the property or field of the class or structure
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BinarySerializeAttribute : Attribute
    {
        #region Fields

        private readonly bool _key;
        private readonly bool _include;
        
        #endregion

        #region Properties

        public bool Key => _key;
        public bool Include => _include;
        
        #endregion

        #region Constructor(s)

        public BinarySerializeAttribute(bool key = false, bool include = true)
        {
            _key = key;
            _include = include;
        }

        #endregion
    }
}