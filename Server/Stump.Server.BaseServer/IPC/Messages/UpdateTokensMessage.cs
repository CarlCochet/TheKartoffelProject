using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.BaseServer.IPC.Messages
{
    [ProtoContract]
    public class UpdateTokensMessage : IPCMessage
    {
        public UpdateTokensMessage()
        {

        }
        public UpdateTokensMessage(int tokens, int accountId)
        {
            Tokens = tokens;
            AccountId = accountId;
        }

        [ProtoMember(2)]
        public int Tokens
        {
            get;
            set;
        }
        [ProtoMember(3)]
        public int AccountId
        {
            get;
            set;
        }
    }
}
