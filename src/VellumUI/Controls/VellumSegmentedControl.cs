using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public class ZenSegmentedItem : RadioButton
    {
        static ZenSegmentedItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenSegmentedItem), new FrameworkPropertyMetadata(typeof(ZenSegmentedItem)));
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(ZenSegmentedItem), new PropertyMetadata(null));

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ZenSegmentedItem),
                new FrameworkPropertyMetadata(new CornerRadius(2), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty BadgeTextProperty =
            DependencyProperty.Register(nameof(BadgeText), typeof(string), typeof(ZenSegmentedItem), new PropertyMetadata(null));

        public string BadgeText
        {
            get => (string)GetValue(BadgeTextProperty);
            set => SetValue(BadgeTextProperty, value);
        }
    }

    public class ZenSegmentedControl : ListBox
    {
        static ZenSegmentedControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenSegmentedControl), new FrameworkPropertyMetadata(typeof(ZenSegmentedControl)));
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ZenSegmentedControl),
                new FrameworkPropertyMetadata(new CornerRadius(2), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ZenSegmentedItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ZenSegmentedItem();
        }
    }
}
