 


// Generated on 10/06/2013 14:21:57
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("AlmanaxCalendars")]
    [D2OClass("AlmanaxCalendar")]
    public class AlmanaxCalendarRecord : ID2ORecord
    {
        private const String MODULE = "AlmanaxCalendars";
        public int id;
        public uint nameId;
        public uint descId;
        public int npcId;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint DescId
        {
            get { return descId; }
            set { descId = value; }
        }

        public int NpcId
        {
            get { return npcId; }
            set { npcId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AlmanaxCalendar)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            DescId = castedObj.descId;
            NpcId = castedObj.npcId;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new AlmanaxCalendar();
            obj.id = Id;
            obj.nameId = NameId;
            obj.descId = DescId;
            obj.npcId = NpcId;
            return obj;
        
        }
    }
}