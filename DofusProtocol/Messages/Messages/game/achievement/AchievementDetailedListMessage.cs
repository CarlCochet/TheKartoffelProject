

// Generated on 02/17/2017 01:57:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementDetailedListMessage : Message
    {
        public const uint Id = 6358;
        public override uint MessageId => Id;


        public IEnumerable<Types.Achievement> startedAchievements;
        public IEnumerable<Types.Achievement> finishedAchievements;

        public AchievementDetailedListMessage()
        {
        }

        public AchievementDetailedListMessage(IEnumerable<Types.Achievement> startedAchievements, IEnumerable<Types.Achievement> finishedAchievements)
        {
            this.startedAchievements = startedAchievements;
            this.finishedAchievements = finishedAchievements;
        }

        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort((short)startedAchievements.Count());
            foreach (var entry in startedAchievements)
            {
                entry.Serialize(writer);
            }
            writer.WriteShort((short)finishedAchievements.Count());
            foreach (var entry in finishedAchievements)
            {
                entry.Serialize(writer);
            }
        }

        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            startedAchievements = new Types.Achievement[limit];
            for (int i = 0; i < limit; i++)
            {
                (startedAchievements as Types.Achievement[])[i] = new Types.Achievement();
                (startedAchievements as Types.Achievement[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            finishedAchievements = new Types.Achievement[limit];
            for (int i = 0; i < limit; i++)
            {
                (finishedAchievements as Types.Achievement[])[i] = new Types.Achievement();
                (finishedAchievements as Types.Achievement[])[i].Deserialize(reader);
            }
        }
    }

}