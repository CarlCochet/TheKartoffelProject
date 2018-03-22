

// Generated on 02/17/2017 01:57:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SelectedServerDataExtendedMessage : SelectedServerDataMessage
    {
        public const uint Id = 6469;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> serverIds;
        
        public SelectedServerDataExtendedMessage()
        {
        }
        
        public SelectedServerDataExtendedMessage(short serverId, string address, short port, bool canCreateNewCharacter, IEnumerable<sbyte> ticket, IEnumerable<short> serverIds)
         : base(serverId, address, port, canCreateNewCharacter, ticket)
        {
            this.serverIds = serverIds;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            var serverIds_before = writer.Position;
            var serverIds_count = 0;
            writer.WriteShort(0);
            foreach (var entry in serverIds)
            {
                 writer.WriteVarShort(entry);
                 serverIds_count++;
            }
            var serverIds_after = writer.Position;
            writer.Seek((int)serverIds_before);
            writer.WriteShort((short)serverIds_count);
            writer.Seek((int)serverIds_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadShort();
            var serverIds_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 serverIds_[i] = reader.ReadVarShort();
                 if (serverIds_[i] < 0)
                     throw new Exception("Forbidden value on serverIds_[i] = " + serverIds_[i] + ", it doesn't respect the following condition : serverIds_[i] < 0");
            }
            serverIds = serverIds_;
        }
        
    }
    
}