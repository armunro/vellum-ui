using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public enum ZenProgressStyle
    {
        Slim,
        BarGraph
    }

    public class ZenProgressBar : ProgressBar
    {
        static ZenProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenProgressBar), new FrameworkPropertyMetadata(typeof(ZenProgressBar)));
        }

        public static readonly DependencyProperty ProgressStyleProperty =
            DependencyProperty.Register(nameof(ProgressStyle), typeof(ZenProgressStyle), typeof(ZenProgressBar),
                new PropertyMetadata(ZenProgressStyle.Slim));

        public ZenProgressStyle ProgressStyle
        {
            get => (ZenProgressStyle)GetValue(ProgressStyleProperty);
            set => SetValue(ProgressStyleProperty, value);
        }

        public static readonly DependencyProperty SegmentCountProperty =
            DependencyProperty.Register(nameof(SegmentCount), typeof(int), typeof(ZenProgressBar),
                new PropertyMetadata(10));

        public int SegmentCount
        {
            get => (int)GetValue(SegmentCountProperty);
            set => SetValue(SegmentCountProperty, value);
        }

        public static readonly DependencyProperty SegmentSpacingProperty =
            DependencyProperty.Register(nameof(SegmentSpacing), typeof(double), typeof(ZenProgressBar),
                new PropertyMetadata(2.0));

        public double SegmentSpacing
        {
            get => (double)GetValue(SegmentSpacingProperty);
            set => SetValue(SegmentSpacingProperty, value);
        }
    }
}
