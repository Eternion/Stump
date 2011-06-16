using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Stump.Core.Extensions;

namespace Stump.DofusProtocol.Types.Extensions
{
    public static class EntityLookExtension
    {
        private static Tuple<int, int> ExtractIndexedColor(int indexedColor)
        {
            int index = indexedColor >> 24;
            int color = indexedColor & 0xFFFFFF;

            return new Tuple<int, int>(index, color);
        }

        private static int ParseIndexedColor(string str)
        {
            int signPos = str.IndexOf("=");
            bool hexNumber = str[signPos + 1] == '#';

            int index = int.Parse(str.Substring(0, signPos));
            int color = int.Parse(str.Substring(signPos + ( hexNumber ? 2 : 1 ), str.Length - (signPos + ( hexNumber ? 2 : 1 ))),
                                  hexNumber ? NumberStyles.HexNumber : NumberStyles.Integer);

            return (index << 24) | color;
        }

        public static EntityLook Copy(this EntityLook entityLook)
        {
            return new EntityLook(
                entityLook.bonesId,
                entityLook.skins,
                entityLook.indexedColors,
                entityLook.scales,
                entityLook.subentities);
        }

        public static string eToString(this EntityLook entityLook)
        {
            StringBuilder result = new StringBuilder();

            // todo : header

            result.Append("{");

            result.Append(entityLook.bonesId + "|");
            result.Append(string.Join(",", entityLook.skins) + "|");
            result.Append(string.Join(",", from entry in entityLook.indexedColors
                                       let tuple = ExtractIndexedColor(entry)
                                       select tuple.Item1 + "=" + tuple.Item2) + "|");
            result.Append(string.Join(",", entityLook.scales));

            // todo : subentities

            result.Append("}");

            return result.ToString();
        }

        public static EntityLook ToEntityLook(this string str)
        {
            if (str[0] != '{')
                throw new Exception("Incorrect EntityLook format : " + str);

            int charPos = 1;
            int separatorPos = str.IndexOf('|');

            if (separatorPos == -1)
            {
                separatorPos = str.IndexOf("}");

                if (separatorPos == -1)
                    throw new Exception("Incorrect EntityLook format : " + str);
            }

            var bonesId = short.Parse(str.Substring(charPos, separatorPos - charPos));
            charPos = separatorPos + 1;

            var skins = new short[0];
            if ((separatorPos = str.IndexOf('|', charPos)) != -1 ||
                (separatorPos = str.IndexOf('}', charPos)) != -1)
            {
                int subseparatorPos = str.IndexOf(',', charPos, separatorPos - charPos);
                skins = new short[str.CountOccurences(',', charPos, separatorPos - charPos) + 1];

                // if there are comma
                int index = 0;
                while (subseparatorPos != -1)
                {
                    skins[index] = short.Parse(str.Substring(charPos, subseparatorPos - charPos));
                    charPos = subseparatorPos + 1;

                    subseparatorPos = str.IndexOf(',', charPos, separatorPos - charPos);
                    index++;
                }

                // if not empty
                if (separatorPos > charPos)
                    skins[index] = short.Parse(str.Substring(charPos, separatorPos - charPos));

                charPos = separatorPos + 1;
            }

            var colors = new int[0];
            if ((separatorPos = str.IndexOf('|', charPos)) != -1 ||
                (separatorPos = str.IndexOf('}', charPos)) != -1)
            {
                int subseparatorPos = str.IndexOf(',', charPos, separatorPos - charPos);
                colors = new int[str.CountOccurences(',', charPos, separatorPos - charPos) + 1];

                // if there are comma
                int index = 0;
                while (subseparatorPos != -1)
                {
                    colors[index] = ParseIndexedColor(str.Substring(charPos, subseparatorPos - charPos));
                    charPos = subseparatorPos + 1;

                    subseparatorPos = str.IndexOf(',', charPos, separatorPos - charPos);
                    index++;
                }

                // if not empty
                if (separatorPos > charPos)
                    colors[index] = ParseIndexedColor(str.Substring(charPos, separatorPos - charPos));

                charPos = separatorPos + 1;
            }

            var scales = new short[0];
            if ((separatorPos = str.IndexOf('|', charPos)) != -1 ||
                (separatorPos = str.IndexOf('}', charPos)) != -1)
            {
                int subseparatorPos = str.IndexOf(',', charPos, separatorPos - charPos);
                scales = new short[str.CountOccurences(',', charPos, separatorPos - charPos) + 1];

                // if there are comma
                int index = 0;
                while (subseparatorPos != -1)
                {
                    scales[index] = short.Parse(str.Substring(charPos, subseparatorPos - charPos));
                    charPos = subseparatorPos + 1;

                    subseparatorPos = str.IndexOf(',', charPos, separatorPos - charPos);
                    index++;
                }

                // if not empty
                if (separatorPos > charPos)
                    scales[index] = short.Parse(str.Substring(charPos, separatorPos - charPos));

                charPos = separatorPos + 1;
            }

            return new EntityLook(bonesId, skins, colors, scales, new SubEntity[0]);
        }
    }
}