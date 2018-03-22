

// Generated on 02/19/2017 13:42:16
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("TypeAction", "com.ankamagames.dofus.datacenter.misc")]
    [Serializable]
    public class TypeAction : IDataObject, IIndexedData
    {
        public const String MODULE = "TypeActions";
        public int id;
        public String elementName;
        public int elementId;
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
        public String ElementName
        {
            get { return this.elementName; }
            set { this.elementName = value; }
        }
        [D2OIgnore]
        public int ElementId
        {
            get { return this.elementId; }
            set { this.elementId = value; }
        }
    }
}