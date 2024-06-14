using Barchart.BinarySerializer.Schemas;

namespace Barchart.BinarySerializer.Tests
{
    class ListSpecs
    {
        [BinarySerialize(key: false)]
        public SerializationOptions? enumField;

        [BinarySerialize(key: false)]
        public SerializationOptions? enumField2;

        [BinarySerialize(key: false)]
        public List<int> list = new() { 2 , 3 };

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherListSpecs = (ListSpecs)obj;

            return enumField == otherListSpecs.enumField &&
                   enumField2 == otherListSpecs.enumField2 &&
                   ListEquals(list, otherListSpecs.list);
        }

        private bool ListEquals(List<int> list1, List<int> list2)
        {
            if (list1 == list2) return true;
            if (list1 == null || list2 == null) return false;
            if (list1.Count != list2.Count) return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!EqualityComparer<int>.Default.Equals(list1[i], list2[i]))
                {
                    return false;
                }
            }

            return true;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return enumField.GetHashCode() + enumField2.GetHashCode() + list.GetHashCode(); 
            }
        }
    }
}