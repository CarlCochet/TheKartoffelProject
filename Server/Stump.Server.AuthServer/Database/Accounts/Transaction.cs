using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.AuthServer.Database.Accounts
{
    [TableName("transactions")]
    public class Transactions
    {
        public int AcctID
        {
            get;
            set;
        }
        public int Tokens
        { get; set; }
    }
}
