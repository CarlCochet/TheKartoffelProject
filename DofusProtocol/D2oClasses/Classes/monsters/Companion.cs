

// Generated on 02/10/2017 09:16:42
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using System.Runtime.InteropServices.ComTypes;

namespace Stump.DofusProtocol.Classes
{
    [D2OClass("Companions", "com.ankamagames.dofus.datacenter.monsters.Companion")]
    public class Companion : IDataObject
    {
        public const String MODULE = "Companions";
        public int id;
        public uint nameId;
        public String look;
        public Boolean webDisplay;
        public uint descriptionId;
        public uint startingSpellLevelId;
        public uint assetId;
        public List<uint> characteristics;
        public List<uint> spells;
        public int creatureBoneId;
    
        [D2OIgnore]
        public int Id
        {
        get { return id; }
        set { id = value ; }
        }
        [D2OIgnore]
        public uint NameId
        {
        get { return nameId; }
        set { nameId = value ; }
        }
        [D2OIgnore]
        public String Look
        {
        get { return look; }
        set { look = value ; }
        }
        [D2OIgnore]
        public Boolean WebDisplay
        {
        get { return webDisplay; }
        set { webDisplay = value ; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
        get { return descriptionId; }
        set { descriptionId = value ; }
        }
        [D2OIgnore]
        public uint StartingSpellLevelId
        {
        get { return startingSpellLevelId; }
        set { startingSpellLevelId = value ; }
        }
        [D2OIgnore]
        public uint AssetId
        {
        get { return assetId; }
        set { assetId = value ; }
        }
        [D2OIgnore]
        public List<uint> Characteristics
        {
        get { return characteristics; }
        set { characteristics = value ; }
        }
        [D2OIgnore]
        public List<uint> Spells
        {
        get { return spells; }
        set { spells = value ; }
        }
        [D2OIgnore]
        public int CreatureBoneId
        {
        get { return creatureBoneId; }
        set { creatureBoneId = value ; }
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