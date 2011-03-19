using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Stump.BaseCore.Framework.IO;

namespace Stump.Server.DataProvider.Data.D2oTool
{
    public class D2OFieldDefinition
    {
        public D2OFieldDefinition(string name, int typeId, FieldInfo fieldInfo, long offset, params int[] vectorsTypesId)
        {
            Name = name;
            TypeId = typeId;
            FieldInfo = fieldInfo;
            Offset = offset;

            VectorTypesId = vectorsTypesId;
        }

        public string Name
        {
            get;
            set;
        }

        public int TypeId
        {
            get;
            set;
        }

        public int[] VectorTypesId
        {
            get;
            set;
        }

        internal long Offset
        {
            get;
            set;
        }

        public Type FieldType
        {
            get
            {
                return FieldInfo.FieldType;
            }
        }

        public FieldInfo FieldInfo
        {
            get;
            set;
        }
    }
}