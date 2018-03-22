

// Generated on 02/17/2017 01:58:09
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildHouseTeleportRequestMessage : Message
    {
        public const uint Id = 5712;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int houseId;
        public int houseInstanceId;
        
        public GuildHouseTeleportRequestMessage()
        {
        }
        
        public GuildHouseTeleportRequestMessage(int houseId, int houseInstanceId)
        {
            this.houseId = houseId;
            this.houseInstanceId = houseInstanceId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(houseId);
            writer.WriteInt(houseInstanceId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            houseId = reader.ReadVarInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            houseInstanceId = reader.ReadInt();
        }
        
    }
    
}