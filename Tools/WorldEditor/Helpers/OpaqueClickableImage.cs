using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WorldEditor.Helpers
{
    public class OpaqueClickableImage : Image
    {
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            BitmapSource source = (BitmapSource)base.Source;
            int x = (int)(hitTestParameters.HitPoint.X / base.ActualWidth * (double)source.PixelWidth);
            int y = (int)(hitTestParameters.HitPoint.Y / base.ActualHeight * (double)source.PixelHeight);
            if (x == source.PixelWidth)
            {
                x--;
            }
            if (y == source.PixelHeight)
            {
                y--;
            }
            byte[] pixels = new byte[4];
            source.CopyPixels(new Int32Rect(x, y, 1, 1), pixels, 4, 0);
            return (pixels[3] < 1) ? null : new PointHitTestResult(this, hitTestParameters.HitPoint);
        }
    }
}
