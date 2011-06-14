namespace Stump.Server.WorldServer.Entities
{
    public class Title
    {
        public Title(uint id, string @params)
        {
            Id = id;
            Params = @params;
        }

        public uint Id
        {
            get;
            set;
        }

        public string Params
        {
            get;
            set;
        }
    }
}