

// Generated on 02/17/2017 01:53:02
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HouseGuildedInformations : HouseInstanceInformations
    {
        public const short Id = 512;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public Types.GuildInformations guildInfo;
        
        public HouseGuildedInformations()
        {
        }
        
        public HouseGuildedInformations(bool secondHand, bool isOnSale, bool isSaleLocked, uint instanceId, string ownerName, Types.GuildInformations guildInfo)
         : base(secondHand, isOnSale, isSaleLocked, instanceId, ownerName)
        {
            this.guildInfo = guildInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            guildInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guildInfo = new Types.GuildInformations();
            guildInfo.Deserialize(reader);
        }
        
        
    }
    
}