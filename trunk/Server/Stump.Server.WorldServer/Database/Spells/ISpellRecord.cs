namespace Stump.Server.WorldServer.Database
{
    public interface ISpellRecord
    {
        int SpellId
        {
            get;
            set;
        }

        sbyte Level
        {
            get;
            set;
        }
    }
}