using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public class ZenNavItem : HeaderedItemsControl
    {
        static ZenNavItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenNavItem), new FrameworkPropertyMetadata(typeof(ZenNavItem)));
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(ZenNavItem), new PropertyMetadata(null));

        public Geometry? Icon
        {
            get => (Geometry?)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty BadgeProperty =
            DependencyProperty.Register(nameof(Badge), typeof(object), typeof(ZenNavItem), new PropertyMetadata(null));

        public object? Badge
        {
            get => GetValue(BadgeProperty);
            set => SetValue(BadgeProperty, value);
        }

        public static readonly DependencyProperty BadgeVariantProperty =
            DependencyProperty.Register(nameof(BadgeVariant), typeof(ZenBadgeVariant), typeof(ZenNavItem), new PropertyMetadata(ZenBadgeVariant.Primary));

        public ZenBadgeVariant BadgeVariant
        {
            get => (ZenBadgeVariant)GetValue(BadgeVariantProperty);
            set => SetValue(BadgeVariantProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(ZenNavItem),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsSelectedChanged));

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZenNavItem item && (bool)e.NewValue)
            {
                item.RaiseEvent(new RoutedEventArgs(SelectedEvent, item));
            }
        }

        public static readonly RoutedEvent SelectedEvent =
            EventManager.RegisterRoutedEvent(nameof(Selected), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ZenNavItem));

        public event RoutedEventHandler Selected
        {
            add => AddHandler(SelectedEvent, value);
            remove => RemoveHandler(SelectedEvent, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ZenNavItem), new PropertyMetadata(null));

        public ICommand? Command
        {
            get => (ICommand?)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(ZenNavItem), new PropertyMetadata(null));

        public object? CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(ZenNavItem), new PropertyMetadata(false));

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public static readonly DependencyProperty IsCompactProperty =
            DependencyProperty.Register(nameof(IsCompact), typeof(bool), typeof(ZenNavItem), new PropertyMetadata(false));

        public bool IsCompact
        {
            get => (bool)GetValue(IsCompactProperty);
            set => SetValue(IsCompactProperty, value);
        }

        public static readonly DependencyProperty NavigationKeyProperty =
            DependencyProperty.Register(nameof(NavigationKey), typeof(string), typeof(ZenNavItem), new PropertyMetadata(null));

        public string? NavigationKey
        {
            get => (string?)GetValue(NavigationKeyProperty);
            set => SetValue(NavigationKeyProperty, value);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (!e.Handled)
            {
                if (HasItems)
                {
                    IsExpanded = !IsExpanded;
                }
                IsSelected = true;
                if (Command != null && Command.CanExecute(CommandParameter))
                {
                    Command.Execute(CommandParameter);
                }
                e.Handled = true;
            }
        }
    }

    public class ZenNavSection : HeaderedContentControl
    {
        static ZenNavSection()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenNavSection), new FrameworkPropertyMetadata(typeof(ZenNavSection)));
        }

        public static readonly DependencyProperty IsCompactProperty =
            DependencyProperty.Register(nameof(IsCompact), typeof(bool), typeof(ZenNavSection), new PropertyMetadata(false));

        public bool IsCompact
        {
            get => (bool)GetValue(IsCompactProperty);
            set => SetValue(IsCompactProperty, value);
        }
    }

    public class ZenLeftNav : HeaderedItemsControl
    {
        static ZenLeftNav()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenLeftNav), new FrameworkPropertyMetadata(typeof(ZenLeftNav)));
        }

        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register(nameof(Footer), typeof(object), typeof(ZenLeftNav), new PropertyMetadata(null));

        public object? Footer
        {
            get => GetValue(FooterProperty);
            set => SetValue(FooterProperty, value);
        }

        public static readonly DependencyProperty IsCompactProperty =
            DependencyProperty.Register(nameof(IsCompact), typeof(bool), typeof(ZenLeftNav),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsCompact
        {
            get => (bool)GetValue(IsCompactProperty);
            set => SetValue(IsCompactProperty, value);
        }

        public static readonly DependencyProperty CompactWidthProperty =
            DependencyProperty.Register(nameof(CompactWidth), typeof(double), typeof(ZenLeftNav), new PropertyMetadata(48.0));

        public double CompactWidth
        {
            get => (double)GetValue(CompactWidthProperty);
            set => SetValue(CompactWidthProperty, value);
        }

        public static readonly DependencyProperty ExpandedWidthProperty =
            DependencyProperty.Register(nameof(ExpandedWidth), typeof(double), typeof(ZenLeftNav), new PropertyMetadata(220.0));

        public double ExpandedWidth
        {
            get => (double)GetValue(ExpandedWidthProperty);
            set => SetValue(ExpandedWidthProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(ZenLeftNav),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        public object? SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZenLeftNav nav)
            {
                nav.UpdateSelection(e.NewValue);
                nav.RaiseEvent(new RoutedPropertyChangedEventArgs<object?>(e.OldValue, e.NewValue, SelectedItemChangedEvent));
            }
        }

        public static readonly RoutedEvent SelectedItemChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(SelectedItemChanged), RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<object?>), typeof(ZenLeftNav));

        public event RoutedPropertyChangedEventHandler<object?> SelectedItemChanged
        {
            add => AddHandler(SelectedItemChangedEvent, value);
            remove => RemoveHandler(SelectedItemChangedEvent, value);
        }

        public ZenLeftNav()
        {
            AddHandler(ZenNavItem.SelectedEvent, new RoutedEventHandler(OnChildItemSelected));
        }

        private void OnChildItemSelected(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is ZenNavItem item)
            {
                SelectedItem = item;
                UpdateSelection(item);
            }
        }

        private void UpdateSelection(object? selected)
        {
            UnselectAllItems(this, selected);
        }

        private static void UnselectAllItems(ItemsControl parent, object? selected)
        {
            foreach (var item in parent.Items)
            {
                if (item is ZenNavItem navItem)
                {
                    if (navItem != selected && navItem.DataContext != selected)
                    {
                        navItem.IsSelected = false;
                    }
                    else
                    {
                        navItem.IsSelected = true;
                    }
                    UnselectAllItems(navItem, selected);
                }
                else
                {
                    var container = parent.ItemContainerGenerator.ContainerFromItem(item) as ZenNavItem;
                    if (container != null)
                    {
                        container.IsSelected = (item == selected);
                        UnselectAllItems(container, selected);
                    }
                }
            }
        }
    }
}
