using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Guilds
{
    public class Guild : DataManager<Guild>, ISaveable
    {
        [Variable(true)]
        public static int MaxMembersNumber = 50;

        private List<GuildRecord> m_guilds;
        private List<GuildMemberRecord> m_guildMember;

        [Initialization(InitializationPass.Sixth)]
        public override void Initialize()
        {
            Load();
        }

        public ReadOnlyCollection<GuildRecord> Guilds
        {
            get { return m_guilds.AsReadOnly(); }
        }

        public ReadOnlyCollection<GuildMemberRecord> GuildMembers
        {
            get { return m_guildMember.AsReadOnly(); }
        }

        public void Load()
        {
            m_guilds = WorldServer.Instance.DBAccessor.Database.Fetch<GuildRecord>(string.Format(GuildRelator.FetchQuery));
            m_guildMember = WorldServer.Instance.DBAccessor.Database.Fetch<GuildMemberRecord>(string.Format(GuildMemberRelator.FetchQuery));
        }

        public void Save()
        {
            var database = WorldServer.Instance.DBAccessor.Database;

            foreach (var guild in m_guilds)
            {
                database.Save(guild);
            }

            foreach (var guildMember in m_guildMember)
            {
                database.Save(guildMember);
            }
        }
    }
}
