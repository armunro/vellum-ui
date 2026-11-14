using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VellumUI.Controls;
using VellumUI.Demo.Models;

namespace VellumUI.Demo.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _selectedSection = "Dashboard";
        private bool _isSidebarCompact = false;
        private string _searchQuery = string.Empty;
        private double _sliderValue = 68.0;
        private bool _toggleState1 = true;
        private bool _toggleState2 = false;
        private string _inputText = "VellumUI High Density Interface";
        private string _statusMessage = "All systems operational";

        public event PropertyChangedEventHandler? PropertyChanged;

        public string SelectedSection
        {
            get => _selectedSection;
            set
            {
                if (_selectedSection != value)
                {
                    _selectedSection = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSidebarCompact
        {
            get => _isSidebarCompact;
            set
            {
                if (_isSidebarCompact != value)
                {
                    _isSidebarCompact = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged();
                }
            }
        }

        public double SliderValue
        {
            get => _sliderValue;
            set
            {
                if (Math.Abs(_sliderValue - value) > 0.001)
                {
                    _sliderValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ToggleState1
        {
            get => _toggleState1;
            set
            {
                if (_toggleState1 != value)
                {
                    _toggleState1 = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ToggleState2
        {
            get => _toggleState2;
            set
            {
                if (_toggleState2 != value)
                {
                    _toggleState2 = value;
                    OnPropertyChanged();
                }
            }
        }

        public string InputText
        {
            get => _inputText;
            set
            {
                if (_inputText != value)
                {
                    _inputText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ServerMetric> Servers { get; } = new();
        public ObservableCollection<ProjectTask> Tasks { get; } = new();
        public ObservableCollection<LogMessage> Logs { get; } = new();
        public ObservableCollection<string> Tags { get; } = new();

        public ICommand NavigateCommand { get; }
        public ICommand ToggleSidebarCommand { get; }
        public ICommand RefreshDataCommand { get; }

        public MainViewModel()
        {
            NavigateCommand = new RelayCommand(param =>
            {
                if (param is string section)
                {
                    SelectedSection = section;
                }
            });

            ToggleSidebarCommand = new RelayCommand(_ =>
            {
                IsSidebarCompact = !IsSidebarCompact;
            });

            RefreshDataCommand = new RelayCommand(_ =>
            {
                StatusMessage = $"Data refreshed at {DateTime.Now:HH:mm:ss}";
            });

            LoadSampleData();
        }

        private void LoadSampleData()
        {
            Servers.Add(new ServerMetric { ServerId = "SRV-US-01", Name = "US-East Production Primary", Region = "us-east-1", CpuUsage = 34.2, MemoryUsage = 68.5, Status = "Online", StatusVariant = ZenBadgeVariant.Success, Latency = "14ms" });
            Servers.Add(new ServerMetric { ServerId = "SRV-US-02", Name = "US-West Database Cluster", Region = "us-west-2", CpuUsage = 82.0, MemoryUsage = 91.2, Status = "High Load", StatusVariant = ZenBadgeVariant.Warning, Latency = "32ms" });
            Servers.Add(new ServerMetric { ServerId = "SRV-EU-01", Name = "EU-Central API Gateway", Region = "eu-central-1", CpuUsage = 18.7, MemoryUsage = 42.1, Status = "Online", StatusVariant = ZenBadgeVariant.Success, Latency = "88ms" });
            Servers.Add(new ServerMetric { ServerId = "SRV-AP-01", Name = "AP-East Edge Cache", Region = "ap-east-1", CpuUsage = 55.4, MemoryUsage = 62.0, Status = "Online", StatusVariant = ZenBadgeVariant.Success, Latency = "124ms" });
            Servers.Add(new ServerMetric { ServerId = "SRV-BACKUP", Name = "Cold Storage & Analytics", Region = "us-east-2", CpuUsage = 5.1, MemoryUsage = 14.8, Status = "Standby", StatusVariant = ZenBadgeVariant.Muted, Latency = "22ms" });

            Tasks.Add(new ProjectTask { Id = "ZEN-101", Title = "Implement high density WPF framework", Assignee = "A. Munro", Priority = "Critical", PriorityVariant = ZenBadgeVariant.Danger, IsCompleted = true, DueDate = DateTime.Today.AddDays(1) });
            Tasks.Add(new ProjectTask { Id = "ZEN-102", Title = "Design dark blue/slate color palette", Assignee = "A. Munro", Priority = "High", PriorityVariant = ZenBadgeVariant.Warning, IsCompleted = true, DueDate = DateTime.Today.AddDays(2) });
            Tasks.Add(new ProjectTask { Id = "ZEN-103", Title = "Build top menu & left navigation controls", Assignee = "A. Munro", Priority = "High", PriorityVariant = ZenBadgeVariant.Primary, IsCompleted = true, DueDate = DateTime.Today.AddDays(3) });
            Tasks.Add(new ProjectTask { Id = "ZEN-104", Title = "Minimal border radius & compact margin theme", Assignee = "A. Munro", Priority = "Medium", PriorityVariant = ZenBadgeVariant.Secondary, IsCompleted = true, DueDate = DateTime.Today.AddDays(4) });
            Tasks.Add(new ProjectTask { Id = "ZEN-105", Title = "Demonstration showcase application", Assignee = "A. Munro", Priority = "High", PriorityVariant = ZenBadgeVariant.Primary, IsCompleted = true, DueDate = DateTime.Today.AddDays(5) });

            Logs.Add(new LogMessage { Timestamp = "12:00:01", Level = "INFO", LevelVariant = ZenBadgeVariant.Info, Message = "VellumUI theme resources loaded successfully.", Source = "VellumUI.Core" });
            Logs.Add(new LogMessage { Timestamp = "12:00:04", Level = "SUCCESS", LevelVariant = ZenBadgeVariant.Success, Message = "Primary & Secondary palettes registered (Cyan/Violet).", Source = "ZenTheme" });
            Logs.Add(new LogMessage { Timestamp = "12:00:09", Level = "WARN", LevelVariant = ZenBadgeVariant.Warning, Message = "GPU acceleration fallback checked.", Source = "RenderEngine" });
            Logs.Add(new LogMessage { Timestamp = "12:00:15", Level = "INFO", LevelVariant = ZenBadgeVariant.Info, Message = "High density layout metrics initialized.", Source = "LayoutSystem" });

            Tags.Add("WPF");
            Tags.Add("Dark Gray");
            Tags.Add("Minimal Border Radius");
            Tags.Add("High Usable Space");
            Tags.Add(".NET 8.0");
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
