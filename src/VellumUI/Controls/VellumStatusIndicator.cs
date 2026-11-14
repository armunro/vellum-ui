using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public enum ZenStatusType
    {
        Online,
        Offline,
        Busy,
        Away,
        Warning,
        Custom
    }

    public class ZenStatusIndicator : Control
    {
        static ZenStatusIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenStatusIndicator), new FrameworkPropertyMetadata(typeof(ZenStatusIndicator)));
        }

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register(nameof(Status), typeof(ZenStatusType), typeof(ZenStatusIndicator), new PropertyMetadata(ZenStatusType.Online));

        public ZenStatusType Status
        {
            get => (ZenStatusType)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(double), typeof(ZenStatusIndicator), new PropertyMetadata(8.0));

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty IsPulsingProperty =
            DependencyProperty.Register(nameof(IsPulsing), typeof(bool), typeof(ZenStatusIndicator), new PropertyMetadata(false));

        public bool IsPulsing
        {
            get => (bool)GetValue(IsPulsingProperty);
            set => SetValue(IsPulsingProperty, value);
        }

        public static readonly DependencyProperty CustomBrushProperty =
            DependencyProperty.Register(nameof(CustomBrush), typeof(Brush), typeof(ZenStatusIndicator), new PropertyMetadata(null));

        public Brush? CustomBrush
        {
            get => (Brush?)GetValue(CustomBrushProperty);
            set => SetValue(CustomBrushProperty, value);
        }
    }
}
