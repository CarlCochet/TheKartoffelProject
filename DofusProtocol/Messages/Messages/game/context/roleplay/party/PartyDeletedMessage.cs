

// Generated on 02/17/2017 01:58:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyDeletedMessage : AbstractPartyMessage
    {
        public const uint Id = 6261;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public PartyDeletedMessage()
        {
        }
        
        public PartyDeletedMessage(int partyId)
         : base(partyId)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}