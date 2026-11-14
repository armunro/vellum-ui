using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public enum ZenTabStyle
    {
        Underline,
        Pill,
        Card,
        Segmented
    }

    public class ZenTabCloseEventArgs : RoutedEventArgs
    {
        public TabItem TabItem { get; }
        public bool Cancel { get; set; }

        public ZenTabCloseEventArgs(RoutedEvent routedEvent, object source, TabItem tabItem)
            : base(routedEvent, source)
        {
            TabItem = tabItem;
        }
    }

    public delegate void ZenTabCloseEventHandler(object sender, ZenTabCloseEventArgs e);

    public class ZenTabPage : TabItem
    {
        static ZenTabPage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenTabPage), new FrameworkPropertyMetadata(typeof(ZenTabPage)));
        }

        public ZenTabPage()
        {
            InternalCloseCommand = new RelayCommand(_ => Close());
        }

        public ICommand InternalCloseCommand { get; }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(ZenTabPage), new PropertyMetadata(null));

        public Geometry? Icon
        {
            get => (Geometry?)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty BadgeTextProperty =
            DependencyProperty.Register(nameof(BadgeText), typeof(string), typeof(ZenTabPage), new PropertyMetadata(null));

        public string? BadgeText
        {
            get => (string?)GetValue(BadgeTextProperty);
            set => SetValue(BadgeTextProperty, value);
        }

        public static readonly DependencyProperty IsClosableProperty =
            DependencyProperty.Register(nameof(IsClosable), typeof(bool), typeof(ZenTabPage), new PropertyMetadata(false));

        public bool IsClosable
        {
            get => (bool)GetValue(IsClosableProperty);
            set => SetValue(IsClosableProperty, value);
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(ZenTabPage), new PropertyMetadata(null));

        public ICommand? CloseCommand
        {
            get => (ICommand?)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        public static readonly DependencyProperty CloseCommandParameterProperty =
            DependencyProperty.Register(nameof(CloseCommandParameter), typeof(object), typeof(ZenTabPage), new PropertyMetadata(null));

        public object? CloseCommandParameter
        {
            get => GetValue(CloseCommandParameterProperty);
            set => SetValue(CloseCommandParameterProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ZenTabPage),
                new FrameworkPropertyMetadata(new CornerRadius(2), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty AccentBrushProperty =
            DependencyProperty.Register(nameof(AccentBrush), typeof(Brush), typeof(ZenTabPage), new PropertyMetadata(null));

        public Brush? AccentBrush
        {
            get => (Brush?)GetValue(AccentBrushProperty);
            set => SetValue(AccentBrushProperty, value);
        }

        public static readonly RoutedEvent CloseRequestedEvent =
            EventManager.RegisterRoutedEvent(nameof(CloseRequested), RoutingStrategy.Bubble, typeof(ZenTabCloseEventHandler), typeof(ZenTabPage));

        public event ZenTabCloseEventHandler CloseRequested
        {
            add => AddHandler(CloseRequestedEvent, value);
            remove => RemoveHandler(CloseRequestedEvent, value);
        }

        public static readonly RoutedEvent ClosedEvent =
            EventManager.RegisterRoutedEvent(nameof(Closed), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZenTabPage));

        public event RoutedEventHandler Closed
        {
            add => AddHandler(ClosedEvent, value);
            remove => RemoveHandler(ClosedEvent, value);
        }

        public void Close()
        {
            var requestArgs = new ZenTabCloseEventArgs(CloseRequestedEvent, this, this);
            RaiseEvent(requestArgs);

            if (requestArgs.Cancel)
                return;

            if (CloseCommand?.CanExecute(CloseCommandParameter ?? this) == true)
            {
                CloseCommand.Execute(CloseCommandParameter ?? this);
            }

            RaiseEvent(new RoutedEventArgs(ClosedEvent, this));
        }

        private class RelayCommand : ICommand
        {
            private readonly Action<object?> _execute;
            private readonly Predicate<object?>? _canExecute;

            public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

            public void Execute(object? parameter) => _execute(parameter);

            public event EventHandler? CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }
    }

    public class ZenTabItem : ZenTabPage
    {
        static ZenTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenTabItem), new FrameworkPropertyMetadata(typeof(ZenTabItem)));
        }
    }

    public class ZenTabControl : TabControl
    {
        static ZenTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenTabControl), new FrameworkPropertyMetadata(typeof(ZenTabControl)));

            EventManager.RegisterClassHandler(typeof(ZenTabControl), ZenTabPage.ClosedEvent, new RoutedEventHandler(OnTabPageClosed));
        }

        private static void OnTabPageClosed(object sender, RoutedEventArgs e)
        {
            if (sender is ZenTabControl tabControl && e.OriginalSource is TabItem tabItem)
            {
                if (tabControl.ItemsSource == null && tabControl.Items.Contains(tabItem))
                {
                    tabControl.Items.Remove(tabItem);
                    e.Handled = true;
                }
            }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ZenTabControl),
                new FrameworkPropertyMetadata(new CornerRadius(2), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty TabStyleProperty =
            DependencyProperty.Register(
                nameof(TabStyle),
                typeof(ZenTabStyle),
                typeof(ZenTabControl),
                new PropertyMetadata(ZenTabStyle.Underline));

        public ZenTabStyle TabStyle
        {
            get => (ZenTabStyle)GetValue(TabStyleProperty);
            set => SetValue(TabStyleProperty, value);
        }

        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register(
                nameof(HeaderBackground),
                typeof(Brush),
                typeof(ZenTabControl),
                new PropertyMetadata(null));

        public Brush? HeaderBackground
        {
            get => (Brush?)GetValue(HeaderBackgroundProperty);
            set => SetValue(HeaderBackgroundProperty, value);
        }

        public static readonly DependencyProperty HeaderPaddingProperty =
            DependencyProperty.Register(
                nameof(HeaderPadding),
                typeof(Thickness),
                typeof(ZenTabControl),
                new PropertyMetadata(new Thickness(2, 0, 2, 0)));

        public Thickness HeaderPadding
        {
            get => (Thickness)GetValue(HeaderPaddingProperty);
            set => SetValue(HeaderPaddingProperty, value);
        }

        public static readonly DependencyProperty ShowAddButtonProperty =
            DependencyProperty.Register(
                nameof(ShowAddButton),
                typeof(bool),
                typeof(ZenTabControl),
                new PropertyMetadata(false));

        public bool ShowAddButton
        {
            get => (bool)GetValue(ShowAddButtonProperty);
            set => SetValue(ShowAddButtonProperty, value);
        }

        public static readonly DependencyProperty AddTabCommandProperty =
            DependencyProperty.Register(
                nameof(AddTabCommand),
                typeof(ICommand),
                typeof(ZenTabControl),
                new PropertyMetadata(null));

        public ICommand? AddTabCommand
        {
            get => (ICommand?)GetValue(AddTabCommandProperty);
            set => SetValue(AddTabCommandProperty, value);
        }

        public static readonly DependencyProperty AddTabToolTipProperty =
            DependencyProperty.Register(
                nameof(AddTabToolTip),
                typeof(string),
                typeof(ZenTabControl),
                new PropertyMetadata("New Tab"));

        public string AddTabToolTip
        {
            get => (string)GetValue(AddTabToolTipProperty);
            set => SetValue(AddTabToolTipProperty, value);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TabItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ZenTabPage();
        }
    }
}
