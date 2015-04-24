

// Generated on 04/24/2015 03:38:25
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("BreedRoleByBreed", "com.ankamagames.dofus.datacenter.breeds")]
    [Serializable]
    public class BreedRoleByBreed : IDataObject, IIndexedData
    {
        public const String MODULE = "BreedRoleByBreeds";
        public int breedId;
        public int roleId;
        [I18NField]
        public uint descriptionId;
        public int value;
        public int order;
        int IIndexedData.Id
        {
            get { return (int)breedId; }
        }
        [D2OIgnore]
        public int BreedId
        {
            get { return this.breedId; }
            set { this.breedId = value; }
        }
        [D2OIgnore]
        public int RoleId
        {
            get { return this.roleId; }
            set { this.roleId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        [D2OIgnore]
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
    }
}