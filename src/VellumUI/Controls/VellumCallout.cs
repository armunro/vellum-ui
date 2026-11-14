using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public enum ZenCalloutSeverity
    {
        Info,
        Success,
        Warning,
        Error
    }

    public class ZenCallout : ContentControl
    {
        static ZenCallout()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenCallout), new FrameworkPropertyMetadata(typeof(ZenCallout)));
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(ZenCallout), new PropertyMetadata(string.Empty));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty SeverityProperty =
            DependencyProperty.Register(nameof(Severity), typeof(ZenCalloutSeverity), typeof(ZenCallout), new PropertyMetadata(ZenCalloutSeverity.Info));

        public ZenCalloutSeverity Severity
        {
            get => (ZenCalloutSeverity)GetValue(SeverityProperty);
            set => SetValue(SeverityProperty, value);
        }

        public static readonly DependencyProperty IsDismissibleProperty =
            DependencyProperty.Register(nameof(IsDismissible), typeof(bool), typeof(ZenCallout), new PropertyMetadata(false));

        public bool IsDismissible
        {
            get => (bool)GetValue(IsDismissibleProperty);
            set => SetValue(IsDismissibleProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(ZenCallout), new PropertyMetadata(null));

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ZenCallout),
                new FrameworkPropertyMetadata(new CornerRadius(2), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty DismissCommandProperty =
            DependencyProperty.Register(nameof(DismissCommand), typeof(ICommand), typeof(ZenCallout), new PropertyMetadata(null));

        public ICommand DismissCommand
        {
            get => (ICommand)GetValue(DismissCommandProperty);
            set => SetValue(DismissCommandProperty, value);
        }

        public static readonly DependencyProperty ActionButtonContentProperty =
            DependencyProperty.Register(nameof(ActionButtonContent), typeof(object), typeof(ZenCallout), new PropertyMetadata(null));

        public object ActionButtonContent
        {
            get => GetValue(ActionButtonContentProperty);
            set => SetValue(ActionButtonContentProperty, value);
        }

        public static readonly DependencyProperty ActionCommandProperty =
            DependencyProperty.Register(nameof(ActionCommand), typeof(ICommand), typeof(ZenCallout), new PropertyMetadata(null));

        public ICommand ActionCommand
        {
            get => (ICommand)GetValue(ActionCommandProperty);
            set => SetValue(ActionCommandProperty, value);
        }

        public static readonly RoutedEvent DismissedEvent =
            EventManager.RegisterRoutedEvent(nameof(Dismissed), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZenCallout));

        public event RoutedEventHandler Dismissed
        {
            add => AddHandler(DismissedEvent, value);
            remove => RemoveHandler(DismissedEvent, value);
        }

        public void Dismiss()
        {
            Visibility = Visibility.Collapsed;

            if (DismissCommand?.CanExecute(this) == true)
            {
                DismissCommand.Execute(this);
            }

            RaiseEvent(new RoutedEventArgs(DismissedEvent, this));
        }
    }
}
