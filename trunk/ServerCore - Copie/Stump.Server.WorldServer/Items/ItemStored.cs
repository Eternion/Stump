
using System;
using System.Runtime.Serialization;
using Stump.Server.WorldServer.Effects;

namespace Stump.Server.WorldServer.Items
{
    [Serializable]
    public class ItemStored
    {
        public int[] Durability = new int[2];
        public EffectBase[] Effects;
        public int Id;
    }

    internal sealed class ItemStoredBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type typeToDeserialize = Type.GetType(typeName);
            return typeToDeserialize;
        }
    }
}