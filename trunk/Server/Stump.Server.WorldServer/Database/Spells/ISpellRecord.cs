namespace Stump.Server.WorldServer.Database.Spells
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