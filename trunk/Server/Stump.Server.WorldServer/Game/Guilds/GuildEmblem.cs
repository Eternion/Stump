using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.Server.WorldServer.Database.Guilds;
using NetworkGuildEmblem = Stump.DofusProtocol.Types.GuildEmblem;

namespace Stump.Server.WorldServer.Game.Guilds
{
    public class GuildEmblem
    {
        private short m_backgroundShape;
        private Color m_backgroundColor;
        private short m_foregroundShape;
        private Color m_foregroundColor;

        public GuildEmblem(GuildRecord record)
        {
            Record = record;
        }

        public GuildRecord Record
        {
            get;
            set;
        }

        public short BackgroundShape
        {
            get { return m_backgroundShape; }
            set
            {
                m_backgroundShape = value;
                IsDirty = true;
            }
        }

        public Color BackgroundColor
        {
            get { return m_backgroundColor; }
            set
            {
                m_backgroundColor = value;
                IsDirty = true;
            }
        }

        public short SymbolShape
        {
            get { return m_foregroundShape; }
            set
            {
                m_foregroundShape = value;
                IsDirty = true;
            }
        }

        public Color SymbolColor
        {
            get { return m_foregroundColor; }
            set
            {
                m_foregroundColor = value;
                IsDirty = true;
            }
        }

        public bool IsDirty
        {
            get;
            set;
        }

        public void ChangeEmblem(NetworkGuildEmblem emblem)
        {
            BackgroundColor = Color.FromArgb(emblem.backgroundColor);
            BackgroundShape = emblem.backgroundShape;
            SymbolColor = Color.FromArgb(emblem.symbolColor);
            SymbolShape = emblem.symbolShape;
        }

        public NetworkGuildEmblem GetNetworkGuildEmblem()
        {
            return new NetworkGuildEmblem()
                {
                    backgroundColor = BackgroundColor.ToArgb(),
                    backgroundShape = BackgroundShape,
                    symbolColor = SymbolColor.ToArgb(),
                    symbolShape = SymbolShape
                };
        }
    }
}
