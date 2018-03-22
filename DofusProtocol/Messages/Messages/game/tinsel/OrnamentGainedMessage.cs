

// Generated on 02/17/2017 01:58:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class OrnamentGainedMessage : Message
    {
        public const uint Id = 6368;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public ushort ornamentId;
        
        public OrnamentGainedMessage()
        {
        }
        
        public OrnamentGainedMessage(ushort ornamentId)
        {
            this.ornamentId = ornamentId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort(ornamentId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            ornamentId = reader.ReadUShort();
            if (ornamentId < 0)
                throw new Exception("Forbidden value on ornamentId = " + ornamentId + ", it doesn't respect the following condition : ornamentId < 0");
        }
        
    }
    
}