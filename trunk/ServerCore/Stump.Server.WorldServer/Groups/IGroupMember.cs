using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Groups
{
    public interface IGroupMember
    {
        LivingEntity Entity
        {
            get;
        }

        IGroup GroupOwner
        {
            get;
        }
    }
}