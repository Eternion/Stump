namespace Stump.ORM
{
    public interface ISaveIntercepter
    {
        void BeforeSave(bool insert);
    }
}