using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using NetworkGuildMember = Stump.DofusProtocol.Types.GuildMember;

namespace Stump.Server.WorldServer.Game.Guilds
{
    public class GuildMember
    {
        public GuildMember(GuildMemberRecord record)
        {
            Record = record;
        }

        public GuildMember(Guild guild, Character character)
        {
            Record = new GuildMemberRecord
                {
                    CharacterId = character.Id,
                    AccountId = character.Account.Id,
                    Name = character.Name,
                    Level = character.Level,
                    Breed = character.BreedId,
                    Sex = character.Sex,
                    AlignmentSide = character.AlignmentSide,
                    GivenExperience = 0,
                    GivenPercent = 0,
                    LastConnection = DateTime.Now,
                    RankId = 0,
                    GuildId = guild.Id,
                    Rights = GuildRightsBitEnum.GUILD_RIGHT_NONE,
                };

            Guild = guild;
            Character = character;
            IsDirty = true;
            IsNew = true;
        }

        public GuildMemberRecord Record
        {
            get;
            private set;
        }

        public int Id
        {
            get { return Record.CharacterId; }
        }

        /// <summary>
        ///     Null if the character isn't connected.
        /// </summary>
        public Character Character
        {
            get;
            private set;
        }

        public bool IsConnected
        {
            get { return Character != null; }
        }

        public Guild Guild
        {
            get;
            private set;
        }

        public long GivenExperience
        {
            get { return Record.GivenExperience; }
            set
            {
                Record.GivenExperience = value;
                IsDirty = true;
            }
        }

        public byte GivenPercent
        {
            get { return Record.GivenPercent; }
            set
            {
                Record.GivenPercent = value;
                IsDirty = true;
            }
        }

        public GuildRightsBitEnum Rights
        {
            get { return Record.Rights; }
            set
            {
                Record.Rights = value;
                IsDirty = true;
            }
        }

        public short RankId
        {
            get { return Record.RankId; }
            set
            {
                Record.RankId = value;
                IsDirty = true;
            }
        }

        public bool IsBoss
        {
            get { return RankId == 1; }
        }

        public string Name
        {
            get
            {
                return Record.Name;
            }
            set
            {
                Record.Name = value;
                IsDirty = true;
            }
        }

        public byte Level
        {
            get
            {
                return Record.Level;
            }
            set
            {
                Record.Level = value;
                IsDirty = true;
            }
        }

        public AlignmentSideEnum AlignmentSide
        {
            get { return Record.AlignmentSide; }
            set
            {
                Record.AlignmentSide = value;
                IsDirty = true;
            }
        }

        public PlayableBreedEnum Breed
        {
            get { return Record.Breed; }
            set
            {
                Record.Breed = value;
                IsDirty = true;
            }
        }

        public SexTypeEnum Sex
        {
            get { return Record.Sex; }
            set
            {
                Record.Sex = value;
                IsDirty = true;
            }
        }

        /// <summary>
        /// True if must be saved
        /// </summary>
        public bool IsDirty
        {
            get;
            protected set;
        }

        public bool IsNew
        {
            get;
            protected set;
        }

        public NetworkGuildMember GetNetworkGuildMember()
        {
            return new NetworkGuildMember(Id, Level, Name, (sbyte) Breed, Sex == SexTypeEnum.SEX_FEMALE, RankId,
                                          GivenExperience, (sbyte) GivenPercent, (uint) Rights, (sbyte) (IsConnected ? 1 : 0),
                                          (sbyte) AlignmentSide, (ushort) (DateTime.Now - Record.LastConnection).TotalHours, 0,
                                          Record.AccountId, 0);
        }

        public bool HasRight(GuildRightsBitEnum right)
        {
            return Rights == GuildRightsBitEnum.GUILD_RIGHT_BOSS || Rights.HasFlag(right);
        }

        public event Action<GuildMember> Connected;
        public event Action<GuildMember, Character> Disconnected;

        public void OnCharacterConnected(Character character)
        {
            if (character.Id != Record.CharacterId)
            {
                throw new Exception(string.Format("GuildMember.CharacterId ({0}) != characterid ({1})",
                                                  Record.CharacterId, character.Id));
            }

            Character = character;

            var evnt = Connected;
            if (evnt != null)
                evnt(this);
        }

        public void OnCharacterDisconnected(Character character)
        {
            Record.LastConnection = DateTime.Now;
            IsDirty = true;
            Character = null;

            var evnt = Disconnected;
            if (evnt != null)
                evnt(this, character);
        }

        public void BindGuild(Guild guild)
        {
            if (Guild != null)
                throw new Exception(string.Format("Guild already bound to GuildMember {0}", Id));

            Guild = guild;
        }

        public void Save(ORM.Database database)
        {
            if (IsNew)
                database.Insert(Record);
            else
                database.Update(Record);

            IsNew = false;
            IsDirty = false;
        }
    }
}