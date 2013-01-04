namespace Stump.Server.WorldServer.Database.Spells
{
    public interface ISpellRecord
    {
        int SpellId
        {
            get;
            set;
        }

        short Level
        {
            get;
            set;
        }
    }
}