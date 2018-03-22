

// Generated on 02/10/2017 09:16:42
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using System.Runtime.InteropServices.ComTypes;

namespace Stump.DofusProtocol.Classes
{
    [D2OClass("CompanionCharacteristics", "com.ankamagames.dofus.datacenter.monsters.CompanionCharacteristic")]
    public class CompanionCharacteristic : IDataObject
    {
        public const String MODULE = "CompanionCharacteristics";
        public int id;
        public int caracId;
        public int companionId;
        public int order;
        public List<List<double>> statPerLevelRange;
    
        [D2OIgnore]
        public int Id
        {
        get { return id; }
        set { id = value ; }
        }
        [D2OIgnore]
        public int CaracId
        {
        get { return caracId; }
        set { caracId = value ; }
        }
        [D2OIgnore]
        public int CompanionId
        {
        get { return companionId; }
        set { companionId = value ; }
        }
        [D2OIgnore]
        public int Order
        {
        get { return order; }
        set { order = value ; }
        }
        [D2OIgnore]
        public List<List<double>> StatPerLevelRange
        {
        get { return statPerLevelRange; }
        set { statPerLevelRange = value ; }
        }

        public void GetData(ref FORMATETC format, out STGMEDIUM medium)
        {
            throw new NotImplementedException();
        }

        public void GetDataHere(ref FORMATETC format, ref STGMEDIUM medium)
        {
            throw new NotImplementedException();
        }

        public int QueryGetData(ref FORMATETC format)
        {
            throw new NotImplementedException();
        }

        public int GetCanonicalFormatEtc(ref FORMATETC formatIn, out FORMATETC formatOut)
        {
            throw new NotImplementedException();
        }

        public void SetData(ref FORMATETC formatIn, ref STGMEDIUM medium, bool release)
        {
            throw new NotImplementedException();
        }

        public IEnumFORMATETC EnumFormatEtc(DATADIR direction)
        {
            throw new NotImplementedException();
        }

        public int DAdvise(ref FORMATETC pFormatetc, ADVF advf, IAdviseSink adviseSink, out int connection)
        {
            throw new NotImplementedException();
        }

        public void DUnadvise(int connection)
        {
            throw new NotImplementedException();
        }

        public int EnumDAdvise(out IEnumSTATDATA enumAdvise)
        {
            throw new NotImplementedException();
        }
    }
}