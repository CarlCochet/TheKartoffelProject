

// Generated on 02/17/2017 01:57:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementFinishedInformationMessage : AchievementFinishedMessage
    {
        public const uint Id = 6381;
        public override uint MessageId => Id;


        public string name;
        public double playerId;

        public AchievementFinishedInformationMessage()
        {
        }

        public AchievementFinishedInformationMessage(ushort id, byte finishedlevel, string name, double playerId)
         : base(id, finishedlevel)
        {
            this.name = name;
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(name);
            writer.WriteVarLong((long)playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadUTF();
            playerId = reader.ReadVarUhLong();
        }

    }

}