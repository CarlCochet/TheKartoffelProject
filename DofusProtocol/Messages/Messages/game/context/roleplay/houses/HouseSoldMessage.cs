

// Generated on 02/17/2017 01:57:56
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HouseSoldMessage : Message
    {
        public const uint Id = 5737;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int houseId;
        public uint instanceId;
        public bool secondHand;
        public long realPrice;
        public string buyerName;
        
        public HouseSoldMessage()
        {
        }
        
        public HouseSoldMessage(int houseId, uint instanceId, bool secondHand, long realPrice, string buyerName)
        {
            this.houseId = houseId;
            this.instanceId = instanceId;
            this.secondHand = secondHand;
            this.realPrice = realPrice;
            this.buyerName = buyerName;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(houseId);
            writer.WriteUInt(instanceId);
            writer.WriteBoolean(secondHand);
            writer.WriteVarLong(realPrice);
            writer.WriteUTF(buyerName);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            houseId = reader.ReadVarInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            instanceId = reader.ReadUInt();
            if (instanceId < 0 || instanceId > 4294967295)
                throw new Exception("Forbidden value on instanceId = " + instanceId + ", it doesn't respect the following condition : instanceId < 0 || instanceId > 4294967295");
            secondHand = reader.ReadBoolean();
            realPrice = reader.ReadVarLong();
            if (realPrice < 0 || realPrice > 9007199254740990)
                throw new Exception("Forbidden value on realPrice = " + realPrice + ", it doesn't respect the following condition : realPrice < 0 || realPrice > 9007199254740990");
            buyerName = reader.ReadUTF();
        }
        
    }
    
}