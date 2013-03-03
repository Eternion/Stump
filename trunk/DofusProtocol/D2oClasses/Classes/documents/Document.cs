
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Documents")]
    [Serializable]
    public class Document : IDataObject, IIndexedData
    {
        private const String MODULE = "Documents";
        public int id;
        public uint typeId;
        public uint titleId;
        public uint authorId;
        public uint subTitleId;
        public uint contentId;
        public String contentCSS;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public uint TitleId
        {
            get { return titleId; }
            set { titleId = value; }
        }

        public uint AuthorId
        {
            get { return authorId; }
            set { authorId = value; }
        }

        public uint SubTitleId
        {
            get { return subTitleId; }
            set { subTitleId = value; }
        }

        public uint ContentId
        {
            get { return contentId; }
            set { contentId = value; }
        }

        public String ContentCSS
        {
            get { return contentCSS; }
            set { contentCSS = value; }
        }

    }
}