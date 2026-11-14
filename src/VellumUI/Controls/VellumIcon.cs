using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public class ZenIcon : Control
    {
        static ZenIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenIcon), new FrameworkPropertyMetadata(typeof(ZenIcon)));
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(Geometry), typeof(ZenIcon), new PropertyMetadata(null));

        public Geometry? Data
        {
            get => (Geometry?)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(double), typeof(ZenIcon), new PropertyMetadata(14.0));

        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(ZenIcon), new PropertyMetadata(null));

        public Brush? Fill
        {
            get => (Brush?)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }
    }
}
