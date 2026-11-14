using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Xunit;
using VellumUI.Controls;
using VellumUI.Demo.Views;

namespace VellumUi.Tests
{
    public class ThemeAndControlsTests
    {
        private static readonly System.Windows.Threading.Dispatcher s_staDispatcher;

        static ThemeAndControlsTests()
        {
            var tcs = new System.Threading.Tasks.TaskCompletionSource<System.Windows.Threading.Dispatcher>();
            var thread = new Thread(() =>
            {
                if (Application.Current == null)
                {
                    var app = new Application
                    {
                        ShutdownMode = ShutdownMode.OnExplicitShutdown
                    };
                }

                if (Application.Current != null && !Application.Current.Resources.MergedDictionaries.Any(d => d.Source?.OriginalString.Contains("ZenTheme.xaml") == true))
                {
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                    {
                        Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                    });
                }

                tcs.SetResult(System.Windows.Threading.Dispatcher.CurrentDispatcher);
                System.Windows.Threading.Dispatcher.Run();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();

            s_staDispatcher = tcs.Task.GetAwaiter().GetResult();
        }

        private void RunInSta(Action action)
        {
            s_staDispatcher.Invoke(action);
        }

        [Fact]
        public void Test_ZenTheme_LoadsResourceDictionariesAndColors()
        {
            RunInSta(() =>
            {
                var dict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/Colors.xaml", UriKind.Absolute)
                };

                Assert.NotNull(dict);
                Assert.True(dict.Contains("Zen.Color.Background.Canvas"));
                Assert.True(dict.Contains("Zen.Color.Background.App"));
                Assert.True(dict.Contains("Zen.Color.Background.Surface"));
                Assert.True(dict.Contains("Zen.Color.Primary"));
                Assert.True(dict.Contains("Zen.Color.Secondary"));
                Assert.True(dict.Contains("Zen.Color.Foreground.Primary"));
                Assert.True(dict.Contains("Zen.Color.Foreground.VibrantCyan"));

                // Verify Dark Gray palette
                var appBg = (Color)dict["Zen.Color.Background.App"];
                Assert.Equal(0x18, appBg.R);
                Assert.Equal(0x18, appBg.G);
                Assert.Equal(0x1B, appBg.B);

                // Verify Primary Cyan palette
                var primary = (Color)dict["Zen.Color.Primary"];
                Assert.Equal(0x0E, primary.R);
                Assert.Equal(0xA5, primary.G);
                Assert.Equal(0xE9, primary.B);

                // Verify Secondary Violet palette
                var secondary = (Color)dict["Zen.Color.Secondary"];
                Assert.Equal(0x8B, secondary.R);
                Assert.Equal(0x5C, secondary.G);
                Assert.Equal(0xF6, secondary.B);
            });
        }

        [Fact]
        public void Test_ZenTheme_MinimalMarginsAndBorderRadiuses()
        {
            RunInSta(() =>
            {
                var dict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/Dimensions.xaml", UriKind.Absolute)
                };

                Assert.NotNull(dict);
                Assert.True(dict.Contains("Zen.CornerRadius.Default"));
                Assert.True(dict.Contains("Zen.Padding.Button"));
                Assert.True(dict.Contains("Zen.Padding.Default"));

                var cornerRadius = (CornerRadius)dict["Zen.CornerRadius.Default"];
                Assert.True(cornerRadius.TopLeft <= 2.0, "Border radius should be minimal (<= 2px)");
                Assert.Equal(2.0, cornerRadius.TopLeft);

                var btnPadding = (Thickness)dict["Zen.Padding.Button"];
                Assert.True(btnPadding.Top <= 4.0, "Button vertical padding should be minimal for density");
            });
        }

        [Fact]
        public void Test_ZenBadge_TemplateApplication()
        {
            RunInSta(() =>
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                var badgeStyle = themeDict[typeof(ZenBadge)] as Style;
                Assert.NotNull(badgeStyle);

                foreach (var variant in Enum.GetValues<ZenBadgeVariant>())
                {
                    var badge = new ZenBadge
                    {
                        Content = $"Badge {variant}",
                        Variant = variant,
                        Style = badgeStyle
                    };
                    badge.Resources.MergedDictionaries.Add(themeDict);
                    badge.ApplyTemplate();
                    badge.Measure(new Size(100, 100));
                    badge.Arrange(new Rect(0, 0, 100, 100));
                    badge.UpdateLayout();
                    Assert.True(badge.DesiredSize.Width > 0);
                }
            });
        }

        [Fact]
        public void Test_ZenCard_PropertiesAndDefaults()
        {
            RunInSta(() =>
            {
                var card = new ZenCard
                {
                    Header = "SYSTEM STATUS",
                    Subtitle = "Cluster Alpha",
                    ShowTopAccent = true
                };

                Assert.Equal("SYSTEM STATUS", card.Header);
                Assert.Equal("Cluster Alpha", card.Subtitle);
                Assert.True(card.ShowTopAccent);
                Assert.Equal(new CornerRadius(2), card.CornerRadius);
            });
        }

        [Fact]
        public void Test_ZenToggleSwitch_StateTransitions()
        {
            RunInSta(() =>
            {
                var toggle = new ZenToggleSwitch();
                Assert.False(toggle.IsChecked);
                Assert.Equal("ON", toggle.OnContent);
                Assert.Equal("OFF", toggle.OffContent);

                toggle.IsChecked = true;
                Assert.True(toggle.IsChecked);
            });
        }

        [Fact]
        public void Test_ZenSearchBox_ClearCommand()
        {
            RunInSta(() =>
            {
                var search = new ZenSearchBox
                {
                    Text = "Sample Search Query"
                };

                Assert.Equal("Sample Search Query", search.Text);
                Assert.NotNull(search.ClearCommand);
                Assert.True(search.ClearCommand.CanExecute(null));

                search.ClearCommand.Execute(null);
                Assert.Equal(string.Empty, search.Text);

                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };
                search.Resources.MergedDictionaries.Add(themeDict);
                search.Style = (Style)themeDict[typeof(ZenSearchBox)];
                search.ApplyTemplate();
                search.Measure(new Size(200, 32));
                search.Arrange(new Rect(0, 0, 200, 32));
                search.UpdateLayout();
                Assert.True(search.DesiredSize.Width > 0);
            });
        }

        [Fact]
        public void Test_ZenLeftNav_ItemSelection()
        {
            RunInSta(() =>
            {
                var nav = new ZenLeftNav();
                var item1 = new ZenNavItem { Header = "Dashboard" };
                var item2 = new ZenNavItem { Header = "Settings" };
                nav.Items.Add(item1);
                nav.Items.Add(item2);

                Assert.False(nav.IsCompact);
                Assert.Equal(48.0, nav.CompactWidth);
                Assert.Equal(220.0, nav.ExpandedWidth);

                item1.IsSelected = true;
                Assert.True(item1.IsSelected);
            });
        }

        [Fact]
        public void Test_ZenTopMenu_Properties()
        {
            RunInSta(() =>
            {
                var topMenu = new ZenTopMenu();
                var topItem = new ZenTopMenuItem { Header = "File", IsActive = true };
                topMenu.Items.Add(topItem);

                Assert.Single(topMenu.Items);
                Assert.True(topItem.IsActive);
            });
        }

        [Fact]
        public void Test_ZenStatusIndicator_Defaults()
        {
            RunInSta(() =>
            {
                var indicator = new ZenStatusIndicator();
                Assert.Equal(ZenStatusType.Online, indicator.Status);
                Assert.Equal(8.0, indicator.Size);
                Assert.False(indicator.IsPulsing);

                indicator.Status = ZenStatusType.Busy;
                indicator.IsPulsing = true;
                Assert.Equal(ZenStatusType.Busy, indicator.Status);
                Assert.True(indicator.IsPulsing);
            });
        }

        [Fact]
        public void Test_DataGrid_InitializationWithZenTheme()
        {
            RunInSta(() =>
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                var grid = new DataGrid();
                grid.Resources.MergedDictionaries.Add(themeDict);
                grid.Style = (Style)themeDict[typeof(DataGrid)];
                grid.BeginInit();
                grid.EndInit();

                Assert.NotNull(grid.AlternatingRowBackground);
                Assert.NotNull(grid.RowHeaderStyle);
                Assert.NotNull(grid.ColumnHeaderStyle);
                Assert.Equal(20.0, grid.RowHeaderWidth);

                Assert.NotNull(themeDict[typeof(DataGridRowHeader)]);
                Assert.NotNull(themeDict["Zen.Style.DataGridRowHeader"]);

                var rowHeader = new DataGridRowHeader();
                rowHeader.Resources.MergedDictionaries.Add(themeDict);
                rowHeader.Style = (Style)themeDict[typeof(DataGridRowHeader)];
                rowHeader.ApplyTemplate();
                Assert.NotNull(rowHeader.Style);
            });
        }

        [Fact]
        public void Test_ProgressBar_SlimAndBarGraphStyles()
        {
            RunInSta(() =>
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                // Standard / Slim ProgressBar
                Assert.NotNull(themeDict[typeof(ProgressBar)]);
                Assert.NotNull(themeDict["Zen.Style.ProgressBar"]);
                Assert.NotNull(themeDict["Zen.Style.ProgressBar.Slim"]);

                var slimProgress = new ProgressBar
                {
                    Value = 45,
                    Maximum = 100
                };
                slimProgress.Resources.MergedDictionaries.Add(themeDict);
                slimProgress.Style = (Style)themeDict["Zen.Style.ProgressBar.Slim"];
                slimProgress.ApplyTemplate();
                slimProgress.Measure(new Size(200, 10));
                slimProgress.Arrange(new Rect(0, 0, 200, 10));
                slimProgress.UpdateLayout();
                Assert.NotNull(slimProgress.Style);

                // BarGraph / Segmented ProgressBar Style
                Assert.NotNull(themeDict["Zen.Style.ProgressBar.BarGraph"]);
                Assert.NotNull(themeDict["Zen.Style.ProgressBar.Segmented"]);

                var barGraphProgress = new ProgressBar
                {
                    Value = 75,
                    Maximum = 100,
                    Style = (Style)themeDict["Zen.Style.ProgressBar.BarGraph"]
                };
                barGraphProgress.Resources.MergedDictionaries.Add(themeDict);
                barGraphProgress.ApplyTemplate();
                barGraphProgress.Measure(new Size(200, 16));
                barGraphProgress.Arrange(new Rect(0, 0, 200, 16));
                barGraphProgress.UpdateLayout();
                Assert.NotNull(barGraphProgress.Style);

                // ZenProgressBar Control
                Assert.NotNull(themeDict[typeof(ZenProgressBar)]);
                Assert.NotNull(themeDict["Zen.Style.ZenProgressBar"]);

                var zenBar = new ZenProgressBar
                {
                    Value = 60,
                    Maximum = 100,
                    ProgressStyle = ZenProgressStyle.BarGraph
                };
                zenBar.Resources.MergedDictionaries.Add(themeDict);
                zenBar.Style = (Style)themeDict[typeof(ZenProgressBar)];
                zenBar.ApplyTemplate();
                zenBar.Measure(new Size(200, 16));
                zenBar.Arrange(new Rect(0, 0, 200, 16));
                zenBar.UpdateLayout();
                Assert.Equal(ZenProgressStyle.BarGraph, zenBar.ProgressStyle);

                // Indeterminate ProgressBar Testing
                var indeterminateProgress = new ProgressBar
                {
                    IsIndeterminate = true,
                    Style = (Style)themeDict["Zen.Style.ProgressBar"]
                };
                indeterminateProgress.Resources.MergedDictionaries.Add(themeDict);
                indeterminateProgress.ApplyTemplate();
                indeterminateProgress.Measure(new Size(200, 6));
                indeterminateProgress.Arrange(new Rect(0, 0, 200, 6));
                indeterminateProgress.UpdateLayout();
                Assert.True(indeterminateProgress.IsIndeterminate);

                var indeterminateBarGraph = new ProgressBar
                {
                    IsIndeterminate = true,
                    Style = (Style)themeDict["Zen.Style.ProgressBar.BarGraph"]
                };
                indeterminateBarGraph.Resources.MergedDictionaries.Add(themeDict);
                indeterminateBarGraph.ApplyTemplate();
                indeterminateBarGraph.Measure(new Size(200, 14));
                indeterminateBarGraph.Arrange(new Rect(0, 0, 200, 14));
                indeterminateBarGraph.UpdateLayout();
                Assert.True(indeterminateBarGraph.IsIndeterminate);
            });
        }

        [Fact]
        public void Test_Slider_StyleAndCustomForegroundTrack()
        {
            RunInSta(() =>
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                Assert.NotNull(themeDict[typeof(Slider)]);
                Assert.NotNull(themeDict["Zen.Style.Slider"]);
                Assert.NotNull(themeDict["Zen.Brush.Secondary"]);

                var violetBrush = (Brush)themeDict["Zen.Brush.Secondary"];

                var slider = new Slider
                {
                    Value = 85,
                    Minimum = 0,
                    Maximum = 100,
                    Foreground = violetBrush
                };
                slider.Resources.MergedDictionaries.Add(themeDict);
                slider.Style = (Style)themeDict["Zen.Style.Slider"];
                slider.ApplyTemplate();
                slider.Measure(new Size(200, 24));
                slider.Arrange(new Rect(0, 0, 200, 24));
                slider.UpdateLayout();

                Assert.Equal(violetBrush, slider.Foreground);
                Assert.True(slider.DesiredSize.Height > 0);
            });
        }

        [Fact]
        public void Test_Typography_TextBlockStyles()
        {
            RunInSta(() =>
            {
                var typoDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/Typography.xaml", UriKind.Absolute)
                };

                Assert.NotNull(typoDict);
                Assert.NotNull(typoDict["Zen.Style.TextBlock.Body"]);
                Assert.NotNull(typoDict["Zen.Style.TextBlock.Secondary"]);
                Assert.NotNull(typoDict["Zen.Style.TextBlock.Heading"]);

                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                Assert.NotNull(themeDict[typeof(TextBlock)]);

                var textBlock = new TextBlock
                {
                    Style = (Style)typoDict["Zen.Style.TextBlock.Body"],
                    Text = "Test Body Text"
                };

                Assert.NotNull(textBlock.Style);
            });
        }

        [Fact]
        public void Test_DemoViews_InitializationWithZenTheme()
        {
            RunInSta(() =>
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                var views = new (string Name, Func<FrameworkElement> Factory)[]
                {
                    ("Dashboard", () => new DashboardView()),
                    ("Buttons", () => new ButtonsView()),
                    ("Inputs", () => new InputsView()),
                    ("DataTables", () => new DataTablesView()),
                    ("Navigation", () => new NavigationDemoView()),
                    ("Cards", () => new CardsContainersView()),
                    ("DesignSystem", () => new DesignSystemView()),
                    ("EmailClient", () => new EmailClientView()),
                    ("MainWindow", () => new VellumUI.Demo.MainWindow())
                };

                foreach (var (name, factory) in views)
                {
                    var view = factory();
                    Assert.NotNull(view);
                    view.Resources.MergedDictionaries.Add(themeDict);
                    view.Measure(new Size(1200, 800));
                    view.Arrange(new Rect(0, 0, 1200, 800));
                    view.UpdateLayout();
                }

                // Dispatch pending layout and render operations
                var frame = new System.Windows.Threading.DispatcherFrame();
                System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Background,
                    new Action(() => frame.Continue = false));
                System.Windows.Threading.Dispatcher.PushFrame(frame);
            });
        }

        [Fact]
        public void Test_MainWindow_InitialViewState()
        {
            RunInSta(() =>
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                var window = new VellumUI.Demo.MainWindow();
                window.Resources.MergedDictionaries.Add(themeDict);
                window.Show();
                window.Measure(new Size(1180, 740));
                window.Arrange(new Rect(0, 0, 1180, 740));
                window.UpdateLayout();

                System.Console.WriteLine($"[DEBUG_LOG] window.Content: {window.Content?.GetType().FullName ?? "null"}");
                System.Console.WriteLine($"[DEBUG_LOG] VisualChildrenCount: {VisualTreeHelper.GetChildrenCount(window)}");
                DumpVisualTree(window, 0);

                var vm = window.DataContext as VellumUI.Demo.ViewModels.MainViewModel;
                Assert.NotNull(vm);
                Assert.Equal("Dashboard", vm.SelectedSection);

                var sections = new[] { "Dashboard", "Buttons", "Inputs", "DataTables", "NavigationDemo", "CardsContainers", "DesignSystem", "EmailClient" };
                foreach (var sectionName in sections)
                {
                    vm.SelectedSection = sectionName;
                    window.Measure(new Size(1180, 740));
                    window.Arrange(new Rect(0, 0, 1180, 740));
                    window.UpdateLayout();
                }
                vm.SelectedSection = "Dashboard";
                window.UpdateLayout();

                // Find DashboardView in visual tree
                var dashboard = FindVisualChild<DashboardView>(window);
                Assert.NotNull(dashboard);
                Assert.Equal(Visibility.Visible, dashboard.Visibility);

                // Navigate to EmailClient section and verify EmailClientView is Visible and Dashboard is Collapsed
                vm.NavigateCommand.Execute("EmailClient");
                window.UpdateLayout();
                var emailView = FindVisualChild<EmailClientView>(window);
                Assert.NotNull(emailView);
                Assert.Equal(Visibility.Visible, emailView.Visibility);
                Assert.Equal(Visibility.Collapsed, dashboard.Visibility);

                // Switch back to Dashboard
                vm.NavigateCommand.Execute("Dashboard");
                window.UpdateLayout();
                Assert.Equal(Visibility.Visible, dashboard.Visibility);
                Assert.Equal(Visibility.Collapsed, emailView.Visibility);

                // Test Window Maximized and Restore transitions
                window.WindowState = WindowState.Maximized;
                Assert.Equal(WindowState.Maximized, window.WindowState);
                var windowRoot = window.Template.FindName("PART_WindowRoot", window) as Border;
                Assert.NotNull(windowRoot);
                Assert.True(windowRoot.Margin.Left > 0);
                Assert.True(windowRoot.Margin.Top > 0);
                Assert.Equal(new Thickness(0), windowRoot.BorderThickness);

                window.WindowState = WindowState.Normal;
                Assert.Equal(WindowState.Normal, window.WindowState);
                Assert.Equal(new Thickness(0), windowRoot.Margin);
                Assert.Equal(window.BorderThickness, windowRoot.BorderThickness);

                window.Close();
            });
        }

        [Fact]
        public void Test_ZenNav_VisualHierarchyAndSections()
        {
            RunInSta(() =>
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                var nav = new ZenLeftNav();
                nav.Resources.MergedDictionaries.Add(themeDict);

                var section = new ZenNavSection { Header = "TEST SECTION" };
                section.Resources.MergedDictionaries.Add(themeDict);

                var item1 = new ZenNavItem { Header = "Item 1", IsSelected = true };
                var item2 = new ZenNavItem { Header = "Item 2" };
                item1.Resources.MergedDictionaries.Add(themeDict);
                item2.Resources.MergedDictionaries.Add(themeDict);

                nav.Items.Add(section);
                nav.Items.Add(item1);
                nav.Items.Add(item2);

                nav.ApplyTemplate();
                nav.Measure(new Size(200, 400));
                nav.Arrange(new Rect(0, 0, 200, 400));
                nav.UpdateLayout();

                Assert.True(item1.IsSelected);
                Assert.False(item2.IsSelected);
                Assert.Equal("TEST SECTION", section.Header);

                // Test compact mode
                nav.IsCompact = true;
                nav.Measure(new Size(48, 400));
                nav.Arrange(new Rect(0, 0, 48, 400));
                nav.UpdateLayout();
                Assert.True(nav.IsCompact);
            });
        }

        [Fact]
        public void Test_ZenNavSection_And_NavItem_CompactProperties()
        {
            RunInSta(() =>
            {
                var section = new ZenNavSection { Header = "GROUP 1", IsCompact = true };
                Assert.True(section.IsCompact);
                Assert.Equal("GROUP 1", section.Header);

                var item = new ZenNavItem { Header = "Home", IsCompact = true };
                Assert.True(item.IsCompact);
                Assert.Equal("Home", item.Header);
            });
        }

        [Fact]
        public void Test_ZenTag_PropertiesAndEvents()
        {
            RunInSta(() =>
            {
                var tag = new ZenTag
                {
                    Content = "Kubernetes",
                    IsSelectable = true,
                    IsRemovable = true,
                    IsSelected = false
                };

                Assert.Equal("Kubernetes", tag.Content);
                Assert.True(tag.IsSelectable);
                Assert.True(tag.IsRemovable);
                Assert.False(tag.IsSelected);

                bool closedEventFired = false;
                tag.Closed += (s, e) => closedEventFired = true;

                tag.Close();
                Assert.True(closedEventFired);

                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };
                tag.Resources.MergedDictionaries.Add(themeDict);
                tag.Style = (Style)themeDict[typeof(ZenTag)];
                tag.ApplyTemplate();
                tag.Measure(new Size(120, 24));
                tag.Arrange(new Rect(0, 0, 120, 24));
                tag.UpdateLayout();
                Assert.True(tag.DesiredSize.Width > 0);
            });
        }

        [Fact]
        public void Test_ZenSegmentedControl_ItemsAndSelection()
        {
            RunInSta(() =>
            {
                var control = new ZenSegmentedControl();
                var item1 = new ZenSegmentedItem { Content = "Day", IsChecked = true };
                var item2 = new ZenSegmentedItem { Content = "Week", BadgeText = "7" };
                var item3 = new ZenSegmentedItem { Content = "Month" };

                control.Items.Add(item1);
                control.Items.Add(item2);
                control.Items.Add(item3);

                Assert.Equal(3, control.Items.Count);
                Assert.True(item1.IsChecked);
                Assert.Equal("7", item2.BadgeText);

                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };
                control.Resources.MergedDictionaries.Add(themeDict);
                item1.Resources.MergedDictionaries.Add(themeDict);
                item2.Resources.MergedDictionaries.Add(themeDict);
                item3.Resources.MergedDictionaries.Add(themeDict);

                control.ApplyTemplate();
                control.Measure(new Size(200, 32));
                control.Arrange(new Rect(0, 0, 200, 32));
                control.UpdateLayout();
                Assert.True(control.DesiredSize.Width > 0);
            });
        }

        [Fact]
        public void Test_ZenStatCard_PropertiesAndLayout()
        {
            RunInSta(() =>
            {
                var stat = new ZenStatCard
                {
                    Title = "CPU LOAD",
                    Value = "42.5",
                    Unit = "%",
                    TrendText = "+3.1%",
                    TrendDirection = ZenTrendDirection.Up,
                    Description = "peak workload",
                    ShowTopAccent = true
                };

                Assert.Equal("CPU LOAD", stat.Title);
                Assert.Equal("42.5", stat.Value);
                Assert.Equal("%", stat.Unit);
                Assert.Equal("+3.1%", stat.TrendText);
                Assert.Equal(ZenTrendDirection.Up, stat.TrendDirection);
                Assert.True(stat.ShowTopAccent);

                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };
                stat.Resources.MergedDictionaries.Add(themeDict);
                stat.Style = (Style)themeDict[typeof(ZenStatCard)];
                stat.ApplyTemplate();
                stat.Measure(new Size(200, 100));
                stat.Arrange(new Rect(0, 0, 200, 100));
                stat.UpdateLayout();
                Assert.True(stat.DesiredSize.Height > 0);
            });
        }

        [Fact]
        public void Test_ZenCallout_PropertiesAndDismiss()
        {
            RunInSta(() =>
            {
                var callout = new ZenCallout
                {
                    Title = "Maintenance Notice",
                    Content = "Cluster updates in progress",
                    Severity = ZenCalloutSeverity.Warning,
                    IsDismissible = true
                };

                Assert.Equal("Maintenance Notice", callout.Title);
                Assert.Equal("Cluster updates in progress", callout.Content);
                Assert.Equal(ZenCalloutSeverity.Warning, callout.Severity);
                Assert.True(callout.IsDismissible);
                Assert.Equal(Visibility.Visible, callout.Visibility);

                bool dismissedEventFired = false;
                callout.Dismissed += (s, e) => dismissedEventFired = true;

                callout.Dismiss();
                Assert.True(dismissedEventFired);
                Assert.Equal(Visibility.Collapsed, callout.Visibility);

                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };
                callout.Resources.MergedDictionaries.Add(themeDict);
                callout.Style = (Style)themeDict[typeof(ZenCallout)];
                callout.ApplyTemplate();
                callout.Measure(new Size(300, 60));
                callout.Arrange(new Rect(0, 0, 300, 60));
                callout.UpdateLayout();
            });
        }

        [Fact]
        public void Test_Controls_CornerRadius_And_TemplateInstantiation()
        {
            RunInSta(() =>
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                // ZenCard
                var card = new ZenCard { Header = "Card Header", Content = "Card Content" };
                card.Resources.MergedDictionaries.Add(themeDict);
                card.Style = (Style)themeDict[typeof(ZenCard)];
                card.ApplyTemplate();
                card.Measure(new Size(200, 100));
                card.Arrange(new Rect(0, 0, 200, 100));
                card.UpdateLayout();
                Assert.Equal(new CornerRadius(2), card.CornerRadius);

                // ZenBadge
                var badge = new ZenBadge { Content = "TEST" };
                badge.Resources.MergedDictionaries.Add(themeDict);
                badge.Style = (Style)themeDict[typeof(ZenBadge)];
                badge.ApplyTemplate();
                badge.Measure(new Size(100, 30));
                badge.Arrange(new Rect(0, 0, 100, 30));
                badge.UpdateLayout();
                Assert.Equal(new CornerRadius(2), badge.CornerRadius);

                // ZenTag
                var tag = new ZenTag { Content = "Tag Content" };
                tag.Resources.MergedDictionaries.Add(themeDict);
                tag.Style = (Style)themeDict[typeof(ZenTag)];
                tag.ApplyTemplate();
                tag.Measure(new Size(100, 30));
                tag.Arrange(new Rect(0, 0, 100, 30));
                tag.UpdateLayout();
                Assert.Equal(new CornerRadius(2), tag.CornerRadius);

                // ZenSegmentedControl & ZenSegmentedItem
                var segCtrl = new ZenSegmentedControl();
                var segItem = new ZenSegmentedItem { Content = "Item 1" };
                segCtrl.Items.Add(segItem);
                segCtrl.Resources.MergedDictionaries.Add(themeDict);
                segItem.Resources.MergedDictionaries.Add(themeDict);
                segCtrl.Style = (Style)themeDict[typeof(ZenSegmentedControl)];
                segItem.Style = (Style)themeDict[typeof(ZenSegmentedItem)];
                segCtrl.ApplyTemplate();
                segItem.ApplyTemplate();
                segCtrl.Measure(new Size(200, 40));
                segCtrl.Arrange(new Rect(0, 0, 200, 40));
                segCtrl.UpdateLayout();
                Assert.Equal(new CornerRadius(2), segCtrl.CornerRadius);
                Assert.Equal(new CornerRadius(2), segItem.CornerRadius);

                // ZenStatCard
                var statCard = new ZenStatCard { Title = "STAT", Value = "100" };
                statCard.Resources.MergedDictionaries.Add(themeDict);
                statCard.Style = (Style)themeDict[typeof(ZenStatCard)];
                statCard.ApplyTemplate();
                statCard.Measure(new Size(200, 100));
                statCard.Arrange(new Rect(0, 0, 200, 100));
                statCard.UpdateLayout();
                Assert.Equal(new CornerRadius(2), statCard.CornerRadius);

                // ZenCallout
                var callout = new ZenCallout { Title = "Callout Title" };
                callout.Resources.MergedDictionaries.Add(themeDict);
                callout.Style = (Style)themeDict[typeof(ZenCallout)];
                callout.ApplyTemplate();
                callout.Measure(new Size(300, 60));
                callout.Arrange(new Rect(0, 0, 300, 60));
                callout.UpdateLayout();
                Assert.Equal(new CornerRadius(2), callout.CornerRadius);

                // ZenTabControl & ZenTabPage
                var tabCtrl = new ZenTabControl();
                var tabPage = new ZenTabPage { Header = "Page 1", Content = "Page Content", Icon = Geometry.Parse("M0,0 L10,10"), BadgeText = "NEW", IsClosable = true };
                tabCtrl.Items.Add(tabPage);
                tabCtrl.Resources.MergedDictionaries.Add(themeDict);
                tabPage.Resources.MergedDictionaries.Add(themeDict);
                tabCtrl.Style = (Style)themeDict[typeof(ZenTabControl)];
                tabPage.Style = (Style)themeDict[typeof(ZenTabPage)];
                tabCtrl.ApplyTemplate();
                tabPage.ApplyTemplate();
                tabCtrl.Measure(new Size(400, 200));
                tabCtrl.Arrange(new Rect(0, 0, 400, 200));
                tabCtrl.UpdateLayout();
                Assert.Equal(new CornerRadius(2), tabCtrl.CornerRadius);
                Assert.Equal(new CornerRadius(2), tabPage.CornerRadius);
            });
        }

        [Fact]
        public void Test_ZenTabControl_PropertiesAndStyles()
        {
            RunInSta(() =>
            {
                var tabControl = new ZenTabControl
                {
                    TabStyle = ZenTabStyle.Pill,
                    ShowAddButton = true,
                    AddTabToolTip = "Add New Workspace"
                };

                Assert.Equal(ZenTabStyle.Pill, tabControl.TabStyle);
                Assert.True(tabControl.ShowAddButton);
                Assert.Equal("Add New Workspace", tabControl.AddTabToolTip);
                Assert.Equal(new CornerRadius(2), tabControl.CornerRadius);

                var page1 = new ZenTabPage
                {
                    Header = "Overview",
                    BadgeText = "Active",
                    IsClosable = false
                };

                var page2 = new ZenTabItem
                {
                    Header = "Logs",
                    BadgeText = "12",
                    IsClosable = true
                };

                tabControl.Items.Add(page1);
                tabControl.Items.Add(page2);

                Assert.Equal(2, tabControl.Items.Count);
                Assert.False(page1.IsClosable);
                Assert.True(page2.IsClosable);
                Assert.Equal("Active", page1.BadgeText);
                Assert.Equal("12", page2.BadgeText);
            });
        }

        [Fact]
        public void Test_ZenTabPage_CloseCommandAndEvent()
        {
            RunInSta(() =>
            {
                var tabControl = new ZenTabControl();
                bool customCommandExecuted = false;
                bool closedEventRaised = false;

                var customCloseCommand = new RelayCommandTest(_ => customCommandExecuted = true);

                var page = new ZenTabPage
                {
                    Header = "Terminal",
                    IsClosable = true,
                    CloseCommand = customCloseCommand
                };

                page.Closed += (s, e) => closedEventRaised = true;

                tabControl.Items.Add(page);
                Assert.Single(tabControl.Items);

                page.Close();

                Assert.True(customCommandExecuted);
                Assert.True(closedEventRaised);
                Assert.Empty(tabControl.Items);
            });
        }

        [Fact]
        public void Test_ZenTabPage_CloseRequested_Cancellation()
        {
            RunInSta(() =>
            {
                var tabControl = new ZenTabControl();
                bool closedEventRaised = false;

                var page = new ZenTabPage
                {
                    Header = "Unsaved Document",
                    IsClosable = true
                };

                page.CloseRequested += (s, e) =>
                {
                    e.Cancel = true; // cancel closing
                };

                page.Closed += (s, e) => closedEventRaised = true;

                tabControl.Items.Add(page);
                page.Close();

                Assert.False(closedEventRaised);
                Assert.Single(tabControl.Items);
            });
        }

        [Fact]
        public void Test_ZenDatePicker_PropertiesAndTemplate()
        {
            RunInSta(() =>
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                var datePicker = new ZenDatePicker
                {
                    Placeholder = "Pick date...",
                    CornerRadius = new CornerRadius(3),
                    SelectedDate = new DateTime(2026, 11, 12)
                };
                datePicker.Resources.MergedDictionaries.Add(themeDict);
                datePicker.Style = (Style)themeDict[typeof(ZenDatePicker)];

                Assert.Equal("Pick date...", datePicker.Placeholder);
                Assert.Equal(new CornerRadius(3), datePicker.CornerRadius);
                Assert.Equal(new DateTime(2026, 11, 12), datePicker.SelectedDate);

                datePicker.ApplyTemplate();
                datePicker.Measure(new Size(200, 32));
                datePicker.Arrange(new Rect(0, 0, 200, 32));
                datePicker.UpdateLayout();

                Assert.True(datePicker.DesiredSize.Width > 0);
                Assert.True(datePicker.DesiredSize.Height > 0);
            });
        }

        [Fact]
        public void Test_Calendar_StylingAndSelection()
        {
            RunInSta(() =>
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                var calendar = new Calendar
                {
                    SelectedDate = new DateTime(2026, 11, 12)
                };
                calendar.Resources.MergedDictionaries.Add(themeDict);
                calendar.Style = (Style)themeDict[typeof(Calendar)];

                Assert.Equal(new DateTime(2026, 11, 12), calendar.SelectedDate);

                calendar.ApplyTemplate();
                calendar.Measure(new Size(220, 220));
                calendar.Arrange(new Rect(0, 0, 220, 220));
                calendar.UpdateLayout();

                Assert.True(calendar.DesiredSize.Width > 0);
            });
        }

        [Fact]
        public void Test_ZenColorPicker_RgbAndHexSync()
        {
            RunInSta(() =>
            {
                var colorPicker = new ZenColorPicker();

                // Initial default check (Cyan #0EA5E9)
                Assert.Equal(Color.FromRgb(14, 165, 233), colorPicker.SelectedColor);
                Assert.Equal("#0EA5E9", colorPicker.HexCode);
                Assert.Equal(14, colorPicker.R);
                Assert.Equal(165, colorPicker.G);
                Assert.Equal(233, colorPicker.B);
                Assert.Equal(255, colorPicker.A);

                // Change via SelectedColor
                colorPicker.SelectedColor = Color.FromArgb(255, 244, 63, 94); // Rose
                Assert.Equal("#F43F5E", colorPicker.HexCode);
                Assert.Equal(244, colorPicker.R);
                Assert.Equal(63, colorPicker.G);
                Assert.Equal(94, colorPicker.B);

                // Change via HexCode
                colorPicker.HexCode = "#10B981"; // Emerald
                Assert.Equal(Color.FromRgb(16, 185, 129), colorPicker.SelectedColor);
                Assert.Equal(16, colorPicker.R);
                Assert.Equal(185, colorPicker.G);
                Assert.Equal(129, colorPicker.B);

                // Change via RGB
                colorPicker.R = 139;
                colorPicker.G = 92;
                colorPicker.B = 246; // Violet
                Assert.Equal(Color.FromRgb(139, 92, 246), colorPicker.SelectedColor);
                Assert.Equal("#8B5CF6", colorPicker.HexCode);
            });
        }

        [Fact]
        public void Test_ZenColorPicker_PaletteSelectionAndModes()
        {
            RunInSta(() =>
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                var colorPicker = new ZenColorPicker
                {
                    DisplayMode = ZenColorPickerMode.Inline
                };
                colorPicker.Resources.MergedDictionaries.Add(themeDict);
                colorPicker.Style = (Style)themeDict[typeof(ZenColorPicker)];

                Assert.NotNull(colorPicker.PaletteColors);
                Assert.NotEmpty(colorPicker.PaletteColors);

                // Select from Palette via Command
                var targetColor = Color.FromRgb(245, 158, 11); // Amber
                Assert.True(colorPicker.SelectColorCommand.CanExecute(targetColor));
                colorPicker.SelectColorCommand.Execute(targetColor);

                Assert.Equal(targetColor, colorPicker.SelectedColor);
                Assert.Equal("#F59E0B", colorPicker.HexCode);

                colorPicker.ApplyTemplate();
                colorPicker.Measure(new Size(280, 400));
                colorPicker.Arrange(new Rect(0, 0, 280, 400));
                colorPicker.UpdateLayout();

                Assert.True(colorPicker.DesiredSize.Width > 0);
            });
        }

        [Fact]
        public void Test_EmailClient_ViewModel_And_View_Functionality()
        {
            RunInSta(() =>
            {
                var themeDict = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/VellumUI;component/Themes/ZenTheme.xaml", UriKind.Absolute)
                };

                // 1. Test ViewModel logic
                var vm = new VellumUI.Demo.ViewModels.EmailClientViewModel();
                Assert.NotEmpty(vm.Folders);
                Assert.NotEmpty(vm.AllEmails);
                Assert.NotEmpty(vm.FilteredEmails);
                Assert.NotNull(vm.SelectedEmail);
                Assert.True(vm.HasSelectedEmail);

                // Filter by unread
                vm.SelectedFilter = "Unread";
                Assert.All(vm.FilteredEmails, e => Assert.True(e.IsUnread));

                // Filter by search query
                vm.SelectedFilter = "All";
                vm.SearchQuery = "Sarah";
                Assert.Single(vm.FilteredEmails);
                Assert.Contains("Sarah Jenkins", vm.FilteredEmails[0].SenderName);

                // Star toggle
                var targetEmail = vm.FilteredEmails[0];
                bool initialStar = targetEmail.IsStarred;
                vm.ToggleStarCommand.Execute(targetEmail);
                Assert.Equal(!initialStar, targetEmail.IsStarred);

                // Send reply
                vm.SelectedEmail = targetEmail;
                vm.ReplyText = "Thanks Sarah! The architecture proposal looks great.";
                int countBefore = vm.AllEmails.Count;
                vm.SendReplyCommand.Execute(null);
                Assert.Equal(countBefore + 1, vm.AllEmails.Count);
                Assert.Empty(vm.ReplyText);
                Assert.True(vm.IsStatusBannerVisible);

                // Folder selection (Drafts)
                vm.SearchQuery = string.Empty;
                vm.SelectFolderCommand.Execute("Drafts");
                Assert.Equal("Drafts", vm.SelectedFolder?.Name);
                Assert.NotEmpty(vm.FilteredEmails);
                Assert.All(vm.FilteredEmails, e => Assert.Equal("Drafts", e.Folder));

                // Folder selection (Sent)
                vm.SelectFolderCommand.Execute("Sent");
                Assert.Equal("Sent", vm.SelectedFolder?.Name);
                Assert.NotEmpty(vm.FilteredEmails);

                // Mark all read
                vm.SelectFolderCommand.Execute("Inbox");
                vm.MarkAllReadCommand.Execute(null);
                Assert.All(vm.FilteredEmails, e => Assert.False(e.IsUnread));

                // Archive email
                var emailToArchive = vm.FilteredEmails.First();
                vm.ArchiveCommand.Execute(emailToArchive);
                Assert.Equal("Archive", emailToArchive.Folder);

                // Delete email to trash
                var emailToDelete = vm.FilteredEmails.First();
                vm.DeleteCommand.Execute(emailToDelete);
                Assert.Equal("Trash", emailToDelete.Folder);

                // 2. Test View layout and tree
                var view = new EmailClientView
                {
                    DataContext = vm
                };
                view.Resources.MergedDictionaries.Add(themeDict);
                view.Measure(new Size(1200, 800));
                view.Arrange(new Rect(0, 0, 1200, 800));
                view.UpdateLayout();

                Assert.True(view.DesiredSize.Width > 0);
                Assert.True(view.DesiredSize.Height > 0);

                var searchBox = FindVisualChild<ZenSearchBox>(view);
                Assert.NotNull(searchBox);

                var cards = new List<ZenCard>();
                void CollectCards(DependencyObject parent)
                {
                    int count = VisualTreeHelper.GetChildrenCount(parent);
                    for (int i = 0; i < count; i++)
                    {
                        var child = VisualTreeHelper.GetChild(parent, i);
                        if (child is ZenCard c) cards.Add(c);
                        CollectCards(child);
                    }
                }
                CollectCards(view);
                Assert.NotEmpty(cards);
            });
        }

        private class RelayCommandTest : System.Windows.Input.ICommand
        {
            private readonly Action<object?> _execute;
            public RelayCommandTest(Action<object?> execute) => _execute = execute;
            public bool CanExecute(object? parameter) => true;
            public void Execute(object? parameter) => _execute(parameter);
            public event EventHandler? CanExecuteChanged { add { } remove { } }
        }

        private static void DumpVisualTree(DependencyObject parent, int indent)
        {
            if (parent == null) return;
            string pad = new string(' ', indent * 2);
            System.Console.WriteLine($"[DEBUG_LOG] {pad}{parent.GetType().Name}");
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DumpVisualTree(VisualTreeHelper.GetChild(parent, i), indent + 1);
            }
        }

        private static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    return typedChild;

                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
