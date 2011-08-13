using System.Collections.Generic;
using Stump.Core.Pool;
using Stump.Core.Reflection;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Worlds.Groups
{
    public class GroupManager : Singleton<GroupManager>
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();
        private readonly Dictionary<int, Group> m_groups = new Dictionary<int, Group>();

        public Group Create(Character leader)
        {
            var group = new Group(m_idProvider.Pop());
            group.AddMember(leader);

            m_groups.Add(group.Id, group);

            return group;
        }

        public void Remove(Group @group)
        {
            m_groups.Remove(@group.Id);

            m_idProvider.Push(@group.Id);
        }

        public Group GetGroup(int id)
        {
            return m_groups.ContainsKey(id) ? m_groups[id] : null;
        }
    }
}