using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldEditor.Loaders.Data
{
    [DebuggerDisplay("{ClassName}")]
    public class D2OTable
    {
        public Type Type { get; set; }

        public string ClassName { get; set; }

        public string TableName { get; set; }
    }
}
