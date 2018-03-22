

// Generated on 02/17/2017 01:57:54
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayArenaSwitchToFightServerMessage : Message
    {
        public const uint Id = 6575;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string address;
        public short port;
        public IEnumerable<sbyte> ticket;
        
        public GameRolePlayArenaSwitchToFightServerMessage()
        {
        }
        
        public GameRolePlayArenaSwitchToFightServerMessage(string address, short port, IEnumerable<sbyte> ticket)
        {
            this.address = address;
            this.port = port;
            this.ticket = ticket;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(address);
            writer.WriteShort(port);
            writer.WriteVarInt((int)ticket.Count());
            foreach (var entry in ticket)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            address = reader.ReadUTF();
            port = reader.ReadShort();
            if (port < 0 || port > 65535)
                throw new Exception("Forbidden value on port = " + port + ", it doesn't respect the following condition : port < 0 || port > 65535");
            var limit = reader.ReadVarInt();
            var ticket_ = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 ticket_[i] = reader.ReadSByte();
            }
            ticket = ticket_;
        }
        
    }
    
}