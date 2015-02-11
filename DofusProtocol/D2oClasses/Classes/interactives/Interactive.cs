

// Generated on 02/11/2015 10:21:31
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Interactive", "com.ankamagames.dofus.datacenter.interactives")]
    [Serializable]
    public class Interactive : IDataObject, IIndexedData
    {
        public const String MODULE = "Interactives";
        public int id;
        [I18NField]
        public uint nameId;
        public int actionId;
        public Boolean displayTooltip;
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
        public int ActionId
        {
            get { return this.actionId; }
            set { this.actionId = value; }
        }
        [D2OIgnore]
        public Boolean DisplayTooltip
        {
            get { return this.displayTooltip; }
            set { this.displayTooltip = value; }
        }
    }
}