

// Generated on 02/19/2015 12:10:49
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Npc", "com.ankamagames.dofus.datacenter.npcs")]
    [Serializable]
    public class Npc : IDataObject, IIndexedData
    {
        public const String MODULE = "Npcs";
        public int id;
        [I18NField]
        public uint nameId;
        public List<List<int>> dialogMessages;
        public List<List<int>> dialogReplies;
        public List<uint> actions;
        public uint gender;
        public String look;
        public int tokenShop;
        public List<AnimFunNpcData> animFunList;
        public Boolean fastAnimsFun;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public List<List<int>> DialogMessages
        {
            get { return this.dialogMessages; }
            set { this.dialogMessages = value; }
        }
        [D2OIgnore]
        public List<List<int>> DialogReplies
        {
            get { return this.dialogReplies; }
            set { this.dialogReplies = value; }
        }
        [D2OIgnore]
        public List<uint> Actions
        {
            get { return this.actions; }
            set { this.actions = value; }
        }
        [D2OIgnore]
        public uint Gender
        {
            get { return this.gender; }
            set { this.gender = value; }
        }
        [D2OIgnore]
        public String Look
        {
            get { return this.look; }
            set { this.look = value; }
        }
        [D2OIgnore]
        public int TokenShop
        {
            get { return this.tokenShop; }
            set { this.tokenShop = value; }
        }
        [D2OIgnore]
        public List<AnimFunNpcData> AnimFunList
        {
            get { return this.animFunList; }
            set { this.animFunList = value; }
        }
        [D2OIgnore]
        public Boolean FastAnimsFun
        {
            get { return this.fastAnimsFun; }
            set { this.fastAnimsFun = value; }
        }
    }
}