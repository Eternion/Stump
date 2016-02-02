

// Generated on 02/02/2016 14:15:00
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Document", "com.ankamagames.dofus.datacenter.documents")]
    [Serializable]
    public class Document : IDataObject, IIndexedData
    {
        private const String MODULE = "Documents";
        public int id;
        public uint typeId;
        public Boolean showTitle;
        public Boolean showBackgroundImage;
        [I18NField]
        public uint titleId;
        [I18NField]
        public uint authorId;
        [I18NField]
        public uint subTitleId;
        [I18NField]
        public uint contentId;
        public String contentCSS;
        public String clientProperties;
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
        public uint TypeId
        {
            get { return this.typeId; }
            set { this.typeId = value; }
        }
        [D2OIgnore]
        public Boolean ShowTitle
        {
            get { return this.showTitle; }
            set { this.showTitle = value; }
        }
        [D2OIgnore]
        public Boolean ShowBackgroundImage
        {
            get { return this.showBackgroundImage; }
            set { this.showBackgroundImage = value; }
        }
        [D2OIgnore]
        public uint TitleId
        {
            get { return this.titleId; }
            set { this.titleId = value; }
        }
        [D2OIgnore]
        public uint AuthorId
        {
            get { return this.authorId; }
            set { this.authorId = value; }
        }
        [D2OIgnore]
        public uint SubTitleId
        {
            get { return this.subTitleId; }
            set { this.subTitleId = value; }
        }
        [D2OIgnore]
        public uint ContentId
        {
            get { return this.contentId; }
            set { this.contentId = value; }
        }
        [D2OIgnore]
        public String ContentCSS
        {
            get { return this.contentCSS; }
            set { this.contentCSS = value; }
        }
        [D2OIgnore]
        public String ClientProperties
        {
            get { return this.clientProperties; }
            set { this.clientProperties = value; }
        }
    }
}