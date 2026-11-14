using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public enum ZenBadgeVariant
    {
        Primary,
        Secondary,
        Success,
        Warning,
        Danger,
        Info,
        Muted,
        Outline
    }

    public class ZenBadge : ContentControl
    {
        static ZenBadge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenBadge), new FrameworkPropertyMetadata(typeof(ZenBadge)));
        }

        public static readonly DependencyProperty VariantProperty =
            DependencyProperty.Register(nameof(Variant), typeof(ZenBadgeVariant), typeof(ZenBadge), new PropertyMetadata(ZenBadgeVariant.Primary));

        public ZenBadgeVariant Variant
        {
            get => (ZenBadgeVariant)GetValue(VariantProperty);
            set => SetValue(VariantProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ZenBadge),
                new FrameworkPropertyMetadata(new CornerRadius(2), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
