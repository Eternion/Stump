
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Interactives")]
    [Serializable]
    public class Interactive : IDataObject, IIndexedData
    {
        private const String MODULE = "Interactives";
        public int id;
        public uint nameId;
        public int actionId;
        public Boolean displayTooltip;

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

        public int ActionId
        {
            get { return actionId; }
            set { actionId = value; }
        }

        public Boolean DisplayTooltip
        {
            get { return displayTooltip; }
            set { displayTooltip = value; }
        }

    }
}