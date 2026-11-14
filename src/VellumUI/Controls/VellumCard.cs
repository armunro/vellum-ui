using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public class ZenCard : HeaderedContentControl
    {
        static ZenCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenCard), new FrameworkPropertyMetadata(typeof(ZenCard)));
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ZenCard),
                new FrameworkPropertyMetadata(new CornerRadius(2), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register(nameof(Subtitle), typeof(string), typeof(ZenCard), new PropertyMetadata(null));

        public string? Subtitle
        {
            get => (string?)GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }

        public static readonly DependencyProperty HeaderActionProperty =
            DependencyProperty.Register(nameof(HeaderAction), typeof(object), typeof(ZenCard), new PropertyMetadata(null));

        public object? HeaderAction
        {
            get => GetValue(HeaderActionProperty);
            set => SetValue(HeaderActionProperty, value);
        }

        public static readonly DependencyProperty HighlightBrushProperty =
            DependencyProperty.Register(nameof(HighlightBrush), typeof(Brush), typeof(ZenCard), new PropertyMetadata(null));

        public Brush? HighlightBrush
        {
            get => (Brush?)GetValue(HighlightBrushProperty);
            set => SetValue(HighlightBrushProperty, value);
        }

        public static readonly DependencyProperty ShowTopAccentProperty =
            DependencyProperty.Register(nameof(ShowTopAccent), typeof(bool), typeof(ZenCard), new PropertyMetadata(false));

        public bool ShowTopAccent
        {
            get => (bool)GetValue(ShowTopAccentProperty);
            set => SetValue(ShowTopAccentProperty, value);
        }
    }
}
