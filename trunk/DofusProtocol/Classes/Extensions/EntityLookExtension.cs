using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Stump.DofusProtocol.Classes.Extensions
{
    public static class EntityLookExtension
    {
        private static Tuple<int, int> ExtractIndexedColor(int indexedColor)
        {
            string str = indexedColor.ToString("X");

            int index = int.Parse(str.Substring(0, 1));
            int color = int.Parse(str.Substring(1), NumberStyles.HexNumber);

            return new Tuple<int, int>(index, color);
        }

        private static int ParseIndexedColor(string str)
        {
            int signPos = str.IndexOf("=");
            bool hexNumber = str[signPos + 1] == '#';

            int index = int.Parse(str.Substring(0, signPos));
            int color = int.Parse(str.Substring(signPos + ( hexNumber ? 2 : 1 ), str.Length - (signPos + ( hexNumber ? 2 : 1 ))),
                                  hexNumber ? NumberStyles.HexNumber : NumberStyles.Integer);

            return int.Parse(index.ToString() + color.ToString("X6"), NumberStyles.HexNumber);
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

        public static string ToString(this EntityLook entityLook)
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

            uint bonesId = uint.Parse(str.Substring(charPos, separatorPos - charPos));
            charPos = separatorPos + 1;

            var skins = new List<uint>();
            if ((separatorPos = str.IndexOf('|', charPos)) != -1 ||
                (separatorPos = str.IndexOf('}', charPos)) != -1)
            {
                int subseparatorPos = str.IndexOf(',', charPos, separatorPos - charPos);

                // if there are comma
                while (subseparatorPos != -1)
                {
                    skins.Add(uint.Parse(str.Substring(charPos, subseparatorPos - charPos)));
                    charPos = subseparatorPos + 1;

                    subseparatorPos = str.IndexOf(',', charPos, separatorPos - charPos);
                }

                // if not empty
                if (separatorPos > charPos)
                    skins.Add(uint.Parse(str.Substring(charPos, separatorPos - charPos)));

                charPos = separatorPos + 1;
            }

            var colors = new List<int>();
            if ((separatorPos = str.IndexOf('|', charPos)) != -1 ||
                (separatorPos = str.IndexOf('}', charPos)) != -1)
            {
                int subseparatorPos = str.IndexOf(',', charPos, separatorPos - charPos);

                // if there are comma
                while (subseparatorPos != -1)
                {
                    colors.Add(ParseIndexedColor(str.Substring(charPos, subseparatorPos - charPos)));
                    charPos = subseparatorPos + 1;

                    subseparatorPos = str.IndexOf(',', charPos, separatorPos - charPos);
                }

                // if not empty
                if (separatorPos > charPos)
                    colors.Add(ParseIndexedColor(str.Substring(charPos, separatorPos - charPos)));

                charPos = separatorPos + 1;
            }

            var scales = new List<int>();
            if ((separatorPos = str.IndexOf('|', charPos)) != -1 ||
                (separatorPos = str.IndexOf('}', charPos)) != -1)
            {
                int subseparatorPos = str.IndexOf(',', charPos, separatorPos - charPos);

                // if there are comma
                while (subseparatorPos != -1)
                {
                    scales.Add(int.Parse(str.Substring(charPos, subseparatorPos - charPos)));
                    charPos = subseparatorPos + 1;

                    subseparatorPos = str.IndexOf(',', charPos, separatorPos - charPos);
                }

                // if not empty
                if (separatorPos > charPos)
                    scales.Add(int.Parse(str.Substring(charPos, separatorPos - charPos)));

                charPos = separatorPos + 1;
            }

            return new EntityLook(bonesId, skins, colors, scales, new List<SubEntity>());
        }
    }
}