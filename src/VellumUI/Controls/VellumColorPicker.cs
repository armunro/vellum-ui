using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VellumUI.Controls
{
    public enum ZenColorPickerMode
    {
        DropDown,
        Inline,
        PaletteOnly
    }

    public class ZenColorPicker : Control
    {
        static ZenColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenColorPicker), new FrameworkPropertyMetadata(typeof(ZenColorPicker)));
        }

        private bool _isUpdating;

        public ZenColorPicker()
        {
            SelectColorCommand = new RelayCommand(OnSelectColor);
            ToggleDropDownCommand = new RelayCommand(_ => IsDropDownOpen = !IsDropDownOpen);
            CloseDropDownCommand = new RelayCommand(_ => IsDropDownOpen = false);

            PaletteColors = new ObservableCollection<Color>(GetDefaultPalette());
            UpdateFromColor(SelectedColor);
        }

        public ICommand SelectColorCommand { get; }
        public ICommand ToggleDropDownCommand { get; }
        public ICommand CloseDropDownCommand { get; }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(Color),
                typeof(ZenColorPicker),
                new FrameworkPropertyMetadata(
                    Color.FromRgb(14, 165, 233), // Default Cyan #0EA5E9
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender,
                    OnSelectedColorChanged));

        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register(
                nameof(SelectedBrush),
                typeof(SolidColorBrush),
                typeof(ZenColorPicker),
                new PropertyMetadata(null));

        public SolidColorBrush SelectedBrush
        {
            get => (SolidColorBrush)GetValue(SelectedBrushProperty);
            private set => SetValue(SelectedBrushProperty, value);
        }

        public static readonly DependencyProperty HexCodeProperty =
            DependencyProperty.Register(
                nameof(HexCode),
                typeof(string),
                typeof(ZenColorPicker),
                new FrameworkPropertyMetadata("#0EA5E9", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHexCodeChanged));

        public string HexCode
        {
            get => (string)GetValue(HexCodeProperty);
            set => SetValue(HexCodeProperty, value);
        }

        public static readonly DependencyProperty AProperty =
            DependencyProperty.Register(
                nameof(A),
                typeof(byte),
                typeof(ZenColorPicker),
                new FrameworkPropertyMetadata((byte)255, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRgbChanged));

        public byte A
        {
            get => (byte)GetValue(AProperty);
            set => SetValue(AProperty, value);
        }

        public static readonly DependencyProperty RProperty =
            DependencyProperty.Register(
                nameof(R),
                typeof(byte),
                typeof(ZenColorPicker),
                new FrameworkPropertyMetadata((byte)14, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRgbChanged));

        public byte R
        {
            get => (byte)GetValue(RProperty);
            set => SetValue(RProperty, value);
        }

        public static readonly DependencyProperty GProperty =
            DependencyProperty.Register(
                nameof(G),
                typeof(byte),
                typeof(ZenColorPicker),
                new FrameworkPropertyMetadata((byte)165, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRgbChanged));

        public byte G
        {
            get => (byte)GetValue(GProperty);
            set => SetValue(GProperty, value);
        }

        public static readonly DependencyProperty BProperty =
            DependencyProperty.Register(
                nameof(B),
                typeof(byte),
                typeof(ZenColorPicker),
                new FrameworkPropertyMetadata((byte)233, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRgbChanged));

        public byte B
        {
            get => (byte)GetValue(BProperty);
            set => SetValue(BProperty, value);
        }

        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register(
                nameof(Hue),
                typeof(double),
                typeof(ZenColorPicker),
                new FrameworkPropertyMetadata(198.6, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHsvChanged));

        public double Hue
        {
            get => (double)GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register(
                nameof(Saturation),
                typeof(double),
                typeof(ZenColorPicker),
                new FrameworkPropertyMetadata(94.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHsvChanged));

        public double Saturation
        {
            get => (double)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        public static readonly DependencyProperty BrightnessProperty =
            DependencyProperty.Register(
                nameof(Brightness),
                typeof(double),
                typeof(ZenColorPicker),
                new FrameworkPropertyMetadata(91.4, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHsvChanged));

        public double Brightness
        {
            get => (double)GetValue(BrightnessProperty);
            set => SetValue(BrightnessProperty, value);
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register(
                nameof(IsDropDownOpen),
                typeof(bool),
                typeof(ZenColorPicker),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register(
                nameof(DisplayMode),
                typeof(ZenColorPickerMode),
                typeof(ZenColorPicker),
                new PropertyMetadata(ZenColorPickerMode.DropDown));

        public ZenColorPickerMode DisplayMode
        {
            get => (ZenColorPickerMode)GetValue(DisplayModeProperty);
            set => SetValue(DisplayModeProperty, value);
        }

        public static readonly DependencyProperty ShowAlphaProperty =
            DependencyProperty.Register(
                nameof(ShowAlpha),
                typeof(bool),
                typeof(ZenColorPicker),
                new PropertyMetadata(true));

        public bool ShowAlpha
        {
            get => (bool)GetValue(ShowAlphaProperty);
            set => SetValue(ShowAlphaProperty, value);
        }

        public static readonly DependencyProperty ShowSlidersProperty =
            DependencyProperty.Register(
                nameof(ShowSliders),
                typeof(bool),
                typeof(ZenColorPicker),
                new PropertyMetadata(true));

        public bool ShowSliders
        {
            get => (bool)GetValue(ShowSlidersProperty);
            set => SetValue(ShowSlidersProperty, value);
        }

        public static readonly DependencyProperty ShowHexProperty =
            DependencyProperty.Register(
                nameof(ShowHex),
                typeof(bool),
                typeof(ZenColorPicker),
                new PropertyMetadata(true));

        public bool ShowHex
        {
            get => (bool)GetValue(ShowHexProperty);
            set => SetValue(ShowHexProperty, value);
        }

        public static readonly DependencyProperty ShowPaletteProperty =
            DependencyProperty.Register(
                nameof(ShowPalette),
                typeof(bool),
                typeof(ZenColorPicker),
                new PropertyMetadata(true));

        public bool ShowPalette
        {
            get => (bool)GetValue(ShowPaletteProperty);
            set => SetValue(ShowPaletteProperty, value);
        }

        public static readonly DependencyProperty ShowPreviewProperty =
            DependencyProperty.Register(
                nameof(ShowPreview),
                typeof(bool),
                typeof(ZenColorPicker),
                new PropertyMetadata(true));

        public bool ShowPreview
        {
            get => (bool)GetValue(ShowPreviewProperty);
            set => SetValue(ShowPreviewProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ZenColorPicker),
                new FrameworkPropertyMetadata(new CornerRadius(2), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty PaletteColorsProperty =
            DependencyProperty.Register(
                nameof(PaletteColors),
                typeof(ObservableCollection<Color>),
                typeof(ZenColorPicker),
                new PropertyMetadata(null));

        public ObservableCollection<Color> PaletteColors
        {
            get => (ObservableCollection<Color>)GetValue(PaletteColorsProperty);
            set => SetValue(PaletteColorsProperty, value);
        }

        public static readonly RoutedEvent SelectedColorChangedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(SelectedColorChanged),
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<Color>),
                typeof(ZenColorPicker));

        public event RoutedPropertyChangedEventHandler<Color> SelectedColorChanged
        {
            add => AddHandler(SelectedColorChangedEvent, value);
            remove => RemoveHandler(SelectedColorChangedEvent, value);
        }

        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZenColorPicker picker && !picker._isUpdating)
            {
                var oldColor = (Color)e.OldValue;
                var newColor = (Color)e.NewValue;

                picker._isUpdating = true;
                try
                {
                    picker.UpdateFromColor(newColor);
                }
                finally
                {
                    picker._isUpdating = false;
                }

                picker.RaiseEvent(new RoutedPropertyChangedEventArgs<Color>(oldColor, newColor, SelectedColorChangedEvent));
            }
        }

        private static void OnRgbChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZenColorPicker picker && !picker._isUpdating)
            {
                picker._isUpdating = true;
                try
                {
                    var color = Color.FromArgb(picker.A, picker.R, picker.G, picker.B);
                    picker.SelectedColor = color;
                    picker.UpdateBrushAndHex(color);
                    var (h, s, v) = ColorToHsv(color);
                    picker.Hue = Math.Round(h, 1);
                    picker.Saturation = Math.Round(s * 100.0, 1);
                    picker.Brightness = Math.Round(v * 100.0, 1);
                }
                finally
                {
                    picker._isUpdating = false;
                }
            }
        }

        private static void OnHsvChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZenColorPicker picker && !picker._isUpdating)
            {
                picker._isUpdating = true;
                try
                {
                    var color = HsvToColor(picker.A, picker.Hue, picker.Saturation / 100.0, picker.Brightness / 100.0);
                    picker.SelectedColor = color;
                    picker.R = color.R;
                    picker.G = color.G;
                    picker.B = color.B;
                    picker.UpdateBrushAndHex(color);
                }
                finally
                {
                    picker._isUpdating = false;
                }
            }
        }

        private static void OnHexCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZenColorPicker picker && !picker._isUpdating && e.NewValue is string hex)
            {
                if (TryParseHex(hex, out var color))
                {
                    picker._isUpdating = true;
                    try
                    {
                        picker.SelectedColor = color;
                        picker.A = color.A;
                        picker.R = color.R;
                        picker.G = color.G;
                        picker.B = color.B;
                        var brush = new SolidColorBrush(color);
                        brush.Freeze();
                        picker.SelectedBrush = brush;
                        var (h, s, v) = ColorToHsv(color);
                        picker.Hue = Math.Round(h, 1);
                        picker.Saturation = Math.Round(s * 100.0, 1);
                        picker.Brightness = Math.Round(v * 100.0, 1);
                    }
                    finally
                    {
                        picker._isUpdating = false;
                    }
                }
            }
        }

        private void UpdateFromColor(Color color)
        {
            A = color.A;
            R = color.R;
            G = color.G;
            B = color.B;

            UpdateBrushAndHex(color);

            var (h, s, v) = ColorToHsv(color);
            Hue = Math.Round(h, 1);
            Saturation = Math.Round(s * 100.0, 1);
            Brightness = Math.Round(v * 100.0, 1);
        }

        private void UpdateBrushAndHex(Color color)
        {
            var brush = new SolidColorBrush(color);
            brush.Freeze();
            SelectedBrush = brush;

            HexCode = color.A < 255
                ? $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}"
                : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        private void OnSelectColor(object? parameter)
        {
            if (parameter is Color color)
            {
                SelectedColor = color;
            }
            else if (parameter is SolidColorBrush brush)
            {
                SelectedColor = brush.Color;
            }
            else if (parameter is string hex && TryParseHex(hex, out var parsed))
            {
                SelectedColor = parsed;
            }
        }

        public static bool TryParseHex(string hex, out Color color)
        {
            color = Colors.Transparent;
            if (string.IsNullOrWhiteSpace(hex)) return false;

            try
            {
                hex = hex.Trim();
                if (!hex.StartsWith("#")) hex = "#" + hex;
                var obj = ColorConverter.ConvertFromString(hex);
                if (obj is Color c)
                {
                    color = c;
                    return true;
                }
            }
            catch { }

            return false;
        }

        public static (double h, double s, double v) ColorToHsv(Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;

            double h = 0;
            if (delta > 0.00001)
            {
                if (Math.Abs(max - r) < 0.00001)
                    h = 60.0 * (((g - b) / delta) % 6);
                else if (Math.Abs(max - g) < 0.00001)
                    h = 60.0 * (((b - r) / delta) + 2);
                else
                    h = 60.0 * (((r - g) / delta) + 4);

                if (h < 0) h += 360.0;
            }

            double s = max == 0 ? 0 : delta / max;
            double v = max;

            return (h, s, v);
        }

        public static Color HsvToColor(byte a, double h, double s, double v)
        {
            while (h < 0) h += 360;
            while (h >= 360) h -= 360;
            s = Math.Clamp(s, 0.0, 1.0);
            v = Math.Clamp(v, 0.0, 1.0);

            double c = v * s;
            double x = c * (1 - Math.Abs((h / 60.0) % 2 - 1));
            double m = v - c;

            double rPrime = 0, gPrime = 0, bPrime = 0;
            if (h < 60) { rPrime = c; gPrime = x; bPrime = 0; }
            else if (h < 120) { rPrime = x; gPrime = c; bPrime = 0; }
            else if (h < 180) { rPrime = 0; gPrime = c; bPrime = x; }
            else if (h < 240) { rPrime = 0; gPrime = x; bPrime = c; }
            else if (h < 300) { rPrime = x; gPrime = 0; bPrime = c; }
            else { rPrime = c; gPrime = 0; bPrime = x; }

            byte r = (byte)Math.Clamp(Math.Round((rPrime + m) * 255.0), 0, 255);
            byte g = (byte)Math.Clamp(Math.Round((gPrime + m) * 255.0), 0, 255);
            byte b = (byte)Math.Clamp(Math.Round((bPrime + m) * 255.0), 0, 255);

            return Color.FromArgb(a, r, g, b);
        }

        public static Color[] GetDefaultPalette()
        {
            return new[]
            {
                Color.FromRgb(14, 165, 233),   // Sky/Cyan (#0EA5E9)
                Color.FromRgb(56, 189, 248),   // Light Sky (#38BDF8)
                Color.FromRgb(20, 184, 166),   // Teal (#14B8A6)
                Color.FromRgb(16, 185, 129),   // Emerald (#10B981)
                Color.FromRgb(34, 197, 94),    // Green (#22C55E)
                Color.FromRgb(245, 158, 11),   // Amber (#F59E0B)
                Color.FromRgb(249, 115, 22),   // Orange (#F97316)
                Color.FromRgb(244, 63, 94),    // Rose (#F43F5E)
                Color.FromRgb(239, 68, 68),    // Red (#EF4444)
                Color.FromRgb(139, 92, 246),   // Violet (#8B5CF6)
                Color.FromRgb(168, 85, 247),   // Purple (#A855F7)
                Color.FromRgb(59, 130, 246),   // Blue (#3B82F6)
                Color.FromRgb(100, 116, 139),  // Slate (#64748B)
                Color.FromRgb(113, 113, 122),  // Zinc (#71717A)
                Color.FromRgb(24, 24, 27),     // Dark (#18181B)
                Color.FromRgb(244, 244, 245),  // Light (#F4F4F5)
            };
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
}
