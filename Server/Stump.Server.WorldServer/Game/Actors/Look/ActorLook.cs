#region License GNU GPL

// ActorLook.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using Stump.Core.Cache;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Game.Actors.Look
{
    public class ActorLook
    {
        private const short PET_SIZE = 75;

        private List<short> m_scales = new List<short>();
        private List<short> m_skins = new List<short>();
        private List<SubActorLook> m_subLooks = new List<SubActorLook>();
        private Dictionary<int, Color> m_colors = new Dictionary<int, Color>();

        public ActorLook()
        {
            m_entityLook = new ObjectValidator<EntityLook>(BuildEntityLook);
        }

        public ActorLook(short bones, IEnumerable<short> skins, Dictionary<int, Color> indexedColors, IEnumerable<short> scales, 
            IEnumerable<SubActorLook> subLooks)
            : this()
        {
            m_bonesID = bones;
            m_skins = skins.ToList();
            m_colors = indexedColors;
            m_scales = scales.ToList();
            m_subLooks = subLooks.ToList();
        }

        public short BonesID
        {
            get { return m_bonesID; }
            set { m_bonesID = value;
                m_entityLook.Invalidate();
            }
        }

        public ReadOnlyCollection<short> Skins
        {
            get { return m_skins.AsReadOnly(); }
        }

        public ReadOnlyCollection<short> Scales
        {
            get { return m_scales.AsReadOnly(); }
        }

        public ReadOnlyCollection<SubActorLook> SubLooks
        {
            get { return m_subLooks.AsReadOnly(); }
        }

        public ActorLook PetLook
        {
            get
            {
                var subLook = m_subLooks.FirstOrDefault(
                    x => x.BindingCategory == SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_PET);

                return subLook != null ? subLook.Look : null;
            }
        }

        public ReadOnlyDictionary<int, Color> Colors
        {
            get { return new ReadOnlyDictionary<int, Color>(m_colors); }
        }

        public void SetSkins(params short[] skins)
        {
            m_skins = skins.ToList();
            m_entityLook.Invalidate();
        }

        public void AddSkin(short skin)
        {
            m_skins.Add(skin);
            m_entityLook.Invalidate();
        }

        public void SetScales(params short[] scales)
        {
            m_scales = scales.ToList();
            m_entityLook.Invalidate();
        }

        public void AddScale(short scale)
        {
            m_scales.Add(scale);
            m_entityLook.Invalidate();
        }

        public void Rescale(double factor)
        {
            if (m_scales.Count == 0)
                AddScale((short) (100*factor));
            else
            {
                SetScales(m_scales.Select(x => (short) (x*factor)).ToArray());
            }
        }

        public void AddColor(int index, Color color)
        {
            m_colors.Add(index, color);
            m_entityLook.Invalidate();
        }

        public void SetColors(params Color[] colors)
        {
            var index = 1;

            m_colors = colors.ToDictionary(x => index++);

            m_entityLook.Invalidate();
        }

        public void SetColors(int[] indexes, Color[] colors)
        {
            if (indexes.Length != colors.Length)
                throw new ArgumentException("indexes.Length != colors.Length");

            m_colors.Clear();
            for (var i = 0; i < indexes.Length; i++)
            {
                m_colors.Add(indexes[i], colors[i]);
            }

            m_entityLook.Invalidate();
        }

        public void AddSubLook(SubActorLook subLook)
        {
            m_subLooks.Add(subLook);
            m_entityLook.Invalidate();

            subLook.SubEntityValidator.ObjectInvalidated += OnSubEntityInvalidated;
        }

        public void SetSubLooks(params SubActorLook[] subLooks)
        {
            foreach (var subLook in m_subLooks)
                subLook.SubEntityValidator.ObjectInvalidated -= OnSubEntityInvalidated;

            m_subLooks= subLooks.ToList();
            m_entityLook.Invalidate();

            foreach (var subLook in m_subLooks)
                subLook.SubEntityValidator.ObjectInvalidated += OnSubEntityInvalidated;
        }

        public void SetPetSkin(short skin)
        {
            var petLook = PetLook;

            AddSubLook(
                new SubActorLook(0, SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_PET,
                                 petLook = new ActorLook()));

            petLook.SetScales(PET_SIZE);
            petLook.BonesID = skin;
        }

        public void RemovePets()
        {
            m_subLooks.RemoveAll(x => x.BindingCategory == SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_PET);
            m_entityLook.Invalidate();
        }


        public void SetRiderLook(ActorLook look)
        {
            AddSubLook(new SubActorLook(0, SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MOUNT_DRIVER,
                                  look));
        }

        public void RemoveMounts()
        {
            m_subLooks.RemoveAll(x => x.BindingCategory == SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MOUNT_DRIVER);
            m_entityLook.Invalidate();
        }

        public void RemoveAuras()
        {
            m_subLooks.RemoveAll(x => x.BindingCategory == SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_BASE_FOREGROUND
                && (x.Look.BonesID == 169 || x.Look.BonesID == 170));

            m_entityLook.Invalidate();
        }

        private void OnSubEntityInvalidated(ObjectValidator<SubEntity> obj)
        {
            m_entityLook.Invalidate();
        }

        #region EntityLook

        private readonly ObjectValidator<EntityLook> m_entityLook;
        private short m_bonesID;

        public ObjectValidator<EntityLook> EntityLookValidator
        {
            get { return m_entityLook; }
        }

        private EntityLook BuildEntityLook()
        {
            return new EntityLook(
                BonesID,
                Skins,
                Colors.Select(x => x.Key << 24 | x.Value.ToArgb() & 0xFFFFFF).ToArray(),
                Scales,
                SubLooks.Select(x => x.GetSubEntity()).ToArray());
        }

        public EntityLook GetEntityLook()
        {
            return m_entityLook;
        }

        #endregion

        public ActorLook Clone()
        {
            return new ActorLook
                {
                    BonesID = m_bonesID,
                    m_colors = m_colors.ToDictionary(x => x.Key, x => x.Value),
                    m_skins = m_skins.ToList(),
                    m_scales = m_scales.ToList(),
                    m_subLooks = m_subLooks.Select(x => new SubActorLook(x.BindingIndex, x.BindingCategory, x.Look.Clone())).ToList(),
                };
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            // todo : header

            result.Append("{");

            var missingBars = 0;

            result.Append(BonesID);

            if (Skins == null || !Skins.Any())
                missingBars++;
            else
            {
                result.Append("|".ConcatCopy(missingBars + 1));
                missingBars = 0;
                result.Append(string.Join(",", Skins));
            }

            if (Colors == null || !Colors.Any())
                missingBars++;
            else
            {
                result.Append("|".ConcatCopy(missingBars + 1));
                missingBars = 0;
                result.Append(string.Join(",", from entry in Colors
                                               select entry.Key + "=" + entry.Value.ToArgb()));
            }

            if (Scales == null || !Scales.Any())
                missingBars++;
            else
            {
                result.Append("|".ConcatCopy(missingBars + 1));
                missingBars = 0;
                result.Append(string.Join(",", Scales));
            }

            if (SubLooks == null || !SubLooks.Any())
                missingBars++;
            else
            {
                result.Append("|".ConcatCopy(missingBars + 1));
                result.Append(string.Join(",", SubLooks.Select(entry => entry)));
            }

            result.Append("}");

            return result.ToString();
        }

        public static ActorLook Parse(string str)
        {
            if (string.IsNullOrEmpty(str) || str[0] != '{')
                throw new Exception("Incorrect EntityLook format : " + str);

            var cursorPos = 1;
            var separatorPos = str.IndexOf('|');

            if (separatorPos == -1)
            {
                separatorPos = str.IndexOf("}");

                if (separatorPos == -1)
                    throw new Exception("Incorrect EntityLook format : " + str);
            }

            short bonesId = short.Parse(str.Substring(cursorPos, separatorPos - cursorPos));
            cursorPos = separatorPos + 1;

            var skins = new short[0];
            if (( separatorPos = str.IndexOf('|', cursorPos) ) != -1 ||
                 ( separatorPos = str.IndexOf('}', cursorPos) ) != -1)
            {
                skins = ParseCollection(str.Substring(cursorPos, separatorPos - cursorPos), short.Parse);
                cursorPos = separatorPos + 1;
            }

            var colors = new Tuple<int, int>[0];
            if (( separatorPos = str.IndexOf('|', cursorPos) ) != -1 ||
                 ( separatorPos = str.IndexOf('}', cursorPos) ) != -1) // if false there are no informations between the two separators
            {
                colors = ParseCollection(str.Substring(cursorPos, separatorPos - cursorPos), ParseIndexedColor);
                cursorPos = separatorPos + 1;
            }

            var scales = new short[0];
            if (( separatorPos = str.IndexOf('|', cursorPos) ) != -1 ||
                 ( separatorPos = str.IndexOf('}', cursorPos) ) != -1) // if false there are no informations between the two separators
            {
                scales = ParseCollection(str.Substring(cursorPos, separatorPos - cursorPos), short.Parse);
                cursorPos = separatorPos + 1;
            }

            var subEntities = new List<SubActorLook>();
            while (cursorPos < str.Length)
            {
                var atSeparatorIndex = str.IndexOf('@', cursorPos, 3); // max size of a byte = 255, so 3 characters
                var equalsSeparatorIndex = str.IndexOf('=', atSeparatorIndex + 1, 3); // max size of a byte = 255, so 3 characters
                var category = byte.Parse(str.Substring(cursorPos, atSeparatorIndex - cursorPos));
                var index = byte.Parse(str.Substring(atSeparatorIndex + 1, equalsSeparatorIndex - ( atSeparatorIndex + 1 )));

                var hookDepth = 0;
                var i = equalsSeparatorIndex + 1;
                var subEntity = new StringBuilder();
                do
                {
                    subEntity.Append(str[i]);

                    switch (str[i])
                    {
                        case '{':
                            hookDepth++;
                            break;
                        case '}':
                            hookDepth--;
                            break;
                    }

                    i++;
                } while (hookDepth > 0);

                subEntities.Add(new SubActorLook((sbyte)index, (SubEntityBindingPointCategoryEnum) category, Parse(subEntity.ToString())));

                cursorPos = i + 1; // ignore the comma and the last '}' char
            }

            return new ActorLook(bonesId, skins, colors.ToDictionary(x => x.Item1, x => Color.FromArgb(x.Item2)), scales, subEntities.ToArray());
        }

        private static Tuple<int, int> ParseIndexedColor(string str)
        {
            var signPos = str.IndexOf("=");
            var hexNumber = str[signPos + 1] == '#';

            var index = int.Parse(str.Substring(0, signPos));
            var color = int.Parse(str.Substring(signPos + ( hexNumber ? 2 : 1 ), str.Length - ( signPos + ( hexNumber ? 2 : 1 ) )),
                                  hexNumber ? NumberStyles.HexNumber : NumberStyles.Integer);

            return Tuple.Create(index, color);
        }

        private static T[] ParseCollection<T>(string str, Func<string, T> converter)
        {
            if (string.IsNullOrEmpty(str))
                return new T[0];

            var cursorPos = 0;
            var subseparatorPos = str.IndexOf(',', cursorPos);

            // if not empty
            if (subseparatorPos == -1)
                return new[] { converter(str) };

            var collection = new T[str.CountOccurences(',', cursorPos, str.Length - cursorPos) + 1];

            // if there are commas
            int index = 0;
            while (subseparatorPos != -1)
            {
                collection[index] = converter(str.Substring(cursorPos, subseparatorPos - cursorPos));
                cursorPos = subseparatorPos + 1;

                subseparatorPos = str.IndexOf(',', cursorPos);
                index++;
            }

            // last value is not read yet because subseparatorPos = -1
            // 'cause the value is after the comma
            collection[index] = converter(str.Substring(cursorPos, str.Length - cursorPos));

            return collection;
        }
    }
}