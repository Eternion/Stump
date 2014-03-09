using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.WorldServer.Database.Accounts
{
    public class UserGroup
    {
        private readonly UserGroupData m_data;

        public UserGroup(UserGroupData data, UserGroupCommand[] commands)
        {
            m_data = data;
            AvailableCommands = commands;
        }

        public int Id
        {
            get { return m_data.Id; }
        }

        public UserGroupCommand[] AvailableCommands
        {
            get;
            private set;
        }

        public string Name
        {
            get { return m_data.Name; }
        }

        public RoleEnum Role
        {
            get { return m_data.Role; }
        }

        public bool IsGameMaster
        {
            get { return m_data.IsGameMaster; }
        }

        public bool IsCommandAvailable(CommandBase command)
        {
            return Role >= command.RequiredRole || AvailableCommands.Any(x => command.Aliases.Contains(x.CommandAlias));
        }
    }
}