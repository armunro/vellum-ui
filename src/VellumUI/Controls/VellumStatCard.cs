using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public enum ZenTrendDirection
    {
        None,
        Up,
        Down,
        Neutral
    }

    public class ZenStatCard : ContentControl
    {
        static ZenStatCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenStatCard), new FrameworkPropertyMetadata(typeof(ZenStatCard)));
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(ZenStatCard), new PropertyMetadata(string.Empty));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(string), typeof(ZenStatCard), new PropertyMetadata(string.Empty));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register(nameof(Unit), typeof(string), typeof(ZenStatCard), new PropertyMetadata(string.Empty));

        public string Unit
        {
            get => (string)GetValue(UnitProperty);
            set => SetValue(UnitProperty, value);
        }

        public static readonly DependencyProperty TrendTextProperty =
            DependencyProperty.Register(nameof(TrendText), typeof(string), typeof(ZenStatCard), new PropertyMetadata(string.Empty));

        public string TrendText
        {
            get => (string)GetValue(TrendTextProperty);
            set => SetValue(TrendTextProperty, value);
        }

        public static readonly DependencyProperty TrendDirectionProperty =
            DependencyProperty.Register(nameof(TrendDirection), typeof(ZenTrendDirection), typeof(ZenStatCard), new PropertyMetadata(ZenTrendDirection.None));

        public ZenTrendDirection TrendDirection
        {
            get => (ZenTrendDirection)GetValue(TrendDirectionProperty);
            set => SetValue(TrendDirectionProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(ZenStatCard), new PropertyMetadata(string.Empty));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(ZenStatCard), new PropertyMetadata(null));

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty AccentBrushProperty =
            DependencyProperty.Register(nameof(AccentBrush), typeof(Brush), typeof(ZenStatCard), new PropertyMetadata(null));

        public Brush AccentBrush
        {
            get => (Brush)GetValue(AccentBrushProperty);
            set => SetValue(AccentBrushProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ZenStatCard),
                new FrameworkPropertyMetadata(new CornerRadius(2), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty ShowTopAccentProperty =
            DependencyProperty.Register(nameof(ShowTopAccent), typeof(bool), typeof(ZenStatCard), new PropertyMetadata(false));

        public bool ShowTopAccent
        {
            get => (bool)GetValue(ShowTopAccentProperty);
            set => SetValue(ShowTopAccentProperty, value);
        }
    }
}
