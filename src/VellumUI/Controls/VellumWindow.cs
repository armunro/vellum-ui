using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public class ZenWindow : Window
    {
        private const int WM_GETMINMAXINFO = 0x0024;
        private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;
        private const int SM_CXSIZEFRAME = 32;
        private const int SM_CYSIZEFRAME = 33;
        private const int SM_CXPADDEDBORDER = 92;

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public int dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        static ZenWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenWindow), new FrameworkPropertyMetadata(typeof(ZenWindow)));
        }

        public static readonly DependencyProperty TitleBarContentProperty =
            DependencyProperty.Register(nameof(TitleBarContent), typeof(object), typeof(ZenWindow), new PropertyMetadata(null));

        public object? TitleBarContent
        {
            get => GetValue(TitleBarContentProperty);
            set => SetValue(TitleBarContentProperty, value);
        }

        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register(nameof(TitleBarHeight), typeof(double), typeof(ZenWindow), new PropertyMetadata(36.0));

        public double TitleBarHeight
        {
            get => (double)GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }

        public static readonly DependencyProperty ShowTitleBarProperty =
            DependencyProperty.Register(nameof(ShowTitleBar), typeof(bool), typeof(ZenWindow), new PropertyMetadata(true));

        public bool ShowTitleBar
        {
            get => (bool)GetValue(ShowTitleBarProperty);
            set => SetValue(ShowTitleBarProperty, value);
        }

        public static readonly DependencyProperty WindowIconProperty =
            DependencyProperty.Register(nameof(WindowIcon), typeof(Geometry), typeof(ZenWindow), new PropertyMetadata(null));

        public Geometry? WindowIcon
        {
            get => (Geometry?)GetValue(WindowIconProperty);
            set => SetValue(WindowIconProperty, value);
        }

        public ZenWindow()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (s, e) => SystemCommands.CloseWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (s, e) => SystemCommands.MinimizeWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (s, e) => SystemCommands.MaximizeWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (s, e) => SystemCommands.RestoreWindow(this)));
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if (PresentationSource.FromVisual(this) is HwndSource source)
            {
                source.AddHook(WindowProc);
            }
        }

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_GETMINMAXINFO)
            {
                WmGetMinMaxInfo(hwnd, lParam);
                handled = true;
            }
            return IntPtr.Zero;
        }

        private Border? _windowRoot;

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = Marshal.PtrToStructure<MINMAXINFO>(lParam);
            var hMonitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (hMonitor != IntPtr.Zero)
            {
                var monitorInfo = new MONITORINFO { cbSize = Marshal.SizeOf<MONITORINFO>() };
                if (GetMonitorInfo(hMonitor, ref monitorInfo))
                {
                    var rcWork = monitorInfo.rcWork;
                    var rcMonitor = monitorInfo.rcMonitor;
                    mmi.ptMaxPosition.x = Math.Abs(rcWork.left - rcMonitor.left);
                    mmi.ptMaxPosition.y = Math.Abs(rcWork.top - rcMonitor.top);
                    mmi.ptMaxSize.x = Math.Abs(rcWork.right - rcWork.left);
                    mmi.ptMaxSize.y = Math.Abs(rcWork.bottom - rcWork.top);
                    mmi.ptMaxTrackSize.x = mmi.ptMaxSize.x;
                    mmi.ptMaxTrackSize.y = mmi.ptMaxSize.y;
                }
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _windowRoot = GetTemplateChild("PART_WindowRoot") as Border;
            UpdateLayoutForWindowState();

            if (GetTemplateChild("PART_TitleBar") is FrameworkElement titleBar)
            {
                titleBar.MouseLeftButtonDown += OnTitleBarMouseLeftButtonDown;
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            UpdateLayoutForWindowState();
        }

        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            base.OnDpiChanged(oldDpi, newDpi);
            UpdateLayoutForWindowState();
        }

        private void UpdateLayoutForWindowState()
        {
            if (_windowRoot == null) return;

            if (WindowState == WindowState.Maximized)
            {
                var dpi = VisualTreeHelper.GetDpi(this);
                double dpiX = dpi.DpiScaleX > 0 ? dpi.DpiScaleX : 1.0;
                double dpiY = dpi.DpiScaleY > 0 ? dpi.DpiScaleY : 1.0;

                int frameX = GetSystemMetrics(SM_CXSIZEFRAME) + GetSystemMetrics(SM_CXPADDEDBORDER);
                int frameY = GetSystemMetrics(SM_CYSIZEFRAME) + GetSystemMetrics(SM_CXPADDEDBORDER);

                if (frameX <= 0) frameX = 8;
                if (frameY <= 0) frameY = 8;

                double dipX = frameX / dpiX;
                double dipY = frameY / dpiY;

                _windowRoot.Margin = new Thickness(dipX, dipY, dipX, dipY);
                _windowRoot.BorderThickness = new Thickness(0);
            }
            else
            {
                _windowRoot.Margin = new Thickness(0);
                _windowRoot.BorderThickness = BorderThickness;
            }
        }

        private void OnTitleBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject dep)
            {
                if (FindVisualParent<Button>(dep) != null ||
                    FindVisualParent<TextBox>(dep) != null ||
                    FindVisualParent<MenuItem>(dep) != null ||
                    FindVisualParent<ZenTopMenuItem>(dep) != null)
                {
                    return;
                }
            }

            if (e.ClickCount == 2 && ResizeMode != ResizeMode.NoResize && ResizeMode != ResizeMode.CanMinimize)
            {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                e.Handled = true;
                return;
            }

            if (e.ButtonState == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Maximized)
                {
                    var mousePosOnScreen = PointToScreen(e.GetPosition(this));
                    var mousePosInWindow = e.GetPosition(this);
                    var ratioX = mousePosInWindow.X / Math.Max(1.0, ActualWidth);

                    WindowState = WindowState.Normal;

                    Left = Math.Max(0, mousePosOnScreen.X - (ActualWidth * ratioX));
                    Top = Math.Max(0, mousePosOnScreen.Y - mousePosInWindow.Y);
                }

                try
                {
                    DragMove();
                }
                catch
                {
                    // DragMove may fail if mouse button was already released
                }
            }
        }

        private static T? FindVisualParent<T>(DependencyObject? child) where T : DependencyObject
        {
            while (child != null)
            {
                if (child is T parent)
                    return parent;
                if (child is Visual || child is System.Windows.Media.Media3D.Visual3D)
                    child = VisualTreeHelper.GetParent(child);
                else
                    break;
            }
            return null;
        }
    }
}
