

// Generated on 02/17/2017 01:52:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AchievementObjective
    {
        public const short Id = 404;
        public virtual short TypeId
        {
            get { return Id; }
        }

        public uint id;
        public ushort maxValue;

        public AchievementObjective()
        {
        }

        public AchievementObjective(uint id, ushort maxValue)
        {
            this.id = id;
            this.maxValue = maxValue;
        }

        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt((int)id);
            writer.WriteVarShort((short)maxValue);
        }

        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarUInt();
            maxValue = reader.ReadVarUhShort();
        }

    }

}