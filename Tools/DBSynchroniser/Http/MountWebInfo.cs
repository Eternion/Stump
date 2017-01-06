namespace DBSynchroniser.Http
{
    public class MountWebInfo
    {
        public MountWebInfo(int id, MountWebStat[] petWebEffects)
        {
            Id = id;
            Effects = petWebEffects;
        }

        public int Id
        {
            get;
            set;
        }

        public MountWebStat[] Effects
        {
            get;
            set;
        }
    }

    public class MountWebStat
    {
        public MountWebStat(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }
    }
}