

// Generated on 02/17/2017 01:57:54
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayArenaRegistrationOnlyStatusMessage : Message
    {
        public const uint Id = 6284;
        public override uint MessageId
        {
            get { return Id; }
        }

        public bool registered;
        public sbyte step;
        public int battleMode;

        public GameRolePlayArenaRegistrationOnlyStatusMessage()
        {
        }

        public GameRolePlayArenaRegistrationOnlyStatusMessage(bool registered, sbyte step, int battleMode)
        {
            this.registered = registered;
            this.step = step;
            this.battleMode = battleMode;
        }

        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(registered);
            writer.WriteSByte(step);
            writer.WriteInt(battleMode);
        }

        public override void Deserialize(IDataReader reader)
        {
            registered = reader.ReadBoolean();
            step = reader.ReadSByte();
            battleMode = reader.ReadInt();
        }

    }

}