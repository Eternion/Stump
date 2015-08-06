#region License GNU GPL
// RawDataMessageFixed.cs
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
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages.Custom
{
    [OverrideMessage]
    public class RawDataMessageFixed : Message
    {
        public const uint Id = 6253;
        public byte[] content;
        public override uint MessageId
        {
            get { return Id; }
        }

        public RawDataMessageFixed()
        {
            
        }

        public RawDataMessageFixed(byte[] _content)
        {
            content = _content;
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(content.Length);
            writer.WriteBytes(content);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var len = reader.ReadVarInt();
            content = reader.ReadBytes(len);
        }
    }
    
}