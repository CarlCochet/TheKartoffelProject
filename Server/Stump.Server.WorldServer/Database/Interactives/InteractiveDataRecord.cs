using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Database.Interactives
{
    public class InteractiveDataRecordRelator
    {
        public static string FetchQuery = "SELECT * FROM interactive_datas";
    }
    [TableName("interactive_datas")]
    public class InteractiveDataRecord
    {
        public int Id
        {
            get;
            set;
        }
        public int MapId
        {
            get;
            set;
        }
        public int Data1
        {
            get;
            set;
        }
        public int Data2
        {
            get;
            set;
        }
    }
}
