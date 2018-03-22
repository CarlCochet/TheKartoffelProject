

// Generated on 02/19/2017 13:42:08
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("AbuseReasons", "com.ankamagames.dofus.datacenter.abuse")]
    [Serializable]
    public class AbuseReasons : IDataObject, IIndexedData
    {
        public const String MODULE = "AbuseReasons";
        public uint _abuseReasonId;
        public uint _mask;
        [I18NField]
        public int _reasonTextId;
        int IIndexedData.Id
        {
            get { return (int)_abuseReasonId; }
        }
        [D2OIgnore]
        public uint AbuseReasonId
        {
            get { return this._abuseReasonId; }
            set { this._abuseReasonId = value; }
        }
        [D2OIgnore]
        public uint Mask
        {
            get { return this._mask; }
            set { this._mask = value; }
        }
        [D2OIgnore]
        public int ReasonTextId
        {
            get { return this._reasonTextId; }
            set { this._reasonTextId = value; }
        }
    }
}