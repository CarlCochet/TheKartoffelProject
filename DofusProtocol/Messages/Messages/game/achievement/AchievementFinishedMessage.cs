

// Generated on 02/17/2017 01:57:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementFinishedMessage : Message
    {
        public const uint Id = 6208;
        public override uint MessageId => Id;


        public ushort id;
        public byte finishedlevel;

        public AchievementFinishedMessage()
        {
        }

        public AchievementFinishedMessage(ushort id, byte finishedlevel)
        {
            this.id = id;
            this.finishedlevel = finishedlevel;
        }

        //public override void Serialize(ICustomDataOutput writer)
        //{
        //    writer.WriteVarShort((int)id);
        //    writer.WriteByte(finishedlevel);
        //}

        //public override void Deserialize(ICustomDataInput reader)
        //{
        //    id = reader.ReadVarUhShort();
        //    finishedlevel = reader.ReadByte();
        //}

        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort((short)id);
            writer.WriteByte(finishedlevel);
        }

        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarUhShort();
            finishedlevel = reader.ReadByte();
        }
    }
}