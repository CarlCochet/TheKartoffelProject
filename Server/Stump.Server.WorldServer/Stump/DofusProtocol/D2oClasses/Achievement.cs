// Generated on 02/10/2017 09:16:42
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using System.Runtime.InteropServices.ComTypes;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Achievements", "com.ankamagames.dofus.datacenter.quest.Achievement")]
    public class Achievement : IDataObject
    {
        public const String MODULE = "Achievements";
        public uint id;
        public uint nameId;
        public uint categoryId;
        public uint descriptionId;
        public int iconId;
        public uint points;
        public uint level;
        public uint order;
        public double kamasRatio;
        public double experienceRatio;
        public Boolean kamasScaleWithPlayerLevel;
        public List<int> objectiveIds;
        public List<int> rewardIds;

        [D2OIgnore]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public uint CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }
        [D2OIgnore]
        public uint Points
        {
            get { return points; }
            set { points = value; }
        }
        [D2OIgnore]
        public uint Level
        {
            get { return level; }
            set { level = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }
        [D2OIgnore]
        public double KamasRatio
        {
            get { return kamasRatio; }
            set { kamasRatio = value; }
        }
        [D2OIgnore]
        public double ExperienceRatio
        {
            get { return experienceRatio; }
            set { experienceRatio = value; }
        }
        [D2OIgnore]
        public Boolean KamasScaleWithPlayerLevel
        {
            get { return kamasScaleWithPlayerLevel; }
            set { kamasScaleWithPlayerLevel = value; }
        }
        [D2OIgnore]
        public List<int> ObjectiveIds
        {
            get { return objectiveIds; }
            set { objectiveIds = value; }
        }
        [D2OIgnore]
        public List<int> RewardIds
        {
            get { return rewardIds; }
            set { rewardIds = value; }
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