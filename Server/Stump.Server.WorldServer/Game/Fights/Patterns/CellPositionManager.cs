using Stump.Core.IO;
using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.Server.BaseServer.Initialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stump.Server.WorldServer.Game.Maps;
namespace Stump.Server.WorldServer.Game.Fights.Patterns
{
    public class CellPositionManager : Singleton<CellPositionManager>
    {
        private List<Pattern> m_patterns;

        [Initialization(InitializationPass.Fourth)]
        public void Initialize()
        {
            try
            {
                m_patterns = new List<Pattern>();
                var path = Directory.GetCurrentDirectory() + "\\Pattern\\";
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    var buffer = File.ReadAllBytes(file);
                    var reader = new BigEndianReader(buffer);
                    var length = reader.ReadInt();
                    var data = reader.ReadBytes(length);
                    var reds = DeserializeFightCells(data);
                    length = reader.ReadInt();
                    data = reader.ReadBytes(length);
                    var blue = DeserializeFightCells(data);
                    m_patterns.Add(new Pattern(reds, blue));
                }
            }
            catch
            {
                //ignore
            }
        }

        public Pattern GetPatternExist(Map map)
        {
            Pattern result;
            if (m_patterns.Count < 1)
            {
                result = null;
            }
            else
            {
                var possiblePaterns = new List<Pattern>();
                foreach (var pattern in m_patterns)
                {
                    var redPosible = true;
                    var array = pattern.RedCells;
                    foreach (var test in array.Where(test => !map.Cells[test].Walkable))
                        redPosible = false;
                    var bluePosible = true;
                    array = pattern.BlueCells;
                    foreach (var test in array.Where(test => !map.Cells[test].Walkable))
                        bluePosible = false;
                    if (bluePosible && redPosible)
                        possiblePaterns.Add(pattern);
                }
                result = possiblePaterns.Count < 1 ? null : possiblePaterns[new AsyncRandom().Next(0, possiblePaterns.Count)];
            }
            return result;
        }

        public static short[] DeserializeFightCells(byte[] bytes)
        {
            if (bytes.Length % 2 != 0)
            {
                throw new ArgumentException("bytes.Length % 2 != 0");
            }
            var array = new short[bytes.Length / 2];
            var i = 0;
            var num = 0;
            while (i < bytes.Length)
            {
                array[num] = (short)(bytes[i] << 8 | bytes[i + 1]);
                i += 2;
                num++;
            }
            return array;
        }
    }
}
