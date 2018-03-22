

// Generated on 02/10/2017 09:16:42
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.DofusProtocol.D2oClasses;

namespace Stump.DofusProtocol.Classes
{
    [D2OClass("AchievementObjectives", "com.ankamagames.dofus.datacenter.quest.AchievementObjective")]
    public class AchievementObjective : IDataObject
    {
        public const String MODULE = "AchievementObjectives";
        public uint id;
        public uint achievementId;
        public uint order;
        public uint nameId;
        public String criterion;
    
        [D2OIgnore]
        public uint Id
        {
        get { return id; }
        set { id = value ; }
        }
        [D2OIgnore]
        public uint AchievementId
        {
        get { return achievementId; }
        set { achievementId = value ; }
        }
        [D2OIgnore]
        public uint Order
        {
        get { return order; }
        set { order = value ; }
        }
        [D2OIgnore]
        public uint NameId
        {
        get { return nameId; }
        set { nameId = value ; }
        }
        [D2OIgnore]
        public String Criterion
        {
        get { return criterion; }
        set { criterion = value ; }
        }
    
    }
}