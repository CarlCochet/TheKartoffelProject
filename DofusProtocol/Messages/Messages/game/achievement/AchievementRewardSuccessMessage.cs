

// Generated on 02/17/2017 01:57:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementRewardSuccessMessage : Message
    {
        public const uint Id = 6376;
        public override uint MessageId => Id;


        public short achievementId;

        public AchievementRewardSuccessMessage()
        {
        }

        public AchievementRewardSuccessMessage(short achievementId)
        {
            this.achievementId = achievementId;
        }

        //public override void Serialize(ICustomDataOutput writer)
        //{
        //    writer.WriteShort(achievementId);
        //}

        //public override void Deserialize(ICustomDataInput reader)
        //{
        //    achievementId = reader.ReadShort();
        //}

        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(achievementId);
        }

        public override void Deserialize(IDataReader reader)
        {
            achievementId = reader.ReadShort();
        }
    }

}