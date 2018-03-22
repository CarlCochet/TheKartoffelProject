

// Generated on 02/17/2017 01:53:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HouseInformationsInside : HouseInformations
    {
        public const short Id = 218;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public uint instanceId;
        public bool secondHand;
        public int ownerId;
        public string ownerName;
        public short worldX;
        public short worldY;
        public long price;
        public bool isLocked;
        
        public HouseInformationsInside()
        {
        }
        
        public HouseInformationsInside(int houseId, short modelId, uint instanceId, bool secondHand, int ownerId, string ownerName, short worldX, short worldY, long price, bool isLocked)
         : base(houseId, modelId)
        {
            this.instanceId = instanceId;
            this.secondHand = secondHand;
            this.ownerId = ownerId;
            this.ownerName = ownerName;
            this.worldX = worldX;
            this.worldY = worldY;
            this.price = price;
            this.isLocked = isLocked;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUInt(instanceId);
            writer.WriteBoolean(secondHand);
            writer.WriteInt(ownerId);
            writer.WriteUTF(ownerName);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteVarLong(price);
            writer.WriteBoolean(isLocked);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            instanceId = reader.ReadUInt();
            if (instanceId < 0 || instanceId > 4294967295)
                throw new Exception("Forbidden value on instanceId = " + instanceId + ", it doesn't respect the following condition : instanceId < 0 || instanceId > 4294967295");
            secondHand = reader.ReadBoolean();
            ownerId = reader.ReadInt();
            ownerName = reader.ReadUTF();
            worldX = reader.ReadShort();
            if (worldX < -255 || worldX > 255)
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : worldX < -255 || worldX > 255");
            worldY = reader.ReadShort();
            if (worldY < -255 || worldY > 255)
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : worldY < -255 || worldY > 255");
            price = reader.ReadVarLong();
            if (price < 0 || price > 9007199254740990)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0 || price > 9007199254740990");
            isLocked = reader.ReadBoolean();
        }
        
        
    }
    
}