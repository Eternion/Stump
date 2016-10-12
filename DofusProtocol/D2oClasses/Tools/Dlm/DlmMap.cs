#region License GNU GPL

// DlmMap.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
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
using System.Drawing;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.D2oClasses.Tools.Dlm
{
    public class DlmMap : IDataObject
    {
        public const uint Width = 14;
        public const uint Height = 20;

        public const int CellCount = 560;
        private static readonly Point[] s_orthogonalGridReference = new Point[CellCount];
        private static bool m_initialized;


        public byte Version
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public bool Encrypted
        {
            get;
            set;
        }

        public byte EncryptionVersion
        {
            get;
            set;
        }

        public uint RelativeId
        {
            get;
            set;
        }

        public byte MapType
        {
            get;
            set;
        }

        public int SubAreaId
        {
            get;
            set;
        }

        public int BottomNeighbourId
        {
            get;
            set;
        }

        public int LeftNeighbourId
        {
            get;
            set;
        }

        public int RightNeighbourId
        {
            get;
            set;
        }

        public int ShadowBonusOnEntities
        {
            get;
            set;
        }

        public Color BackgroundColor
        {
            get;
            set;
        }

        public Color GridColor
        {
            get;
            set;
        }

        public ushort ZoomScale
        {
            get;
            set;
        }

        public short ZoomOffsetX
        {
            get;
            set;
        }

        public short ZoomOffsetY
        {
            get;
            set;
        }

        public bool UseLowPassFilter
        {
            get;
            set;
        }

        public bool UseReverb
        {
            get;
            set;
        }

        public int PresetId
        {
            get;
            set;
        }

        public DlmFixture[] BackgroudFixtures
        {
            get;
            set;
        }

        public int TopNeighbourId
        {
            get;
            set;
        }

        public DlmFixture[] ForegroundFixtures
        {
            get;
            set;
        }

        public DlmCellData[] Cells
        {
            get;
            set;
        }

        public int GroundCRC
        {
            get;
            set;
        }

        public DlmLayer[] Layers
        {
            get;
            set;
        }

        public bool UsingNewMovementSystem
        {
            get;
            set;
        }

        public static DlmMap ReadFromStream(IDataReader givenReader, DlmReader dlmReader)
        {
            DlmMap map = null;

            try
            {
                var reader = givenReader;
                map = new DlmMap
                {
                    Version = reader.ReadByte(),
                    Id = reader.ReadInt()
                };

                if (map.Version > DlmReader.VERSION)
                    throw new Exception(string.Format("Reader outdated for this map (old version:{0} new version:{1})",
                        DlmReader.VERSION, map.Version));

                if (map.Version >= 7)
                {
                    map.Encrypted = reader.ReadBoolean();
                    map.EncryptionVersion = reader.ReadByte();

                    var len = reader.ReadInt();

                    if (map.Encrypted)
                    {
                        var key = dlmReader.DecryptionKey;

                        if (key == null && dlmReader.DecryptionKeyProvider != null)
                            key = dlmReader.DecryptionKeyProvider(map.Id);

                        if (key == null)
                        {
                            throw new InvalidOperationException(string.Format("Cannot decrypt the map {0} without decryption key", map.Id));
                        }

                        var data = reader.ReadBytes(len);
                        var encodedKey = Encoding.Default.GetBytes(key);

                        if (key.Length > 0)
                        {
                            for (var i = 0; i < data.Length; i++)
                            {
                                data[i] = (byte) (data[i] ^ encodedKey[i%key.Length]);
                            }

                            reader = new FastBigEndianReader(data);
                        }
                    }
                }

                map.RelativeId = reader.ReadUInt();
                map.MapType = reader.ReadByte();

                // temp, just to know if the result is coherent
                if (map.MapType < 0 || map.MapType > 1)
                    throw new Exception("Invalid decryption key");

                map.SubAreaId = reader.ReadInt();
                map.TopNeighbourId = reader.ReadInt();
                map.BottomNeighbourId = reader.ReadInt();
                map.LeftNeighbourId = reader.ReadInt();
                map.RightNeighbourId = reader.ReadInt();
                map.ShadowBonusOnEntities = reader.ReadInt();

                if (map.Version >= 9)
                {
                    map.BackgroundColor = Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                    map.GridColor = Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                }

                else if (map.Version >= 3)
                {
                    map.BackgroundColor = Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                }

                if (map.Version >= 4)
                {
                    map.ZoomScale = reader.ReadUShort();
                    map.ZoomOffsetX = reader.ReadShort();
                    map.ZoomOffsetY = reader.ReadShort();
                }

                map.UseLowPassFilter = reader.ReadByte() == 1;
                map.UseReverb = reader.ReadByte() == 1;

                if (map.UseReverb)
                {
                    map.PresetId = reader.ReadInt();
                }
                {
                    map.PresetId = -1;
                }

                map.BackgroudFixtures = new DlmFixture[reader.ReadByte()];
                for (var i = 0; i < map.BackgroudFixtures.Length; i++)
                {
                    map.BackgroudFixtures[i] = DlmFixture.ReadFromStream(map, reader);
                }

                map.ForegroundFixtures = new DlmFixture[reader.ReadByte()];
                for (var i = 0; i < map.ForegroundFixtures.Length; i++)
                {
                    map.ForegroundFixtures[i] = DlmFixture.ReadFromStream(map, reader);
                }

                reader.ReadInt();
                map.GroundCRC = reader.ReadInt();

                map.Layers = new DlmLayer[reader.ReadByte()];
                for (var i = 0; i < map.Layers.Length; i++)
                {
                    map.Layers[i] = DlmLayer.ReadFromStream(map, reader);
                }

                map.Cells = new DlmCellData[CellCount];
                int? lastMoveZone = null;
                for (short i = 0; i < map.Cells.Length; i++)
                {
                    map.Cells[i] = DlmCellData.ReadFromStream(i, map.Version, reader);
                    if (!lastMoveZone.HasValue)
                        lastMoveZone = map.Cells[i].MoveZone;
                    else if (lastMoveZone != map.Cells[i].MoveZone) // if a cell is different the new system is used
                        map.UsingNewMovementSystem = true;
                }

                return map;
            }
            catch (Exception ex)
            {
                throw new BadEncodedMapException(ex, map);
            }
        }
    }

    public class BadEncodedMapException : Exception
    {
        public DlmMap Map {get;}

        public BadEncodedMapException(Exception exception, DlmMap map)
            : base("Bad encoded map", exception)
        {
            Map = map;
        }
    }
}