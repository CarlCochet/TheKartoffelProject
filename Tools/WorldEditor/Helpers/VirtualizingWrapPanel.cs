using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
namespace WorldEditor.Helpers
{
    public class VirtualizingWrapPanel : VirtualizingPanel, IScrollInfo
    {
        private class ItemAbstraction
        {
            private VirtualizingWrapPanel.WrapPanelAbstraction _panel;
            public readonly int _index;
            private int _sectionIndex = -1;
            private int _section = -1;
            public int SectionIndex
            {
                get
                {
                    int result;
                    if (_sectionIndex == -1)
                    {
                        result = _index % _panel._averageItemsPerSection - 1;
                    }
                    else
                    {
                        result = _sectionIndex;
                    }
                    return result;
                }
                set
                {
                    if (_sectionIndex == -1)
                    {
                        _sectionIndex = value;
                    }
                }
            }
            public int Section
            {
                get
                {
                    int result;
                    if (_section == -1)
                    {
                        result = _index / _panel._averageItemsPerSection;
                    }
                    else
                    {
                        result = _section;
                    }
                    return result;
                }
                set
                {
                    if (_section == -1)
                    {
                        _section = value;
                    }
                }
            }
            public ItemAbstraction(VirtualizingWrapPanel.WrapPanelAbstraction panel, int index)
            {
                _panel = panel;
                _index = index;
            }
        }
        private class WrapPanelAbstraction : IEnumerable<VirtualizingWrapPanel.ItemAbstraction>, IEnumerable
        {
            public readonly int _itemCount;
            public int _averageItemsPerSection;
            private int _currentSetSection = -1;
            private int _currentSetItemIndex = -1;
            private int _itemsInCurrentSecction = 0;
            private object _syncRoot = new object();
            public int SectionCount
            {
                get
                {
                    int ret = _currentSetSection + 1;
                    if (_currentSetItemIndex + 1 < Items.Count)
                    {
                        int itemsLeft = Items.Count - _currentSetItemIndex;
                        ret += itemsLeft / _averageItemsPerSection + 1;
                    }
                    return ret;
                }
            }
            private ReadOnlyCollection<VirtualizingWrapPanel.ItemAbstraction> Items
            {
                get;
                set;
            }
            public VirtualizingWrapPanel.ItemAbstraction this[int index]
            {
                get
                {
                    return Items[index];
                }
            }
            public WrapPanelAbstraction(int itemCount)
            {
                List<VirtualizingWrapPanel.ItemAbstraction> items = new List<VirtualizingWrapPanel.ItemAbstraction>(itemCount);
                for (int i = 0; i < itemCount; i++)
                {
                    VirtualizingWrapPanel.ItemAbstraction item = new VirtualizingWrapPanel.ItemAbstraction(this, i);
                    items.Add(item);
                }
                Items = new ReadOnlyCollection<VirtualizingWrapPanel.ItemAbstraction>(items);
                _averageItemsPerSection = itemCount;
                _itemCount = itemCount;
            }
            public void SetItemSection(int index, int section)
            {
                lock (_syncRoot)
                {
                    if (section <= _currentSetSection + 1 && index == _currentSetItemIndex + 1)
                    {
                        _currentSetItemIndex++;
                        Items[index].Section = section;
                        if (section == _currentSetSection + 1)
                        {
                            _currentSetSection = section;
                            if (section > 0)
                            {
                                _averageItemsPerSection = index / section;
                            }
                            _itemsInCurrentSecction = 1;
                        }
                        else
                        {
                            _itemsInCurrentSecction++;
                        }
                        Items[index].SectionIndex = _itemsInCurrentSecction - 1;
                    }
                }
            }
            public IEnumerator<VirtualizingWrapPanel.ItemAbstraction> GetEnumerator()
            {
                return Items.GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        private UIElementCollection _children;
        private ItemsControl _itemsControl;
        private IItemContainerGenerator _generator;
        private Point _offset = new Point(0.0, 0.0);
        private Size _extent = new Size(0.0, 0.0);
        private Size _viewport = new Size(0.0, 0.0);
        private int firstIndex = 0;
        private Size childSize;
        private Size _pixelMeasuredViewport = new Size(0.0, 0.0);
        private Dictionary<UIElement, Rect> _realizedChildLayout = new Dictionary<UIElement, Rect>();
        private VirtualizingWrapPanel.WrapPanelAbstraction _abstractPanel;
        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(VirtualizingWrapPanel), new FrameworkPropertyMetadata(double.PositiveInfinity));
        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(VirtualizingWrapPanel), new FrameworkPropertyMetadata(double.PositiveInfinity));
        public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof(VirtualizingWrapPanel), new FrameworkPropertyMetadata(Orientation.Horizontal));
        private bool _canHScroll = false;
        private bool _canVScroll = false;
        private ScrollViewer _owner;
        private Size ChildSlotSize
        {
            get
            {
                return new Size(ItemWidth, ItemHeight);
            }
        }
        [TypeConverter(typeof(LengthConverter))]
        public double ItemHeight
        {
            get
            {
                return (double)base.GetValue(VirtualizingWrapPanel.ItemHeightProperty);
            }
            set
            {
                base.SetValue(VirtualizingWrapPanel.ItemHeightProperty, value);
            }
        }
        [TypeConverter(typeof(LengthConverter))]
        public double ItemWidth
        {
            get
            {
                return (double)base.GetValue(VirtualizingWrapPanel.ItemWidthProperty);
            }
            set
            {
                base.SetValue(VirtualizingWrapPanel.ItemWidthProperty, value);
            }
        }
        public Orientation Orientation
        {
            get
            {
                return (Orientation)base.GetValue(VirtualizingWrapPanel.OrientationProperty);
            }
            set
            {
                base.SetValue(VirtualizingWrapPanel.OrientationProperty, value);
            }
        }
        public bool CanHorizontallyScroll
        {
            get
            {
                return _canHScroll;
            }
            set
            {
                _canHScroll = value;
            }
        }
        public bool CanVerticallyScroll
        {
            get
            {
                return _canVScroll;
            }
            set
            {
                _canVScroll = value;
            }
        }
        public double ExtentHeight
        {
            get
            {
                return _extent.Height;
            }
        }
        public double ExtentWidth
        {
            get
            {
                return _extent.Width;
            }
        }
        public double HorizontalOffset
        {
            get
            {
                return _offset.X;
            }
        }
        public double VerticalOffset
        {
            get
            {
                return _offset.Y;
            }
        }
        public ScrollViewer ScrollOwner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
            }
        }
        public double ViewportHeight
        {
            get
            {
                return _viewport.Height;
            }
        }
        public double ViewportWidth
        {
            get
            {
                return _viewport.Width;
            }
        }
        public void SetFirstRowViewItemIndex(int index)
        {
            SetVerticalOffset((double)index / Math.Floor(_viewport.Width / childSize.Width));
            SetHorizontalOffset((double)index / Math.Floor(_viewport.Height / childSize.Height));
        }
        private void Resizing(object sender, EventArgs e)
        {
            if (_viewport.Width != 0.0)
            {
                int firstIndexCache = firstIndex;
                _abstractPanel = null;
                MeasureOverride(_viewport);
                SetFirstRowViewItemIndex(firstIndex);
                firstIndex = firstIndexCache;
            }
        }
        public int GetFirstVisibleSection()
        {
            int maxSection = _abstractPanel.Max((VirtualizingWrapPanel.ItemAbstraction x) => x.Section);
            int section;
            if (Orientation == Orientation.Horizontal)
            {
                section = (int)_offset.Y;
            }
            else
            {
                section = (int)_offset.X;
            }
            if (section > maxSection)
            {
                section = maxSection;
            }
            return section;
        }
        public int GetFirstVisibleIndex()
        {
            int section = GetFirstVisibleSection();
            VirtualizingWrapPanel.ItemAbstraction item = (
                from x in _abstractPanel
                where x.Section == section
                select x).FirstOrDefault<VirtualizingWrapPanel.ItemAbstraction>();
            int result;
            if (item != null)
            {
                result = item._index;
            }
            else
            {
                result = 0;
            }
            return result;
        }
        private void CleanUpItems(int minDesiredGenerated, int maxDesiredGenerated)
        {
            for (int i = _children.Count - 1; i >= 0; i--)
            {
                GeneratorPosition childGeneratorPos = new GeneratorPosition(i, 0);
                int itemIndex = _generator.IndexFromGeneratorPosition(childGeneratorPos);
                if (itemIndex < minDesiredGenerated || itemIndex > maxDesiredGenerated)
                {
                    _generator.Remove(childGeneratorPos, 1);
                    base.RemoveInternalChildRange(i, 1);
                }
            }
        }
        private void ComputeExtentAndViewport(Size pixelMeasuredViewportSize, int visibleSections)
        {
            if (Orientation == Orientation.Horizontal)
            {
                _viewport.Height = (double)visibleSections;
                _viewport.Width = pixelMeasuredViewportSize.Width;
            }
            else
            {
                _viewport.Width = (double)visibleSections;
                _viewport.Height = pixelMeasuredViewportSize.Height;
            }
            if (Orientation == Orientation.Horizontal)
            {
                _extent.Height = (double)_abstractPanel.SectionCount + ViewportHeight - 1.0;
            }
            else
            {
                _extent.Width = (double)_abstractPanel.SectionCount + ViewportWidth - 1.0;
            }
            _owner.InvalidateScrollInfo();
        }
        private void ResetScrollInfo()
        {
            _offset.X = 0.0;
            _offset.Y = 0.0;
        }
        private int GetNextSectionClosestIndex(int itemIndex)
        {
            VirtualizingWrapPanel.ItemAbstraction abstractItem = _abstractPanel[itemIndex];
            int result;
            if (abstractItem.Section < _abstractPanel.SectionCount - 1)
            {
                VirtualizingWrapPanel.ItemAbstraction ret = (
                    from x in _abstractPanel
                    where x.Section == abstractItem.Section + 1
                    orderby Math.Abs(x.SectionIndex - abstractItem.SectionIndex)
                    select x).First<VirtualizingWrapPanel.ItemAbstraction>();
                result = ret._index;
            }
            else
            {
                result = itemIndex;
            }
            return result;
        }
        private int GetLastSectionClosestIndex(int itemIndex)
        {
            VirtualizingWrapPanel.ItemAbstraction abstractItem = _abstractPanel[itemIndex];
            int result;
            if (abstractItem.Section > 0)
            {
                VirtualizingWrapPanel.ItemAbstraction ret = (
                    from x in _abstractPanel
                    where x.Section == abstractItem.Section - 1
                    orderby Math.Abs(x.SectionIndex - abstractItem.SectionIndex)
                    select x).First<VirtualizingWrapPanel.ItemAbstraction>();
                result = ret._index;
            }
            else
            {
                result = itemIndex;
            }
            return result;
        }
        private void NavigateDown()
        {
            ItemContainerGenerator gen = _generator.GetItemContainerGeneratorForPanel(this);
            UIElement selected = (UIElement)Keyboard.FocusedElement;
            int itemIndex = gen.IndexFromContainer(selected);
            int depth = 0;
            while (itemIndex == -1)
            {
                selected = (UIElement)VisualTreeHelper.GetParent(selected);
                itemIndex = gen.IndexFromContainer(selected);
                depth++;
            }
            DependencyObject next;
            if (Orientation == Orientation.Horizontal)
            {
                int nextIndex = GetNextSectionClosestIndex(itemIndex);
                for (next = gen.ContainerFromIndex(nextIndex); next == null; next = gen.ContainerFromIndex(nextIndex))
                {
                    SetVerticalOffset(VerticalOffset + 1.0);
                    base.UpdateLayout();
                }
            }
            else
            {
                if (itemIndex == _abstractPanel._itemCount - 1)
                {
                    return;
                }
                for (next = gen.ContainerFromIndex(itemIndex + 1); next == null; next = gen.ContainerFromIndex(itemIndex + 1))
                {
                    SetHorizontalOffset(HorizontalOffset + 1.0);
                    base.UpdateLayout();
                }
            }
            while (depth != 0)
            {
                next = VisualTreeHelper.GetChild(next, 0);
                depth--;
            }
            (next as UIElement).Focus();
        }
        private void NavigateLeft()
        {
            ItemContainerGenerator gen = _generator.GetItemContainerGeneratorForPanel(this);
            UIElement selected = (UIElement)Keyboard.FocusedElement;
            int itemIndex = gen.IndexFromContainer(selected);
            int depth = 0;
            while (itemIndex == -1)
            {
                selected = (UIElement)VisualTreeHelper.GetParent(selected);
                itemIndex = gen.IndexFromContainer(selected);
                depth++;
            }
            DependencyObject next;
            if (Orientation == Orientation.Vertical)
            {
                int nextIndex = GetLastSectionClosestIndex(itemIndex);
                for (next = gen.ContainerFromIndex(nextIndex); next == null; next = gen.ContainerFromIndex(nextIndex))
                {
                    SetHorizontalOffset(HorizontalOffset - 1.0);
                    base.UpdateLayout();
                }
            }
            else
            {
                if (itemIndex == 0)
                {
                    return;
                }
                for (next = gen.ContainerFromIndex(itemIndex - 1); next == null; next = gen.ContainerFromIndex(itemIndex - 1))
                {
                    SetVerticalOffset(VerticalOffset - 1.0);
                    base.UpdateLayout();
                }
            }
            while (depth != 0)
            {
                next = VisualTreeHelper.GetChild(next, 0);
                depth--;
            }
            (next as UIElement).Focus();
        }
        private void NavigateRight()
        {
            ItemContainerGenerator gen = _generator.GetItemContainerGeneratorForPanel(this);
            UIElement selected = (UIElement)Keyboard.FocusedElement;
            int itemIndex = gen.IndexFromContainer(selected);
            int depth = 0;
            while (itemIndex == -1)
            {
                selected = (UIElement)VisualTreeHelper.GetParent(selected);
                itemIndex = gen.IndexFromContainer(selected);
                depth++;
            }
            DependencyObject next;
            if (Orientation == Orientation.Vertical)
            {
                int nextIndex = GetNextSectionClosestIndex(itemIndex);
                for (next = gen.ContainerFromIndex(nextIndex); next == null; next = gen.ContainerFromIndex(nextIndex))
                {
                    SetHorizontalOffset(HorizontalOffset + 1.0);
                    base.UpdateLayout();
                }
            }
            else
            {
                if (itemIndex == _abstractPanel._itemCount - 1)
                {
                    return;
                }
                for (next = gen.ContainerFromIndex(itemIndex + 1); next == null; next = gen.ContainerFromIndex(itemIndex + 1))
                {
                    SetVerticalOffset(VerticalOffset + 1.0);
                    base.UpdateLayout();
                }
            }
            while (depth != 0)
            {
                next = VisualTreeHelper.GetChild(next, 0);
                depth--;
            }
            (next as UIElement).Focus();
        }
        private void NavigateUp()
        {
            ItemContainerGenerator gen = _generator.GetItemContainerGeneratorForPanel(this);
            UIElement selected = (UIElement)Keyboard.FocusedElement;
            int itemIndex = gen.IndexFromContainer(selected);
            int depth = 0;
            while (itemIndex == -1)
            {
                selected = (UIElement)VisualTreeHelper.GetParent(selected);
                itemIndex = gen.IndexFromContainer(selected);
                depth++;
            }
            DependencyObject next;
            if (Orientation == Orientation.Horizontal)
            {
                int nextIndex = GetLastSectionClosestIndex(itemIndex);
                for (next = gen.ContainerFromIndex(nextIndex); next == null; next = gen.ContainerFromIndex(nextIndex))
                {
                    SetVerticalOffset(VerticalOffset - 1.0);
                    base.UpdateLayout();
                }
            }
            else
            {
                if (itemIndex == 0)
                {
                    return;
                }
                for (next = gen.ContainerFromIndex(itemIndex - 1); next == null; next = gen.ContainerFromIndex(itemIndex - 1))
                {
                    SetHorizontalOffset(HorizontalOffset - 1.0);
                    base.UpdateLayout();
                }
            }
            while (depth != 0)
            {
                next = VisualTreeHelper.GetChild(next, 0);
                depth--;
            }
            (next as UIElement).Focus();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    NavigateLeft();
                    e.Handled = true;
                    break;
                case Key.Up:
                    NavigateUp();
                    e.Handled = true;
                    break;
                case Key.Right:
                    NavigateRight();
                    e.Handled = true;
                    break;
                case Key.Down:
                    NavigateDown();
                    e.Handled = true;
                    break;
                default:
                    base.OnKeyDown(e);
                    break;
            }
        }
        protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
        {
            base.OnItemsChanged(sender, args);
            _abstractPanel = null;
            ResetScrollInfo();
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.SizeChanged += new SizeChangedEventHandler(Resizing);
            base.OnInitialized(e);
            _itemsControl = ItemsControl.GetItemsOwner(this);
            _children = base.InternalChildren;
            _generator = base.ItemContainerGenerator;
        }
        protected override Size MeasureOverride(Size availableSize)
        {
            Size result;
            if (_itemsControl == null || _itemsControl.Items.Count == 0)
            {
                result = availableSize;
            }
            else
            {
                if (_abstractPanel == null)
                {
                    _abstractPanel = new VirtualizingWrapPanel.WrapPanelAbstraction(_itemsControl.Items.Count);
                }
                _pixelMeasuredViewport = availableSize;
                _realizedChildLayout.Clear();
                Size realizedFrameSize = availableSize;
                int itemCount = _itemsControl.Items.Count;
                int firstVisibleIndex = GetFirstVisibleIndex();
                GeneratorPosition startPos = _generator.GeneratorPositionFromIndex(firstVisibleIndex);
                int childIndex = (startPos.Offset == 0) ? startPos.Index : (startPos.Index + 1);
                int current = firstVisibleIndex;
                int visibleSections = 1;
                using (_generator.StartAt(startPos, GeneratorDirection.Forward, true))
                {
                    bool stop = false;
                    bool isHorizontal = Orientation == Orientation.Horizontal;
                    double currentX = 0.0;
                    double currentY = 0.0;
                    double maxItemSize = 0.0;
                    int currentSection = GetFirstVisibleSection();
                    while (current < itemCount)
                    {
                        bool newlyRealized;
                        UIElement child = _generator.GenerateNext(out newlyRealized) as UIElement;
                        if (newlyRealized)
                        {
                            if (childIndex >= _children.Count)
                            {
                                base.AddInternalChild(child);
                            }
                            else
                            {
                                base.InsertInternalChild(childIndex, child);
                            }
                            _generator.PrepareItemContainer(child);
                            child.Measure(ChildSlotSize);
                        }
                        else
                        {
                            Debug.Assert(child == _children[childIndex], "Wrong child was generated");
                        }
                        childSize = child.DesiredSize;
                        Rect childRect = new Rect(new Point(currentX, currentY), childSize);
                        if (isHorizontal)
                        {
                            maxItemSize = Math.Max(maxItemSize, childRect.Height);
                            if (childRect.Right > realizedFrameSize.Width)
                            {
                                currentY += maxItemSize;
                                currentX = 0.0;
                                maxItemSize = childRect.Height;
                                childRect.X = currentX;
                                childRect.Y = currentY;
                                currentSection++;
                                visibleSections++;
                            }
                            if (currentY > realizedFrameSize.Height)
                            {
                                stop = true;
                            }
                            currentX = childRect.Right;
                        }
                        else
                        {
                            maxItemSize = Math.Max(maxItemSize, childRect.Width);
                            if (childRect.Bottom > realizedFrameSize.Height)
                            {
                                currentX += maxItemSize;
                                currentY = 0.0;
                                maxItemSize = childRect.Width;
                                childRect.X = currentX;
                                childRect.Y = currentY;
                                currentSection++;
                                visibleSections++;
                            }
                            if (currentX > realizedFrameSize.Width)
                            {
                                stop = true;
                            }
                            currentY = childRect.Bottom;
                        }
                        _realizedChildLayout.Add(child, childRect);
                        _abstractPanel.SetItemSection(current, currentSection);
                        if (stop)
                        {
                            break;
                        }
                        current++;
                        childIndex++;
                    }
                }
                CleanUpItems(firstVisibleIndex, current - 1);
                ComputeExtentAndViewport(availableSize, visibleSections);
                result = availableSize;
            }
            return result;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_children != null)
            {
                foreach (UIElement child in _children)
                {
                    Rect layoutInfo = _realizedChildLayout[child];
                    child.Arrange(layoutInfo);
                }
            }
            return finalSize;
        }
        public void LineDown()
        {
            if (Orientation == Orientation.Vertical)
            {
                SetVerticalOffset(VerticalOffset + 20.0);
            }
            else
            {
                SetVerticalOffset(VerticalOffset + 1.0);
            }
        }
        public void LineLeft()
        {
            if (Orientation == Orientation.Horizontal)
            {
                SetHorizontalOffset(HorizontalOffset - 20.0);
            }
            else
            {
                SetHorizontalOffset(HorizontalOffset - 1.0);
            }
        }
        public void LineRight()
        {
            if (Orientation == Orientation.Horizontal)
            {
                SetHorizontalOffset(HorizontalOffset + 20.0);
            }
            else
            {
                SetHorizontalOffset(HorizontalOffset + 1.0);
            }
        }
        public void LineUp()
        {
            if (Orientation == Orientation.Vertical)
            {
                SetVerticalOffset(VerticalOffset - 20.0);
            }
            else
            {
                SetVerticalOffset(VerticalOffset - 1.0);
            }
        }
        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            ItemContainerGenerator gen = _generator.GetItemContainerGeneratorForPanel(this);
            UIElement element = (UIElement)visual;
            int itemIndex;
            for (itemIndex = gen.IndexFromContainer(element); itemIndex == -1; itemIndex = gen.IndexFromContainer(element))
            {
                element = (UIElement)VisualTreeHelper.GetParent(element);
            }
            int section = _abstractPanel[itemIndex].Section;
            Rect elementRect = _realizedChildLayout[element];
            if (Orientation == Orientation.Horizontal)
            {
                double viewportHeight = _pixelMeasuredViewport.Height;
                if (elementRect.Bottom > viewportHeight)
                {
                    _offset.Y = _offset.Y + 1.0;
                }
                else
                {
                    if (elementRect.Top < 0.0)
                    {
                        _offset.Y = _offset.Y - 1.0;
                    }
                }
            }
            else
            {
                double viewportWidth = _pixelMeasuredViewport.Width;
                if (elementRect.Right > viewportWidth)
                {
                    _offset.X = _offset.X + 1.0;
                }
                else
                {
                    if (elementRect.Left < 0.0)
                    {
                        _offset.X = _offset.X - 1.0;
                    }
                }
            }
            base.InvalidateMeasure();
            return elementRect;
        }
        public void MouseWheelDown()
        {
            PageDown();
        }
        public void MouseWheelLeft()
        {
            PageLeft();
        }
        public void MouseWheelRight()
        {
            PageRight();
        }
        public void MouseWheelUp()
        {
            PageUp();
        }
        public void PageDown()
        {
            SetVerticalOffset(VerticalOffset + _viewport.Height * 0.8);
        }
        public void PageLeft()
        {
            SetHorizontalOffset(HorizontalOffset - _viewport.Width * 0.8);
        }
        public void PageRight()
        {
            SetHorizontalOffset(HorizontalOffset + _viewport.Width * 0.8);
        }
        public void PageUp()
        {
            SetVerticalOffset(VerticalOffset - _viewport.Height * 0.8);
        }
        public void SetHorizontalOffset(double offset)
        {
            if (offset < 0.0 || _viewport.Width >= _extent.Width)
            {
                offset = 0.0;
            }
            else
            {
                if (offset + _viewport.Width >= _extent.Width)
                {
                    offset = _extent.Width - _viewport.Width;
                }
            }
            _offset.X = offset;
            if (_owner != null)
            {
                _owner.InvalidateScrollInfo();
            }
            base.InvalidateMeasure();
            firstIndex = GetFirstVisibleIndex();
        }
        public void SetVerticalOffset(double offset)
        {
            if (offset < 0.0 || _viewport.Height >= _extent.Height)
            {
                offset = 0.0;
            }
            else
            {
                if (offset + _viewport.Height >= _extent.Height)
                {
                    offset = _extent.Height - _viewport.Height;
                }
            }
            _offset.Y = offset;
            if (_owner != null)
            {
                _owner.InvalidateScrollInfo();
            }
            base.InvalidateMeasure();
            firstIndex = GetFirstVisibleIndex();
        }
    }
}
