
// Generated on 03/25/2013 19:24:32
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("InfoMessages")]
    [Serializable]
    public class InfoMessage : IDataObject, IIndexedData
    {
        private const String MODULE = "InfoMessages";
        public uint typeId;
        public uint messageId;
        public uint textId;

        int IIndexedData.Id
        {
            get { return (int)typeId; }
        }

        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public uint MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        public uint TextId
        {
            get { return textId; }
            set { textId = value; }
        }

    }
}