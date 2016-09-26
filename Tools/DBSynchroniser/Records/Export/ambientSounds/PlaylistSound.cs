 


// Generated on 09/26/2016 01:50:39
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("PlaylistSounds")]
    [D2OClass("PlaylistSound", "com.ankamagames.dofus.datacenter.ambientSounds")]
    public class PlaylistSoundRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "PlaylistSounds";
        public String id;
        public int volume;
        public int channel = 0;

        int ID2ORecord.Id
        {
            get { return int.Parse(id); }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        [NullString]
        public String Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public int Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        [D2OIgnore]
        public int Channel
        {
            get { return channel; }
            set { channel = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (PlaylistSound)obj;
            
            Id = castedObj.id;
            Volume = castedObj.volume;
            Channel = castedObj.channel;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (PlaylistSound)parent : new PlaylistSound();
            obj.id = Id;
            obj.volume = Volume;
            obj.channel = Channel;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}