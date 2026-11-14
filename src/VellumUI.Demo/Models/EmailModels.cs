using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VellumUI.Controls;

namespace VellumUI.Demo.Models
{
    public class EmailAttachment
    {
        public string FileName { get; set; } = string.Empty;
        public string FileSize { get; set; } = string.Empty;
        public string FileType { get; set; } = "FILE";
        public ZenBadgeVariant BadgeVariant { get; set; } = ZenBadgeVariant.Secondary;
    }

    public class MailFolderModel : INotifyPropertyChanged
    {
        private int _unreadCount;

        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string IconKey { get; set; } = "Zen.Icon.Geometry.Folder";
        public ZenBadgeVariant BadgeVariant { get; set; } = ZenBadgeVariant.Primary;

        public int UnreadCount
        {
            get => _unreadCount;
            set
            {
                if (_unreadCount != value)
                {
                    _unreadCount = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasUnread));
                    OnPropertyChanged(nameof(UnreadCountString));
                }
            }
        }

        public bool HasUnread => _unreadCount > 0;
        public string UnreadCountString => _unreadCount > 99 ? "99+" : _unreadCount.ToString();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EmailItemModel : INotifyPropertyChanged
    {
        private bool _isUnread;
        private bool _isStarred;

        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderInitials { get; set; } = "VU";
        public string SenderAvatarColor { get; set; } = "#00D2FF";
        public string Recipients { get; set; } = "Andrew Munro <a.munro@vellumui.io>";
        public string Subject { get; set; } = string.Empty;
        public string Snippet { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string FormattedDate { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Folder { get; set; } = "Inbox";
        public string Category { get; set; } = "General";
        public ZenBadgeVariant CategoryVariant { get; set; } = ZenBadgeVariant.Primary;
        public bool HasAttachments { get; set; }
        public List<EmailAttachment> Attachments { get; set; } = new();
        public ObservableCollection<string> Tags { get; set; } = new();

        public bool IsUnread
        {
            get => _isUnread;
            set
            {
                if (_isUnread != value)
                {
                    _isUnread = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FontWeightString));
                }
            }
        }

        public bool IsStarred
        {
            get => _isStarred;
            set
            {
                if (_isStarred != value)
                {
                    _isStarred = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FontWeightString => _isUnread ? "SemiBold" : "Normal";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
