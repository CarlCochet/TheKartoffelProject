

// Generated on 02/17/2017 01:57:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementListMessage : Message
    {
        public const uint Id = 6205;
        public override uint MessageId => Id;


        public IEnumerable<ushort> finishedAchievementsIds;
        public IEnumerable<Types.AchievementRewardable> rewardableAchievements;

        public AchievementListMessage()
        {
        }

        public AchievementListMessage(IEnumerable<ushort> finishedAchievementsIds, IEnumerable<Types.AchievementRewardable> rewardableAchievements)
        {
            this.finishedAchievementsIds = finishedAchievementsIds;
            this.rewardableAchievements = rewardableAchievements;
        }

        //public override void Serialize(ICustomDataOutput writer)
        //{
        //    writer.WriteShort((short)finishedAchievementsIds.Count());
        //    foreach (var entry in finishedAchievementsIds)
        //    {
        //        writer.WriteVarShort((int)entry);
        //    }
        //    writer.WriteShort((short)rewardableAchievements.Count());
        //    foreach (var entry in rewardableAchievements)
        //    {
        //        entry.Serialize(writer);
        //    }
        //}

        //public override void Deserialize(ICustomDataInput reader)
        //{
        //    var limit = reader.ReadUShort();
        //    finishedAchievementsIds = new ushort[limit];
        //    for (int i = 0; i < limit; i++)
        //    {
        //        (finishedAchievementsIds as ushort[])[i] = reader.ReadVarUhShort();
        //    }
        //    limit = reader.ReadUShort();
        //    rewardableAchievements = new Types.AchievementRewardable[limit];
        //    for (int i = 0; i < limit; i++)
        //    {
        //        (rewardableAchievements as Types.AchievementRewardable[])[i] = new Types.AchievementRewardable();
        //        (rewardableAchievements as Types.AchievementRewardable[])[i].Deserialize(reader);
        //    }
        //}

        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort((short)finishedAchievementsIds.Count());
            foreach (var entry in finishedAchievementsIds)
            {
                writer.WriteVarShort((short)entry);
            }
            writer.WriteShort((short)rewardableAchievements.Count());
            foreach (var entry in rewardableAchievements)
            {
                entry.Serialize((ICustomDataOutput)writer);
            }
        }

        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            finishedAchievementsIds = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                (finishedAchievementsIds as ushort[])[i] = reader.ReadVarUhShort();
            }
            limit = reader.ReadUShort();
            rewardableAchievements = new Types.AchievementRewardable[limit];
            for (int i = 0; i < limit; i++)
            {
                (rewardableAchievements as Types.AchievementRewardable[])[i] = new Types.AchievementRewardable();
                (rewardableAchievements as Types.AchievementRewardable[])[i].Deserialize((ICustomDataInput)reader);
            }
        }
    }

}