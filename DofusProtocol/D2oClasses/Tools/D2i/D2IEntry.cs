namespace Stump.DofusProtocol.D2oClasses.Tools.D2i
{
    public class D2IEntry<T>
    {

        public D2IEntry(T key, string text)
        {
            Key = key;
            Text = text;
        }

        public D2IEntry(T key, string text, string undiactricalText)
        {
            Key = key;
            Text = text;
            UnDiactricialText = undiactricalText;
            UseUndiactricalText = true;
        }

        public T Key
        {
            get;
            set;
        }

        public bool UseUndiactricalText
        {
            get;
            set;
        }

        public string UnDiactricialText
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
    }
}