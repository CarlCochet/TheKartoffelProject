

// Generated on 02/17/2017 01:58:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeCraftCountModifiedMessage : Message
    {
        public const uint Id = 6595;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int count;
        
        public ExchangeCraftCountModifiedMessage()
        {
        }
        
        public ExchangeCraftCountModifiedMessage(int count)
        {
            this.count = count;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(count);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            count = reader.ReadVarInt();
        }
        
    }
    
}