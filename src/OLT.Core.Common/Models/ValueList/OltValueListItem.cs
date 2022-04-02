namespace OLT.Core
{
    public class OltValueListItem<T> : IOltValueListItem<T>
    {
        public OltValueListItem()
        {

        }

        public OltValueListItem(string label, T value)
        {
            Label = label;
            Value = value;
        }

        public T Value { get; set; }
        public string Label { get; set; }
    }
}
