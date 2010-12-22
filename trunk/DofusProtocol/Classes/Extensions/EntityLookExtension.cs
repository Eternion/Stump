// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
            string result = "";

            // todo : header

            result += "{";

            result += entityLook.bonesId + "|";
            result += string.Join(",", entityLook.skins) + "|";
            result += string.Join(",", from entry in entityLook.indexedColors
                                       let tuple = ExtractIndexedColor(entry)
                                       select tuple.Item1 + "=" + tuple.Item2) + "|";
            result += string.Join(",", entityLook.scales);

            // todo : subentities

            result += "}";

            return result;
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