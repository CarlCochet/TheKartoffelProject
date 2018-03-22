using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WorldEditor.Helpers
{
    public class CanvasAutoSize : Canvas
    {
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            if (base.InternalChildren.Count == 0)
            {
                return new Size(0.0, 0.0);
            }
            else
            {
                double minX = Math.Min(0.0, base.InternalChildren.OfType<UIElement>().Min(i => (double)i.GetValue(Canvas.LeftProperty)));
                double minY = Math.Min(0.0, base.InternalChildren.OfType<UIElement>().Min(i => (double)i.GetValue(Canvas.TopProperty)));
                foreach (UIElement child in base.InternalChildren)
                {
                    double left = Canvas.GetLeft(child);
                    double top = Canvas.GetTop(child);
                    child.Arrange(new Rect(new Point(left - minX, top - minY), child.DesiredSize));
                }
                return arrangeSize;
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint);
            Size size = new Size(0.0, 0.0);
            Point minPoint = new Point(0.0, 0.0);
            foreach (UIElement child in base.InternalChildren.OfType<UIElement>())
            {
                double left = (double)child.GetValue(Canvas.LeftProperty);
                double top = (double)child.GetValue(Canvas.TopProperty);
                if (left < 0.0 && left < minPoint.X)
                {
                    minPoint.X = left;
                }
                if (top < 0.0 && top < minPoint.Y)
                {
                    minPoint.Y = top;
                }
                if (child.DesiredSize.Width + left > size.Width)
                {
                    size.Width = child.DesiredSize.Width + left;
                }
                if (child.DesiredSize.Height + top > size.Height)
                {
                    size.Height = child.DesiredSize.Height + top;
                }
            }
            size.Width += -minPoint.X;
            size.Height += -minPoint.Y;
            return size;
        }
    }
}