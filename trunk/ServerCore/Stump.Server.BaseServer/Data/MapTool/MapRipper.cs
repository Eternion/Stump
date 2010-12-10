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
using System.IO;
using Stump.BaseCore.Framework.IO;

namespace Stump.Server.BaseServer.Data.MapTool
{
    public static class MapRipper
    {
        /// <summary>
        ///   Rewrite a .map file without superflous data
        /// </summary>
        /// <param name = "mapfilePath">The unripped .map file</param>
        /// <param name = "dest">Where to write the ripped .map file</param>
        public static void RipMapFile(string mapfilePath, string dest)
        {
            FileStream stream = File.Open(mapfilePath, FileMode.Open);
            int mapversion;
            int mapid;
            int relativeid;
            int maptype;
            int subareaid;
            int topid;
            int bottomid;
            int leftid;
            int rightid;
            int shadownbonusonentities;
            bool uselowpassfilter;
            bool usereverb;
            int presetid;
            const int cellsCount = 560;
            int[] floor;
            int[] losmov;
            int[] speed;
            int[] mapchangedata;

            using (var reader = new BigEndianReader(stream))
            {
                reader.ReadByte(); // header

                mapversion = reader.ReadInt();
                mapid = reader.ReadInt();
                relativeid = reader.ReadInt();
                maptype = reader.ReadInt();
                subareaid = reader.ReadInt();
                topid = reader.ReadInt();
                bottomid = reader.ReadInt();
                leftid = reader.ReadInt();
                rightid = reader.ReadInt();
                shadownbonusonentities = reader.ReadInt();

                if (mapversion >= 3)
                {
                    // background color
                    reader.ReadByte();
                    reader.ReadByte();
                    reader.ReadByte();
                }
                if (mapversion >= 4)
                {
                    /*this.ZOOM_SCALE = */
                    reader.ReadUShort(); // 100 
                    /*this.ZOOM_OFFSET_X = */
                    reader.ReadShort();
                    /*this.ZOOM_OFFSET_Y = */
                    reader.ReadShort();
                }

                uselowpassfilter = reader.ReadByte() != 0;
                usereverb = reader.ReadByte() != 0;
                presetid = -1;
                if (usereverb)
                {
                    presetid = reader.ReadInt();
                }

                // fixtures background
                int count = reader.ReadInt();
                for (int i = 0; i < count; i++)
                {
                    // new fixture
                    reader.ReadInt();
                    reader.ReadShort();
                    reader.ReadShort();
                    reader.ReadShort();
                    reader.ReadShort();
                    reader.ReadShort();
                    reader.ReadByte();
                    reader.ReadByte();
                    reader.ReadByte();
                    reader.ReadByte();
                }

                // fixtures foreground
                count = reader.ReadInt();
                for (int i = 0; i < count; i++)
                {
                    // new fixture
                    reader.ReadInt();
                    reader.ReadShort();
                    reader.ReadShort();
                    reader.ReadShort();
                    reader.ReadShort();
                    reader.ReadShort();
                    reader.ReadByte();
                    reader.ReadByte();
                    reader.ReadByte();
                    reader.ReadByte();
                }

                reader.ReadInt();

                reader.ReadInt(); // ground

                // layers
                count = reader.ReadByte();
                for (int i = 0; i < count; i++)
                {
                    // new layer
                    reader.ReadInt(); // id
                    short cellscount = reader.ReadShort(); // count
                    for (int l = 0; l < cellscount; l++)
                    {
                        reader.ReadShort(); //id
                        short elemcount = reader.ReadShort(); // count
                        for (int k = 0; k < elemcount; k++)
                        {
                            switch (reader.ReadByte())
                            {
                                case 2: // GRAPICAL
                                    reader.ReadShort();
                                    reader.ReadByte();
                                    reader.ReadByte();
                                    reader.ReadByte();
                                    reader.ReadInt();
                                    reader.ReadByte();
                                    reader.ReadByte();
                                    break;
                                case 33: // SOUND
                                    reader.ReadInt();
                                    reader.ReadShort();
                                    reader.ReadInt();
                                    reader.ReadInt();
                                    reader.ReadShort();
                                    reader.ReadShort();
                                    break;
                            }
                        }
                    }
                }

                // cells data
                floor = new int[cellsCount];
                losmov = new int[cellsCount];
                speed = new int[cellsCount];
                mapchangedata = new int[cellsCount];
                for (int i = 0; i < cellsCount; i++)
                {
                    floor[i] = reader.ReadByte();
                    if (floor[i]*10 == -1280)
                        continue;

                    losmov[i] = reader.ReadByte();
                    speed[i] = reader.ReadByte();
                    mapchangedata[i] = reader.ReadByte();
                }


                reader.Dispose();
            }

            // STEP 2 : Write data to a new ripped file
            FileStream ripStream = File.Open(dest, FileMode.Create);

            using (var writer = new BigEndianWriter(ripStream))
            {
                writer.WriteInt(mapversion);
                writer.WriteInt(mapid);
                writer.WriteInt(relativeid);

                writer.WriteInt(maptype);

                writer.WriteInt(subareaid);

                writer.WriteInt(topid);
                writer.WriteInt(bottomid);
                writer.WriteInt(leftid);
                writer.WriteInt(rightid);

                writer.WriteInt(shadownbonusonentities);

                writer.WriteBoolean(uselowpassfilter);

                writer.WriteBoolean(usereverb);
                if (usereverb)
                {
                    writer.WriteInt(presetid);
                }

                for (int i = 0; i < cellsCount; i++)
                {
                    writer.WriteByte((byte) floor[i]);
                    if (floor[i]*10 == -1280)
                        continue;
                    writer.WriteByte((byte) losmov[i]);
                    writer.WriteByte((byte) speed[i]);
                    writer.WriteByte((byte) mapchangedata[i]);
                }

                writer.Dispose();
            }
        }
    }
}