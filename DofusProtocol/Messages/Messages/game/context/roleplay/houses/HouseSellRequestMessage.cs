

// Generated on 02/17/2017 01:57:56
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HouseSellRequestMessage : Message
    {
        public const uint Id = 5697;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public uint instanceId;
        public long amount;
        public bool forSale;
        
        public HouseSellRequestMessage()
        {
        }
        
        public HouseSellRequestMessage(uint instanceId, long amount, bool forSale)
        {
            this.instanceId = instanceId;
            this.amount = amount;
            this.forSale = forSale;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUInt(instanceId);
            writer.WriteVarLong(amount);
            writer.WriteBoolean(forSale);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            instanceId = reader.ReadUInt();
            if (instanceId < 0 || instanceId > 4294967295)
                throw new Exception("Forbidden value on instanceId = " + instanceId + ", it doesn't respect the following condition : instanceId < 0 || instanceId > 4294967295");
            amount = reader.ReadVarLong();
            if (amount < 0 || amount > 9007199254740990)
                throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0 || amount > 9007199254740990");
            forSale = reader.ReadBoolean();
        }
        
    }
    
}