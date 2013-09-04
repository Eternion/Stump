using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Stump.Core.Extensions;

namespace Stump.DofusProtocol.Types.Extensions
{
    public static class GuildEmblemExtension
    {
        public static string ConvertToString(this GuildEmblem guildEmblem)
        {
            return guildEmblem.symbolShape.ToString() + "|" + guildEmblem.symbolColor.ToString() + "|" +
                     guildEmblem.backgroundShape.ToString() + "|" + guildEmblem.backgroundColor.ToString();
        }

        public static GuildEmblem ToGuildEmblem(this string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new Exception("Incorrect GuildEmblem format : " + str);

            var result = str.Split('|');

            return new GuildEmblem(
                    short.Parse(result[0]),
                    int.Parse(result[1]),
                    short.Parse(result[2]),
                    int.Parse(result[3])
                );
        }
    }
}