

// Generated on 02/10/2017 09:16:42
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.DofusProtocol.D2oClasses;

namespace Stump.DofusProtocol.Classes
{
    [D2OClass("CompanionSpells", "com.ankamagames.dofus.datacenter.monsters.CompanionSpell")]
    public class CompanionSpell : IDataObject
    {
        public const String MODULE = "CompanionSpells";
        public int id;
        public int spellId;
        public int companionId;
        public String gradeByLevel;

        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public int SpellId
        {
            get { return spellId; }
            set { spellId = value; }
        }
        [D2OIgnore]
        public int CompanionId
        {
            get { return companionId; }
            set { companionId = value; }
        }
        [D2OIgnore]
        public String GradeByLevel
        {
            get { return gradeByLevel; }
            set { gradeByLevel = value; }
        }

    }
}