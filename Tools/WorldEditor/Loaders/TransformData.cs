// Generated on 10/28/2013 14:03:20
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
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