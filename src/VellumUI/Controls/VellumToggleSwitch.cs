using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public class ZenToggleSwitch : ToggleButton
    {
        static ZenToggleSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenToggleSwitch), new FrameworkPropertyMetadata(typeof(ZenToggleSwitch)));
        }

        public static readonly DependencyProperty OnContentProperty =
            DependencyProperty.Register(nameof(OnContent), typeof(object), typeof(ZenToggleSwitch), new PropertyMetadata("ON"));

        public object? OnContent
        {
            get => GetValue(OnContentProperty);
            set => SetValue(OnContentProperty, value);
        }

        public static readonly DependencyProperty OffContentProperty =
            DependencyProperty.Register(nameof(OffContent), typeof(object), typeof(ZenToggleSwitch), new PropertyMetadata("OFF"));

        public object? OffContent
        {
            get => GetValue(OffContentProperty);
            set => SetValue(OffContentProperty, value);
        }

        public static readonly DependencyProperty AccentBrushProperty =
            DependencyProperty.Register(nameof(AccentBrush), typeof(Brush), typeof(ZenToggleSwitch), new PropertyMetadata(null));

        public Brush? AccentBrush
        {
            get => (Brush?)GetValue(AccentBrushProperty);
            set => SetValue(AccentBrushProperty, value);
        }
    }
}
