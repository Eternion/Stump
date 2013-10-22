 


// Generated on 10/19/2013 17:17:45
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
    [TableName("SpellBombs")]
    [D2OClass("SpellBomb", "com.ankamagames.dofus.datacenter.spells")]
    public class SpellBombRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "SpellBombs";
        public int id;
        public int chainReactionSpellId;
        public int explodSpellId;
        public int wallId;
        public int instantSpellId;
        public int comboCoeff;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public int ChainReactionSpellId
        {
            get { return chainReactionSpellId; }
            set { chainReactionSpellId = value; }
        }

        [D2OIgnore]
        public int ExplodSpellId
        {
            get { return explodSpellId; }
            set { explodSpellId = value; }
        }

        [D2OIgnore]
        public int WallId
        {
            get { return wallId; }
            set { wallId = value; }
        }

        [D2OIgnore]
        public int InstantSpellId
        {
            get { return instantSpellId; }
            set { instantSpellId = value; }
        }

        [D2OIgnore]
        public int ComboCoeff
        {
            get { return comboCoeff; }
            set { comboCoeff = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SpellBomb)obj;
            
            Id = castedObj.id;
            ChainReactionSpellId = castedObj.chainReactionSpellId;
            ExplodSpellId = castedObj.explodSpellId;
            WallId = castedObj.wallId;
            InstantSpellId = castedObj.instantSpellId;
            ComboCoeff = castedObj.comboCoeff;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (SpellBomb)parent : new SpellBomb();
            obj.id = Id;
            obj.chainReactionSpellId = ChainReactionSpellId;
            obj.explodSpellId = ExplodSpellId;
            obj.wallId = WallId;
            obj.instantSpellId = InstantSpellId;
            obj.comboCoeff = ComboCoeff;
            return obj;
        
        }
    }
}