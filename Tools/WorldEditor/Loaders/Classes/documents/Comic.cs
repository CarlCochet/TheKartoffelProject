

// Generated on 02/19/2017 13:42:11
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("Comic", "com.ankamagames.dofus.datacenter.documents")]
    [Serializable]
    public class Comic : IDataObject, IIndexedData
    {
        private const String MODULE = "Comics";
        public int id;
        public String remoteId;
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
        public String RemoteId
        {
            get { return this.remoteId; }
            set { this.remoteId = value; }
        }
    }
}