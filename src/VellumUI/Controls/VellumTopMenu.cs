using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public class ZenTopMenuItem : MenuItem
    {
        static ZenTopMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenTopMenuItem), new FrameworkPropertyMetadata(typeof(ZenTopMenuItem)));
        }

        public static readonly DependencyProperty IconGeometryProperty =
            DependencyProperty.Register(nameof(IconGeometry), typeof(Geometry), typeof(ZenTopMenuItem), new PropertyMetadata(null));

        public Geometry? IconGeometry
        {
            get => (Geometry?)GetValue(IconGeometryProperty);
            set => SetValue(IconGeometryProperty, value);
        }

        public static readonly DependencyProperty BadgeProperty =
            DependencyProperty.Register(nameof(Badge), typeof(object), typeof(ZenTopMenuItem), new PropertyMetadata(null));

        public object? Badge
        {
            get => GetValue(BadgeProperty);
            set => SetValue(BadgeProperty, value);
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(ZenTopMenuItem), new PropertyMetadata(false));

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }
    }

    public class ZenTopMenu : Menu
    {
        static ZenTopMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenTopMenu), new FrameworkPropertyMetadata(typeof(ZenTopMenu)));
        }

        public static readonly DependencyProperty BrandContentProperty =
            DependencyProperty.Register(nameof(BrandContent), typeof(object), typeof(ZenTopMenu), new PropertyMetadata(null));

        public object? BrandContent
        {
            get => GetValue(BrandContentProperty);
            set => SetValue(BrandContentProperty, value);
        }

        public static readonly DependencyProperty ActionsContentProperty =
            DependencyProperty.Register(nameof(ActionsContent), typeof(object), typeof(ZenTopMenu), new PropertyMetadata(null));

        public object? ActionsContent
        {
            get => GetValue(ActionsContentProperty);
            set => SetValue(ActionsContentProperty, value);
        }

        public static readonly DependencyProperty RightContentProperty =
            DependencyProperty.Register(nameof(RightContent), typeof(object), typeof(ZenTopMenu), new PropertyMetadata(null));

        public object? RightContent
        {
            get => GetValue(RightContentProperty);
            set => SetValue(RightContentProperty, value);
        }
    }
}
