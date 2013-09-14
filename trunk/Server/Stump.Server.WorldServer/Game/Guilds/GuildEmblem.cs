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
        private Color m_backgroundColor;
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
            get { return Record.EmblemBackgroundShape; }
            set
            {
                Record.EmblemBackgroundShape = value;
                IsDirty = true;
            }
        }

        public Color BackgroundColor
        {
            get { return m_backgroundColor; }
            set
            {
                m_backgroundColor = value;
                Record.EmblemBackgroundColor = value.ToArgb();
                IsDirty = true;
            }
        }

        public short SymbolShape
        {
            get { return Record.EmblemForegroundShape; }
            set
            {
                Record.EmblemForegroundShape = value;
                IsDirty = true;
            }
        }

        public Color SymbolColor
        {
            get { return m_foregroundColor; }
            set
            {
                m_foregroundColor = value;
                Record.EmblemForegroundColor = value.ToArgb();
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

        public bool DoesEmblemMatch(NetworkGuildEmblem emblem)
        {
            return BackgroundColor.ToArgb() == emblem.backgroundColor &&
                   BackgroundShape == emblem.backgroundShape &&
                   SymbolColor.ToArgb() == emblem.symbolColor &&
                   SymbolShape == emblem.symbolShape;
        }

        public bool DoesEmblemMatch(GuildEmblem emblem)
        {
            return BackgroundColor == emblem.BackgroundColor &&
                   BackgroundShape == emblem.BackgroundShape &&
                   SymbolColor == emblem.SymbolColor &&
                   SymbolShape == emblem.SymbolShape;
        }

        public NetworkGuildEmblem GetNetworkGuildEmblem()
        {
            return new NetworkGuildEmblem()
            {
                backgroundColor = Record.EmblemBackgroundColor,
                backgroundShape = Record.EmblemBackgroundShape,
                symbolColor = Record.EmblemForegroundColor,
                symbolShape = Record.EmblemForegroundShape
            };
        }
    }
}
