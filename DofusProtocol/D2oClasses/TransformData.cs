// Generated on 10/28/2013 14:03:20
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("TransformData", "com.ankamagames.tiphon.types")]
    [Serializable]
    public class TransformData : IDataObject
    {
      public String overrideClip;     
      public String originalClip;    
      public int x;    
      public int y;   
      public int scaleX;
      public int scaleY;
      public int rotation;
    }
}