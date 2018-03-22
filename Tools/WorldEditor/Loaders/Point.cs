using System;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [Serializable]
    [D2OClass("Point", "flash.geom")]
    public class Point : IDataObject
    {
        public int x;
        public int y;
        public double length;
    }
}