

// Generated on 02/17/2017 01:52:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AchievementRewardable
    {
        public const short Id = 412;
        public virtual short TypeId
        {
            get { return Id; }
        }

        public ushort id;
        public byte finishedlevel;

        public AchievementRewardable()
        {
        }

        public AchievementRewardable(ushort id, byte finishedlevel)
        {
            this.id = id;
            this.finishedlevel = finishedlevel;
        }

        public virtual void Serialize(ICustomDataOutput writer)
        {
            writer.WriteVarShort((int)id);
            writer.WriteByte(finishedlevel);
        }

        public virtual void Deserialize(ICustomDataInput reader)
        {
            id = reader.ReadVarUhShort();
            finishedlevel = reader.ReadByte();
        }

    }
}