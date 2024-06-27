namespace Barchart.BinarySerializer.Schemas
{
    /// <summary>
    ///     An attribute which can be used to define properties (or fields) of
    ///     a class (or struct) which should be serialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BinarySerializeAttribute : Attribute
    {
        #region Fields

        private readonly bool _key;
        private readonly bool _include;

        #endregion

        #region Constructor(s)

        public BinarySerializeAttribute(bool key = false, bool include = true)
        {
            _key = key;
            _include = include;
        }

        #region Properties

        /// <summary>
        ///     Indicates whether the property is a key. Typically, each type
        ///     has one key property. Although, multiple properties can be
        ///     defined as keys (when multiple properties are required to
        ///     define uniqueness).
        /// </summary>
        public bool Key => _key;

        /// <summary>
        ///     Indicates whether the property should be included in serialization.
        ///     When false, the property will be excluded from serialization (and
        ///     ignored completely by this library).
        /// </summary>
        public bool Include => _include;

        #endregion
        
        #endregion
    }
}