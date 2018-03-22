using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Core.IO
{
    public interface ICustomDataInput : IDataReader
    {
        new int ReadVarInt();

        uint ReadVarUhInt();

        new short ReadVarShort();

        ushort ReadVarUhShort();

        new double ReadVarLong();

        double ReadVarUhLong();
    }
}
