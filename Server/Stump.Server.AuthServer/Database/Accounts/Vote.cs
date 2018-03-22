using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.AuthServer.Database.Accounts
{
    [TableName("votes")]
    public class Vote
    {
        public int Id
        {
            get;
            set;
        }
        public int AcctID
        {
            get;
            set;
        }
    }
}
