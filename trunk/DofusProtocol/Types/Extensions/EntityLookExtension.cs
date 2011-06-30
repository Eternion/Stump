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

        

        public static EntityLook Copy(this EntityLook entityLook)
        {
            return new EntityLook(
                entityLook.bonesId,
                entityLook.skins,
                entityLook.indexedColors,
                entityLook.scales,
                entityLook.subentities);
        }

        public static string ConvertToString(this EntityLook entityLook)
        {
            var result = new StringBuilder();

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

            int cursorPos = 1;
            int separatorPos = str.IndexOf('|');

            if (separatorPos == -1)
            {
                separatorPos = str.IndexOf("}");

                if (separatorPos == -1)
                    throw new Exception("Incorrect EntityLook format : " + str);
            }

            short bonesId = short.Parse(str.Substring(cursorPos, separatorPos - cursorPos));
            cursorPos = separatorPos + 1;

            var skins = new short[0];
            if ((separatorPos = str.IndexOf('|', cursorPos)) != -1 ||
                (separatorPos = str.IndexOf('}', cursorPos)) != -1)
            {
                int subseparatorPos = str.IndexOf(',', cursorPos, separatorPos - cursorPos);
                skins = new short[str.CountOccurences(',', cursorPos, separatorPos - cursorPos) + 1];

                // if there are comma
                int index = 0;
                while (subseparatorPos != -1)
                {
                    skins[index] = short.Parse(str.Substring(cursorPos, subseparatorPos - cursorPos));
                    cursorPos = subseparatorPos + 1;

                    subseparatorPos = str.IndexOf(',', cursorPos, separatorPos - cursorPos);
                    index++;
                }

                // if not empty
                if (separatorPos > cursorPos)
                    skins[index] = short.Parse(str.Substring(cursorPos, separatorPos - cursorPos));

                cursorPos = separatorPos + 1;
            }

            var colors = new int[0];
            if ((separatorPos = str.IndexOf('|', cursorPos)) != -1 ||
                (separatorPos = str.IndexOf('}', cursorPos)) != -1)
            {
                int subseparatorPos = str.IndexOf(',', cursorPos, separatorPos - cursorPos);
                colors = new int[str.CountOccurences(',', cursorPos, separatorPos - cursorPos) + 1];

                // if there are comma
                int index = 0;
                while (subseparatorPos != -1)
                {
                    colors[index] = ParseIndexedColor(str.Substring(cursorPos, subseparatorPos - cursorPos));
                    cursorPos = subseparatorPos + 1;

                    subseparatorPos = str.IndexOf(',', cursorPos, separatorPos - cursorPos);
                    index++;
                }

                // if not empty
                if (separatorPos > cursorPos)
                    colors[index] = ParseIndexedColor(str.Substring(cursorPos, separatorPos - cursorPos));

                cursorPos = separatorPos + 1;
            }

            var scales = new short[0];
            if ((separatorPos = str.IndexOf('|', cursorPos)) != -1 ||
                (separatorPos = str.IndexOf('}', cursorPos)) != -1)
            {
                int subseparatorPos = str.IndexOf(',', cursorPos, separatorPos - cursorPos);
                scales = new short[str.CountOccurences(',', cursorPos, separatorPos - cursorPos) + 1];

                // if there are comma
                int index = 0;
                while (subseparatorPos != -1)
                {
                    scales[index] = short.Parse(str.Substring(cursorPos, subseparatorPos - cursorPos));
                    cursorPos = subseparatorPos + 1;

                    subseparatorPos = str.IndexOf(',', cursorPos, separatorPos - cursorPos);
                    index++;
                }

                // if not empty
                if (separatorPos > cursorPos)
                    scales[index] = short.Parse(str.Substring(cursorPos, separatorPos - cursorPos));

                cursorPos = separatorPos + 1;
            }

            var subEntities = new List<SubEntity>();
            while (cursorPos < str.Length)
            {
                int atSeparatorIndex = str.IndexOf('@', cursorPos, 3); // max size of a byte = 255, so 3 characters
                int equalsSeparatorIndex = str.IndexOf('=', atSeparatorIndex + 1, 3); // max size of a byte = 255, so 3 characters
                byte category = byte.Parse(str.Substring(cursorPos, atSeparatorIndex - cursorPos));
                byte index = byte.Parse(str.Substring(atSeparatorIndex + 1, equalsSeparatorIndex - (atSeparatorIndex + 1)));

                int hookDepth = 0;
                int i = equalsSeparatorIndex + 1;
                var subEntity = new StringBuilder();
                do
                {
                    subEntity.Append(str[i]);

                    if (str[i] == '{')
                        hookDepth++;

                    else if (str[i] == '}')
                        hookDepth--;

                    i++;
                } while (hookDepth > 0);

                subEntities.Add(new SubEntity(category, index, ToEntityLook(subEntity.ToString())));

                cursorPos = i + 1; // ignore the comma or the last '}' char
            }

            return new EntityLook(bonesId, skins, colors, scales, subEntities.ToArray());
        }

        private static int ParseIndexedColor(string str)
        {
            int signPos = str.IndexOf("=");
            bool hexNumber = str[signPos + 1] == '#';

            int index = int.Parse(str.Substring(0, signPos));
            int color = int.Parse(str.Substring(signPos + (hexNumber ? 2 : 1), str.Length - (signPos + (hexNumber ? 2 : 1))),
                                  hexNumber ? NumberStyles.HexNumber : NumberStyles.Integer);

            return (index << 24) | color;
        }
    }
}