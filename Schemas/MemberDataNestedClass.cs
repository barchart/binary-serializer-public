namespace JerqAggregatorNew.Schemas
{
	public class MemberDataNestedClass<T> : MemberData<T>
    {
		public ISchema Schema { get; set; }
	}
}

