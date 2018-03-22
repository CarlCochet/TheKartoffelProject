

// Generated on 02/17/2017 01:57:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayShowMultipleActorsMessage : Message
    {
        public const uint Id = 6712;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.GameRolePlayActorInformations> informationsList;
        
        public GameRolePlayShowMultipleActorsMessage()
        {
        }
        
        public GameRolePlayShowMultipleActorsMessage(IEnumerable<Types.GameRolePlayActorInformations> informationsList)
        {
            this.informationsList = informationsList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var informationsList_before = writer.Position;
            var informationsList_count = 0;
            writer.WriteShort(0);
            foreach (var entry in informationsList)
            {
                 entry.Serialize(writer);
                 informationsList_count++;
            }
            var informationsList_after = writer.Position;
            writer.Seek((int)informationsList_before);
            writer.WriteShort((short)informationsList_count);
            writer.Seek((int)informationsList_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadShort();
            var informationsList_ = new Types.GameRolePlayActorInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 informationsList_[i] = new Types.GameRolePlayActorInformations();
                 informationsList_[i].Deserialize(reader);
            }
            informationsList = informationsList_;
        }
        
    }
    
}