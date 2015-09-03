 


// Generated on 09/01/2015 10:48:49
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
    [TableName("Playlists")]
    [D2OClass("Playlist", "com.ankamagames.dofus.datacenter.playlists")]
    public class PlaylistRecord : ID2ORecord, ISaveIntercepter
    {
        public const int AMBIENT_TYPE_ROLEPLAY = 1;
        public const int AMBIENT_TYPE_AMBIENT = 2;
        public const int AMBIENT_TYPE_FIGHT = 3;
        public const int AMBIENT_TYPE_BOSS = 4;
        public const String MODULE = "Playlists";
        public int id;
        public int silenceDuration;
        public int iteration;
        public int type;
        public List<PlaylistSound> sounds;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public int SilenceDuration
        {
            get { return silenceDuration; }
            set { silenceDuration = value; }
        }

        [D2OIgnore]
        public int Iteration
        {
            get { return iteration; }
            set { iteration = value; }
        }

        [D2OIgnore]
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<PlaylistSound> Sounds
        {
            get { return sounds; }
            set
            {
                sounds = value;
                m_soundsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_soundsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SoundsBin
        {
            get { return m_soundsBin; }
            set
            {
                m_soundsBin = value;
                sounds = value == null ? null : value.ToObject<List<PlaylistSound>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Playlist)obj;
            
            Id = castedObj.id;
            SilenceDuration = castedObj.silenceDuration;
            Iteration = castedObj.iteration;
            Type = castedObj.type;
            Sounds = castedObj.sounds;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Playlist)parent : new Playlist();
            obj.id = Id;
            obj.silenceDuration = SilenceDuration;
            obj.iteration = Iteration;
            obj.type = Type;
            obj.sounds = Sounds;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_soundsBin = sounds == null ? null : sounds.ToBinary();
        
        }
    }
}