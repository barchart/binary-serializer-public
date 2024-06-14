namespace Barchart.BinarySerializer.Tests
{
    class ListOfObjectsSpecs1 {
        public bool property1;
        public SerializationOptions property2;
        public float? property3;
        public List<ListOfObjectsSpecs2>? example;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var othwerListOfObjectsSpecs2 = (ListOfObjectsSpecs1)obj;

            return property1 == othwerListOfObjectsSpecs2.property1 &&
                   property2 == othwerListOfObjectsSpecs2.property2 &&
                   property3 == othwerListOfObjectsSpecs2.property3 &&
                   ListEquals(example, othwerListOfObjectsSpecs2.example);
        }

        private bool ListEquals<T>(List<T>? list1, List<T>? list2) where T : class
        {
            // Handle nullability and compare list contents
            if (list1 == list2) return true;
            if (list1 == null || list2 == null) return false;
            if (list1.Count != list2.Count) return false;

            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
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
                return property1.GetHashCode() + property2.GetHashCode() + (property3 != null ? property3.GetHashCode() : 0) + (example != null ? example.GetHashCode() : 0); 
            }
        }
    }

    class ListOfObjectsSpecs2 {
        public bool property1;
        public float? property3;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherListOfObjectsSpecs2 = (ListOfObjectsSpecs2)obj;

            return property1 == otherListOfObjectsSpecs2.property1 &&
                   property3 == otherListOfObjectsSpecs2.property3;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return property1.GetHashCode() + (property3 != null ? property3.GetHashCode() : 0);
            }
        }
    }
}