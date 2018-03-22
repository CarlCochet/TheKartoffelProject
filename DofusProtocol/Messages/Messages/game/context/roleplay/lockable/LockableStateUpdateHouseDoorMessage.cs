

// Generated on 02/17/2017 01:57:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class LockableStateUpdateHouseDoorMessage : LockableStateUpdateAbstractMessage
    {
        public const uint Id = 5668;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int houseId;
        public uint instanceId;
        public bool secondHand;
        
        public LockableStateUpdateHouseDoorMessage()
        {
        }
        
        public LockableStateUpdateHouseDoorMessage(bool locked, int houseId, uint instanceId, bool secondHand)
         : base(locked)
        {
            this.houseId = houseId;
            this.instanceId = instanceId;
            this.secondHand = secondHand;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(houseId);
            writer.WriteUInt(instanceId);
            writer.WriteBoolean(secondHand);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            houseId = reader.ReadVarInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            instanceId = reader.ReadUInt();
            if (instanceId < 0 || instanceId > 4294967295)
                throw new Exception("Forbidden value on instanceId = " + instanceId + ", it doesn't respect the following condition : instanceId < 0 || instanceId > 4294967295");
            secondHand = reader.ReadBoolean();
        }
        
    }
    
}