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
using Stump.BaseCore.Framework.IO;

namespace Stump.DofusProtocol.Messages
{
    public abstract class Message
    {
        private const byte BIT_RIGHT_SHIFT_LEN_PACKET_ID = 2;
        private const byte BIT_MASK = 3;

        public abstract uint getMessageId();
        public abstract void reset();

        public abstract void pack(BigEndianWriter writer);
        public abstract void unpack(BigEndianReader reader, uint arg2);

        //public abstract void serialize(BigEndianWriter writer);
        //public abstract void deserialize(BigEndianReader reader);

        internal static void WritePacket(BigEndianWriter writer, uint id)
        {
            byte[] packet = writer.Data;

            writer.Clear();

            int typeLen = ComputeTypeLen(packet.Length);

            int header = SubComputeStaticHeader((int) id, typeLen);

            writer.WriteShort((short) header);

            switch (typeLen)
            {
                case 0:
                {
                    break;
                }
                case 1:
                {
                    writer.WriteByte((byte) packet.Length);
                    break;
                }
                case 2:
                {
                    writer.WriteShort((short) packet.Length);
                    break;
                }
                case 3:
                {
                    var _loc_5 = (byte) (packet.Length >> 16 & 255);
                    var _loc_6 = (short) (packet.Length & 65535);
                    writer.WriteByte(_loc_5);
                    writer.WriteShort(_loc_6);
                    break;
                }
                default:
                {
                    throw new Exception("Packet's length can't be encoded on 4 or more bytes");
                }
            }

            writer.WriteBytes(packet);
        }


        private static int ComputeTypeLen(int param1)
        {
            if (param1 > 65535)
            {
                return 3;
            }
            if (param1 > 255)
            {
                return 2;
            }
            if (param1 > 0)
            {
                return 1;
            }
            return 0;
        }

        private static int SubComputeStaticHeader(int id, int typeLen)
        {
            return id << BIT_RIGHT_SHIFT_LEN_PACKET_ID | typeLen;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}