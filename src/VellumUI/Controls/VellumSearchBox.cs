using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VellumUI.Controls
{
    public class ZenSearchBox : TextBox
    {
        static ZenSearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZenSearchBox), new FrameworkPropertyMetadata(typeof(ZenSearchBox)));
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(ZenSearchBox), new PropertyMetadata("Search..."));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty ClearCommandProperty =
            DependencyProperty.Register(nameof(ClearCommand), typeof(ICommand), typeof(ZenSearchBox), new PropertyMetadata(null));

        public ICommand? ClearCommand
        {
            get => (ICommand?)GetValue(ClearCommandProperty);
            set => SetValue(ClearCommandProperty, value);
        }

        public ZenSearchBox()
        {
            ClearCommand = new RelayCommand(_ => Text = string.Empty, _ => !string.IsNullOrEmpty(Text));
        }
    }

    public class RelayCommand : ICommand
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
