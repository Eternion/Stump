namespace Stump.ORM.Types
{
    public interface IValue
    {
        void BuildLoadQuery(object query);
        void BuildSaveQuery(object query);
        void BuildCreateQuery(object query);

        void Load(object reader);
    }
}