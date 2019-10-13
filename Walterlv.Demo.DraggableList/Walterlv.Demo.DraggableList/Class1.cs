using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace Sakuno.UserInterface.Controls
{
    public class AdvancedTabHeaderItemsControl : ItemsControl
    {
        public static readonly DependencyProperty DisableTabReorderProperty = DependencyProperty.Register(nameof(DisableTabReorder), typeof(bool), typeof(AdvancedTabHeaderItemsControl), new UIPropertyMetadata(BooleanUtil.False));
        public bool DisableTabReorder
        {
            get { return (bool)GetValue(DisableTabReorderProperty); }
            set { SetValue(DisableTabReorderProperty, value); }
        }

        public static readonly DependencyProperty LockLayoutProperty = DependencyProperty.Register(nameof(LockLayout), typeof(bool), typeof(AdvancedTabHeaderItemsControl), new UIPropertyMetadata(BooleanUtil.False));
        public bool LockLayout
        {
            get { return (bool)GetValue(LockLayoutProperty); }
            set { SetValue(LockLayoutProperty, value); }
        }

        internal AdvancedTabControl Owner { get; set; }

        Dictionary<AdvancedTabItem, ItemPositionInfo> r_OriginalPositionInfoOfSiblingItems;

        static AdvancedTabHeaderItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedTabHeaderItemsControl), new FrameworkPropertyMetadata(typeof(AdvancedTabHeaderItemsControl)));

            EventManager.RegisterClassHandler(typeof(AdvancedTabHeaderItemsControl), AdvancedTabItem.DragStartedEvent, new AdvancedTabDragStartedEventHandler((s, e) => ((AdvancedTabHeaderItemsControl)s).OnItemDragStarted(e)));
            EventManager.RegisterClassHandler(typeof(AdvancedTabHeaderItemsControl), AdvancedTabItem.DragDeltaEvent, new AdvancedTabDragDeltaEventHandler((s, e) => ((AdvancedTabHeaderItemsControl)s).OnItemDragDelta(e)));
            EventManager.RegisterClassHandler(typeof(AdvancedTabHeaderItemsControl), AdvancedTabItem.DragCompletedEvent, new AdvancedTabDragCompletedEventHandler((s, e) => ((AdvancedTabHeaderItemsControl)s).OnItemDragCompleted(e)));
        }

        public AdvancedTabHeaderItemsControl()
        {
            ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        protected override DependencyObject GetContainerForItemOverride() => new AdvancedTabItem();
        protected override bool IsItemItsOwnContainerOverride(object rpItem) => rpItem is AdvancedTabItem;
        protected override void ClearContainerForItemOverride(DependencyObject rpElement, object rpItem)
        {
            base.ClearContainerForItemOverride(rpElement, rpItem);

            Dispatcher.BeginInvoke(new Action(InvalidateMeasure));
        }

        internal IEnumerable<AdvancedTabItem> GetItems()
        {
            var rItemCount = ItemContainerGenerator.Items.Count;
            for (var i = 0; i < rItemCount; i++)
            {
                var rItem = ItemContainerGenerator.ContainerFromIndex(i) as AdvancedTabItem;
                if (rItem != null)
                    yield return rItem;
            }
        }

        void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                Dispatcher.BeginInvoke(new Action(InvalidateMeasure));
        }
        protected override Size MeasureOverride(Size rpConstraint)
        {
            var rWidth = .0;
            var rHeight = .0;

            foreach (var rItem in GetItems())
            {
                rItem.Measure(SizeUtil.Infinity);
                rItem.SetCurrentValue(AdvancedTabItem.LeftProperty, rWidth);

                rWidth += rItem.DesiredSize.Width;
                rHeight = Math.Max(rHeight, rItem.DesiredSize.Height);
            }

            var rFinalWidth = rpConstraint.Width.IsInfinity() ? rWidth : rpConstraint.Width;
            var rFinalHeight = rpConstraint.Height.IsInfinity() ? rHeight : rpConstraint.Height;

            return new Size(rFinalWidth, rFinalHeight);
        }

        void OnItemDragStarted(AdvancedTabDragStartedEventArgs e)
        {
            if (DisableTabReorder || LockLayout)
                return;

            r_OriginalPositionInfoOfSiblingItems = GetItems().Except(new[] { e.Item }).Select(r => new ItemPositionInfo(r)).ToDictionary(r => r.Item);

            e.Handled = true;
        }
        void OnItemDragDelta(AdvancedTabDragDeltaEventArgs e)
        {
            if (r_OriginalPositionInfoOfSiblingItems == null)
                return;

            e.Item.SetCurrentValue(AdvancedTabItem.LeftProperty, Math.Max(e.Item.Left + e.DragEventArgs.HorizontalChange, -2.0));

            var rItems = r_OriginalPositionInfoOfSiblingItems.Values.Union(new[] { new ItemPositionInfo(e.Item) }).OrderBy(r => r.Item == e.Item ? r.Position : r_OriginalPositionInfoOfSiblingItems[r.Item].Position).Select(r => r.Item);

            var rPosition = .0;
            foreach (var rItem in rItems)
            {
                if (rItem != e.Item && rItem.Left != rPosition)
                    SetItemPosition(rItem, rPosition);
                rPosition += rItem.DesiredSize.Width;

                Panel.SetZIndex(rItem, 0);
            }

            Panel.SetZIndex(e.Item, int.MaxValue);
        }
        void OnItemDragCompleted(AdvancedTabDragCompletedEventArgs e)
        {
            if (r_OriginalPositionInfoOfSiblingItems == null)
                return;

            var rItems = r_OriginalPositionInfoOfSiblingItems.Values.Union(new[] { new ItemPositionInfo(e.Item) }).OrderBy(r => r.Item == e.Item ? r.Position : r_OriginalPositionInfoOfSiblingItems[r.Item].Position).Select(r => r.Item).ToArray();

            var rPosition = .0;
            foreach (var rItem in rItems)
            {
                if (rItem.Left != rPosition)
                    SetItemPosition(rItem, rPosition);
                rPosition += rItem.DesiredSize.Width;
            }

            e.Handled = true;
        }

        void SetItemPosition(AdvancedTabItem rpItem, double rpPosition)
        {
            var rAnimation = new DoubleAnimation(rpItem.Left, rpPosition, new Duration(TimeSpan.FromMilliseconds(200.0)), FillBehavior.Stop) { EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut } };
            rAnimation.WhenComplete(() => rpItem.SetCurrentValue(AdvancedTabItem.LeftProperty, rpPosition));

            rpItem.BeginAnimation(AdvancedTabItem.LeftProperty, rAnimation);
        }

        struct ItemPositionInfo
        {
            public AdvancedTabItem Item { get; }

            public double Position { get; }

            public ItemPositionInfo(AdvancedTabItem rpItem)
            {
                Item = rpItem;

                Position = rpItem.Left;
            }
        }
    }
}