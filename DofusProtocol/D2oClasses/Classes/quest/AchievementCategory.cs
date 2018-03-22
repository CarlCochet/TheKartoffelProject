

// Generated on 02/10/2017 09:16:42
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.DofusProtocol.D2oClasses;

namespace Stump.DofusProtocol.Classes
{
    [D2OClass("AchievementCategories", "com.ankamagames.dofus.datacenter.quest.AchievementCategory")]
    public class AchievementCategory : IDataObject
    {
        public const String MODULE = "AchievementCategories";
        public uint id;
        public uint nameId;
        public uint parentId;
        public String icon;
        public uint order;
        public String color;
        public List<uint> achievementIds;
    
        [D2OIgnore]
        public uint Id
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
        public uint ParentId
        {
        get { return parentId; }
        set { parentId = value ; }
        }
        [D2OIgnore]
        public String Icon
        {
        get { return icon; }
        set { icon = value ; }
        }
        [D2OIgnore]
        public uint Order
        {
        get { return order; }
        set { order = value ; }
        }
        [D2OIgnore]
        public String Color
        {
        get { return color; }
        set { color = value ; }
        }
        [D2OIgnore]
        public List<uint> AchievementIds
        {
        get { return achievementIds; }
        set { achievementIds = value ; }
        }
    
    }
}