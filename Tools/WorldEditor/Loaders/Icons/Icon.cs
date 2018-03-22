using System;
using System.IO;
using System.Windows.Media.Imaging;
namespace WorldEditor.Loaders.Icons
{
    public class Icon
    {
        public int Id { get; private set; }

        public BitmapImage Image { get; private set; }

        public Icon(int id, byte[] data)
        {
            Id = id;
            Image = new BitmapImage();
            Image.BeginInit();
            Image.StreamSource = new MemoryStream(data);
            Image.EndInit();
            Image.Freeze();
        }
    }
}