 


// Generated on 10/06/2013 14:22:01
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("SpellBombs")]
    [D2OClass("SpellBomb")]
    public class SpellBombRecord : ID2ORecord
    {
        private const String MODULE = "SpellBombs";
        public int id;
        public int chainReactionSpellId;
        public int explodSpellId;
        public int wallId;
        public int instantSpellId;
        public int comboCoeff;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int ChainReactionSpellId
        {
            get { return chainReactionSpellId; }
            set { chainReactionSpellId = value; }
        }

        public int ExplodSpellId
        {
            get { return explodSpellId; }
            set { explodSpellId = value; }
        }

        public int WallId
        {
            get { return wallId; }
            set { wallId = value; }
        }

        public int InstantSpellId
        {
            get { return instantSpellId; }
            set { instantSpellId = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new SpellBomb();
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