namespace Stump.Core.Sql
{
    public struct RawData
    {
        public RawData(object data)
        {
            Data = data;
        }

        public object Data;

        public static RawData Raw(object obj)
        {
            return new RawData(obj);
        }
    }
}