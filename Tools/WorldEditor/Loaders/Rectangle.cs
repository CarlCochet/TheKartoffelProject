using System;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [Serializable]
    [D2OClass("Rectangle", "flash.geom")]
    public class Rectangle : IDataObject
    {
        public int x;
        public int y;
        public int width;
        public int height;
    }
}