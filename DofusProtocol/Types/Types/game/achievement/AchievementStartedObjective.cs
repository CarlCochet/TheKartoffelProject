

// Generated on 02/17/2017 01:52:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AchievementStartedObjective : AchievementObjective
    {
        public const short Id = 402;
        public override short TypeId
        {
            get { return Id; }
        }

        public ushort value;

        public AchievementStartedObjective()
        {
        }

        public AchievementStartedObjective(uint id, ushort maxValue, ushort value)
         : base(id, maxValue)
        {
            this.value = value;
        }

        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarShort((short)value);
        }

        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadVarUhShort();
        }

    }

}