

// Generated on 02/17/2017 01:52:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class Achievement
    {
        public const short Id = 363;
        public virtual short TypeId
        {
            get { return Id; }
        }

        public ushort id;
        public IEnumerable<Types.AchievementObjective> finishedObjective;
        public IEnumerable<Types.AchievementStartedObjective> startedObjectives;

        public Achievement()
        {
        }

        public Achievement(ushort id, IEnumerable<Types.AchievementObjective> finishedObjective, IEnumerable<Types.AchievementStartedObjective> startedObjectives)
        {
            this.id = id;
            this.finishedObjective = finishedObjective;
            this.startedObjectives = startedObjectives;
        }

        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort((short)id);
            writer.WriteShort((short)finishedObjective.Count());
            foreach (var entry in finishedObjective)
            {
                entry.Serialize(writer);
            }
            writer.WriteShort((short)startedObjectives.Count());
            foreach (var entry in startedObjectives)
            {
                entry.Serialize(writer);
            }
        }

        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarUhShort();
            var limit = reader.ReadUShort();
            finishedObjective = new Types.AchievementObjective[limit];
            for (int i = 0; i < limit; i++)
            {
                (finishedObjective as Types.AchievementObjective[])[i] = new Types.AchievementObjective();
                (finishedObjective as Types.AchievementObjective[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            startedObjectives = new Types.AchievementStartedObjective[limit];
            for (int i = 0; i < limit; i++)
            {
                (startedObjectives as Types.AchievementStartedObjective[])[i] = new Types.AchievementStartedObjective();
                (startedObjectives as Types.AchievementStartedObjective[])[i].Deserialize(reader);
            }
        }

    }

}