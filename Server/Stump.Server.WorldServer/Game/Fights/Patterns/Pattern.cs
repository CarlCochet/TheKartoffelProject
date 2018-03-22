using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Fights.Patterns
{
    public class Pattern
    {
        public Pattern(short[] redplacements, short[] blueplacement)
        {
            RedCells = redplacements;
            BlueCells = blueplacement;
        }

        public short[] RedCells { get; private set; }

        public short[] BlueCells { get; private set; }
    }
}
