
using Stump.Server.BaseServer.Manager;

namespace Stump.Server.WorldServer.Groups
{
    public class GroupManager : InstanceManager<IGroup>
    {
        /// <summary>
        ///   Add the group
        /// </summary>
        /// <param name = "grp"></param>
        /// <returns>groupId</returns>
        public static int CreateGroup(IGroup grp)
        {
            return CreateInstance(grp);
        }

        public static bool RemoveGroup(IGroup grp)
        {
            return RemoveInstance(grp);
        }

        public static IGroup GetGroupOfId(int id)
        {
            return GetInstanceById(id);
        }
    }
}