
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Npcs")]
    [Serializable]
    public class Npc : IDataObject, IIndexedData
    {
        private const String MODULE = "Npcs";
        public int id;
        public uint nameId;
        public List<List<int>> dialogMessages;
        public List<List<int>> dialogReplies;
        public List<uint> actions;
        public uint gender;
        public String look;
        public int tokenShop;
        public List<AnimFunNpcData> animFunList;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

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

        public List<List<int>> DialogMessages
        {
            get { return dialogMessages; }
            set { dialogMessages = value; }
        }

        public List<List<int>> DialogReplies
        {
            get { return dialogReplies; }
            set { dialogReplies = value; }
        }

        public List<uint> Actions
        {
            get { return actions; }
            set { actions = value; }
        }

        public uint Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public String Look
        {
            get { return look; }
            set { look = value; }
        }

        public int TokenShop
        {
            get { return tokenShop; }
            set { tokenShop = value; }
        }

        public List<AnimFunNpcData> AnimFunList
        {
            get { return animFunList; }
            set { animFunList = value; }
        }

    }
}