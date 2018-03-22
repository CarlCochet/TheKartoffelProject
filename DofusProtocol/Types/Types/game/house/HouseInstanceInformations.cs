

// Generated on 02/17/2017 01:53:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HouseInstanceInformations
    {
        public const short Id = 511;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public bool secondHand;
        public bool isOnSale;
        public bool isSaleLocked;
        public uint instanceId;
        public string ownerName;
        
        public HouseInstanceInformations()
        {
        }
        
        public HouseInstanceInformations(bool secondHand, bool isOnSale, bool isSaleLocked, uint instanceId, string ownerName)
        {
            this.secondHand = secondHand;
            this.isOnSale = isOnSale;
            this.isSaleLocked = isSaleLocked;
            this.instanceId = instanceId;
            this.ownerName = ownerName;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, secondHand);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, isOnSale);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, isSaleLocked);
            writer.WriteByte(flag1);
            writer.WriteUInt(instanceId);
            writer.WriteUTF(ownerName);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            secondHand = BooleanByteWrapper.GetFlag(flag1, 0);
            isOnSale = BooleanByteWrapper.GetFlag(flag1, 1);
            isSaleLocked = BooleanByteWrapper.GetFlag(flag1, 2);
            instanceId = reader.ReadUInt();
            if (instanceId < 0 || instanceId > 4294967295)
                throw new Exception("Forbidden value on instanceId = " + instanceId + ", it doesn't respect the following condition : instanceId < 0 || instanceId > 4294967295");
            ownerName = reader.ReadUTF();
        }
        
        
    }
    
}