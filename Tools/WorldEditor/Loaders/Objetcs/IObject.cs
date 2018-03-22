using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldEditor.Loaders.Objetcs
{
    public interface IObject
    {
        int Id { get; set; }
        object GetD2oClass(object baseObj);
    }
}
