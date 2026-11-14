using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using VellumUI.Controls;
using VellumUI.Demo.Models;

namespace VellumUI.Demo.ViewModels
{
    public class EmailClientViewModel : INotifyPropertyChanged
    {
        private MailFolderModel? _selectedFolder;
        private string _selectedFilter = "All";
        private string _searchQuery = string.Empty;
        private EmailItemModel? _selectedEmail;
        private string _replyText = string.Empty;
        private string _statusBanner = string.Empty;
        private bool _isStatusBannerVisible = false;
        private bool _isMarkdownEnabled = true;
        private double _storageUsedGb = 7.4;
        private double _storageTotalGb = 15.0;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<MailFolderModel> Folders { get; } = new();
        public ObservableCollection<EmailItemModel> AllEmails { get; } = new();
        public ObservableCollection<EmailItemModel> FilteredEmails { get; } = new();
        public ObservableCollection<string> AvailableTags { get; } = new();

        public MailFolderModel? SelectedFolder
        {
            get => _selectedFolder;
            set
            {
                if (_selectedFolder != value)
                {
                    _selectedFolder = value;
                    OnPropertyChanged();
                    ApplyFilter();
                }
            }
        }

        public string SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (_selectedFilter != value)
                {
                    _selectedFilter = value;
                    OnPropertyChanged();
                    ApplyFilter();
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
                    ApplyFilter();
                }
            }
        }

        public EmailItemModel? SelectedEmail
        {
            get => _selectedEmail;
            set
            {
                if (_selectedEmail != value)
                {
                    _selectedEmail = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasSelectedEmail));
                }
            }
        }

        public bool HasSelectedEmail => SelectedEmail != null;

        public string ReplyText
        {
            get => _replyText;
            set
            {
                if (_replyText != value)
                {
                    _replyText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusBanner
        {
            get => _statusBanner;
            set
            {
                if (_statusBanner != value)
                {
                    _statusBanner = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsStatusBannerVisible
        {
            get => _isStatusBannerVisible;
            set
            {
                if (_isStatusBannerVisible != value)
                {
                    _isStatusBannerVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsMarkdownEnabled
        {
            get => _isMarkdownEnabled;
            set
            {
                if (_isMarkdownEnabled != value)
                {
                    _isMarkdownEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public double StorageUsedGb
        {
            get => _storageUsedGb;
            set
            {
                if (Math.Abs(_storageUsedGb - value) > 0.01)
                {
                    _storageUsedGb = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StoragePercentage));
                }
            }
        }

        public double StorageTotalGb
        {
            get => _storageTotalGb;
            set
            {
                if (Math.Abs(_storageTotalGb - value) > 0.01)
                {
                    _storageTotalGb = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StoragePercentage));
                }
            }
        }

        public double StoragePercentage => StorageTotalGb > 0 ? (StorageUsedGb / StorageTotalGb) * 100 : 0;

        public ICommand SelectFolderCommand { get; }
        public ICommand SelectFilterCommand { get; }
        public ICommand ToggleStarCommand { get; }
        public ICommand ToggleUnreadCommand { get; }
        public ICommand ArchiveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SendReplyCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand MarkAllReadCommand { get; }

        public EmailClientViewModel()
        {
            SelectFolderCommand = new RelayCommand(p =>
            {
                if (p is MailFolderModel folder)
                {
                    SelectedFolder = folder;
                }
                else if (p is string folderName)
                {
                    var found = Folders.FirstOrDefault(f => f.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase));
                    if (found != null)
                        SelectedFolder = found;
                }
            });

            SelectFilterCommand = new RelayCommand(p =>
            {
                if (p is string filter)
                {
                    SelectedFilter = filter;
                }
            });

            ToggleStarCommand = new RelayCommand(p =>
            {
                var target = p as EmailItemModel ?? SelectedEmail;
                if (target != null)
                {
                    target.IsStarred = !target.IsStarred;
                    UpdateFolderCounts();
                }
            });

            ToggleUnreadCommand = new RelayCommand(p =>
            {
                var target = p as EmailItemModel ?? SelectedEmail;
                if (target != null)
                {
                    target.IsUnread = !target.IsUnread;
                    UpdateFolderCounts();
                }
            });

            ArchiveCommand = new RelayCommand(p =>
            {
                var target = p as EmailItemModel ?? SelectedEmail;
                if (target != null)
                {
                    target.Folder = "Archive";
                    ApplyFilter();
                    UpdateFolderCounts();
                    ShowBanner($"Archived conversation: \"{target.Subject}\"");
                }
            });

            DeleteCommand = new RelayCommand(p =>
            {
                var target = p as EmailItemModel ?? SelectedEmail;
                if (target != null)
                {
                    if (target.Folder == "Trash")
                    {
                        AllEmails.Remove(target);
                        if (SelectedEmail == target)
                            SelectedEmail = FilteredEmails.FirstOrDefault();
                    }
                    else
                    {
                        target.Folder = "Trash";
                    }
                    ApplyFilter();
                    UpdateFolderCounts();
                    ShowBanner($"Moved to trash: \"{target.Subject}\"");
                }
            });

            SendReplyCommand = new RelayCommand(_ =>
            {
                if (SelectedEmail != null && !string.IsNullOrWhiteSpace(ReplyText))
                {
                    var sentEmail = new EmailItemModel
                    {
                        SenderName = "Andrew Munro",
                        SenderEmail = "a.munro@vellumui.io",
                        SenderInitials = "AM",
                        SenderAvatarColor = "#00D2FF",
                        Recipients = SelectedEmail.SenderEmail,
                        Subject = SelectedEmail.Subject.StartsWith("Re: ") ? SelectedEmail.Subject : $"Re: {SelectedEmail.Subject}",
                        Snippet = ReplyText.Trim(),
                        Body = ReplyText.Trim(),
                        FormattedDate = "Just now",
                        DateTime = DateTime.Now,
                        Folder = "Sent",
                        Category = "Work",
                        CategoryVariant = ZenBadgeVariant.Primary,
                        IsUnread = false
                    };
                    AllEmails.Insert(0, sentEmail);
                    ReplyText = string.Empty;
                    ShowBanner($"Reply successfully sent to {SelectedEmail.SenderName}!");
                    UpdateFolderCounts();
                }
            });

            RefreshCommand = new RelayCommand(_ =>
            {
                ShowBanner("Mailboxes synchronized with remote IMAP/Exchange server.");
            });

            MarkAllReadCommand = new RelayCommand(_ =>
            {
                foreach (var email in FilteredEmails)
                {
                    email.IsUnread = false;
                }
                UpdateFolderCounts();
                ShowBanner("All messages in current view marked as read.");
            });

            InitializeData();
        }

        private void ShowBanner(string message)
        {
            StatusBanner = message;
            IsStatusBannerVisible = true;
        }

        private void InitializeData()
        {
            Folders.Add(new MailFolderModel { Name = "Inbox", DisplayName = "Inbox", IconKey = "Zen.Icon.Geometry.Inbox", BadgeVariant = ZenBadgeVariant.Primary, UnreadCount = 3 });
            Folders.Add(new MailFolderModel { Name = "Starred", DisplayName = "Starred", IconKey = "Zen.Icon.Geometry.Star", BadgeVariant = ZenBadgeVariant.Warning, UnreadCount = 0 });
            Folders.Add(new MailFolderModel { Name = "Sent", DisplayName = "Sent Mail", IconKey = "Zen.Icon.Geometry.Send", BadgeVariant = ZenBadgeVariant.Secondary, UnreadCount = 0 });
            Folders.Add(new MailFolderModel { Name = "Drafts", DisplayName = "Drafts", IconKey = "Zen.Icon.Geometry.Drafts", BadgeVariant = ZenBadgeVariant.Muted, UnreadCount = 1 });
            Folders.Add(new MailFolderModel { Name = "Archive", DisplayName = "Archive", IconKey = "Zen.Icon.Geometry.Archive", BadgeVariant = ZenBadgeVariant.Secondary, UnreadCount = 0 });
            Folders.Add(new MailFolderModel { Name = "Trash", DisplayName = "Trash", IconKey = "Zen.Icon.Geometry.Trash", BadgeVariant = ZenBadgeVariant.Danger, UnreadCount = 0 });

            AvailableTags.Add("DevOps");
            AvailableTags.Add("Design");
            AvailableTags.Add("Security");
            AvailableTags.Add("Work");
            AvailableTags.Add("GitHub");

            // Mock Emails
            var email1 = new EmailItemModel
            {
                SenderName = "Sarah Jenkins",
                SenderEmail = "s.jenkins@vellumui.io",
                SenderInitials = "SJ",
                SenderAvatarColor = "#00D2FF",
                Recipients = "Andrew Munro <a.munro@vellumui.io>, Core Team <dev@vellumui.io>",
                Subject = "VellumUI v1.2 Architecture Proposal & Rendering Benchmarks",
                Snippet = "Hey Andrew, I've finished the preliminary benchmarks comparing the DirectWrite text stack with default WPF rendering...",
                Body = "Hi Andrew,\n\nI have completed the preliminary benchmarks comparing the DirectWrite text layout stack against standard WPF layout metrics across 10,000 continuous data grid updates.\n\nKey Findings:\n- Frame rendering time reduced from 14.2ms to 4.1ms (71% boost).\n- Memory allocation per row dropped by 38% using ZenCard view caching.\n- Slate Dark contrast ratios meet WCAG 2.1 AAA accessibility thresholds.\n\nAttached is the comprehensive architecture specification document and the raw metrics CSV. Please review and let me know if we can schedule the RFC review for Thursday at 14:00 UTC.\n\nBest regards,\nSarah Jenkins\nPrincipal Architect, VellumUI Team",
                FormattedDate = "10:42 AM",
                DateTime = DateTime.Now.AddHours(-1),
                Folder = "Inbox",
                Category = "Architecture",
                CategoryVariant = ZenBadgeVariant.Primary,
                IsUnread = true,
                IsStarred = true,
                HasAttachments = true,
                Attachments = new List<EmailAttachment>
                {
                    new() { FileName = "VellumUI_v1.2_RFC.pdf", FileSize = "2.4 MB", FileType = "PDF", BadgeVariant = ZenBadgeVariant.Danger },
                    new() { FileName = "benchmark_metrics_raw.csv", FileSize = "340 KB", FileType = "CSV", BadgeVariant = ZenBadgeVariant.Success }
                },
                Tags = { "DevOps", "Design" }
            };

            var email2 = new EmailItemModel
            {
                SenderName = "GitHub Notifications",
                SenderEmail = "notifications@github.com",
                SenderInitials = "GH",
                SenderAvatarColor = "#7A4DFF",
                Recipients = "Andrew Munro <a.munro@vellumui.io>",
                Subject = "[VellumUI/ZenFramework] Pull Request #128 Merged: High-Density Layout Polish",
                Snippet = "PR #128 'Refactor high density layout tokens, minimal scrollbars and compact margins' was merged into main by armunro...",
                Body = "GitHub Actions: Build & Tests Passed [29/29]\n\nPull Request #128 has been successfully merged into 'main' by @armunro.\n\nCommit: 4f8b91a\nAuthor: Andrew Munro <a.munro@vellumui.io>\nBranch: feature/high-density-compact-views\n\nChanges:\n- 12 files changed, +480 lines, -112 lines\n- Automated STA Dispatcher test coverage for ZenTabControl, ZenCallout, ZenStatCard\n\nAll continuous integration checks passed.",
                FormattedDate = "09:15 AM",
                DateTime = DateTime.Now.AddHours(-3),
                Folder = "Inbox",
                Category = "GitHub",
                CategoryVariant = ZenBadgeVariant.Secondary,
                IsUnread = true,
                IsStarred = false,
                HasAttachments = false,
                Tags = { "GitHub", "DevOps" }
            };

            var email3 = new EmailItemModel
            {
                SenderName = "Liam Vance",
                SenderEmail = "l.vance@techcorp.com",
                SenderInitials = "LV",
                SenderAvatarColor = "#00D68F",
                Recipients = "Andrew Munro <a.munro@vellumui.io>",
                Subject = "Client Demo Feedback & Production Deployment Schedule",
                Snippet = "The client stakeholder team was thrilled with the dark slate theme demonstration yesterday! Everything looked crisp...",
                Body = "Hi Andrew,\n\nThe client stakeholder executive team was thrilled with the dark slate theme demonstration yesterday! The high-density layouts, custom stat cards, and compact navigation drawers fit their enterprise desktop requirements perfectly.\n\nAction items for next sprint:\n1. Finalize the standalone email client demonstration page.\n2. Review color contrast tokens for secondary callouts.\n3. Package the NuGet artifact for enterprise SDK distribution.\n\nLet's catch up tomorrow morning to align on milestone 3.\n\nCheers,\nLiam Vance\nProduct Lead",
                FormattedDate = "Yesterday",
                DateTime = DateTime.Now.AddDays(-1),
                Folder = "Inbox",
                Category = "Client",
                CategoryVariant = ZenBadgeVariant.Success,
                IsUnread = true,
                IsStarred = true,
                HasAttachments = false,
                Tags = { "Work" }
            };

            var email4 = new EmailItemModel
            {
                SenderName = "Security Telemetry",
                SenderEmail = "sec-bot@vellumui.io",
                SenderInitials = "SEC",
                SenderAvatarColor = "#FF5252",
                Recipients = "Security Admins <sec-admins@vellumui.io>",
                Subject = "[SECURITY NOTICE] TLS 1.3 Certificate Auto-Renewal Successful",
                Snippet = "Automated certificate authority renew completed for *.vellumui.io and *.api.vellumui.io with 0 error codes...",
                Body = "AUTOMATED SECURITY TELEMETRY NOTICE\n===================================\nStatus: SUCCESS\nHost: edge-ingress-01.us-east.vellumui.io\nCertificate SANs: *.vellumui.io, api.vellumui.io, auth.vellumui.io\nIssuer: DigiCert High Assurance TLS CA-1\nExpiration Date: 2027-11-14 (365 days remaining)\nSignature Algorithm: SHA-256 with RSA 4096-bit\n\nNo administrative intervention required.",
                FormattedDate = "Nov 12",
                DateTime = DateTime.Now.AddDays(-2),
                Folder = "Inbox",
                Category = "Security",
                CategoryVariant = ZenBadgeVariant.Danger,
                IsUnread = false,
                IsStarred = false,
                HasAttachments = false,
                Tags = { "Security" }
            };

            var email5 = new EmailItemModel
            {
                SenderName = "Elena Rostova",
                SenderEmail = "e.rostova@designlab.io",
                SenderInitials = "ER",
                SenderAvatarColor = "#FFB300",
                Recipients = "Andrew Munro <a.munro@vellumui.io>",
                Subject = "Design System Update: Color Swatches & Density Tokens v2",
                Snippet = "Attached are the exported JSON token files for the high-density layout system along with high-res PNG swatches...",
                Body = "Hi Andrew,\n\nI've finalized the token dictionary for the VellumUI design system.\n\nHighlights:\n- Unified color palette with Cyan (#00D2FF), Purple (#7A4DFF), Slate (#0F1318), and Canvas (#151B22).\n- Standardized micro (9pt), caption (10pt), body (12pt), and heading (16pt) typographic scales.\n- Custom icons for mail client: Star, Reply, Forward, Attachments, Inbox, Archive, Send.\n\nPlease check the attached token file and asset bundle.\n\nElena Rostova\nDesign Systems Lead",
                FormattedDate = "Nov 10",
                DateTime = DateTime.Now.AddDays(-4),
                Folder = "Inbox",
                Category = "Design",
                CategoryVariant = ZenBadgeVariant.Warning,
                IsUnread = false,
                IsStarred = false,
                HasAttachments = true,
                Attachments = new List<EmailAttachment>
                {
                    new() { FileName = "vellum_tokens_v2.json", FileSize = "48 KB", FileType = "JSON", BadgeVariant = ZenBadgeVariant.Primary },
                    new() { FileName = "slate_palette_spec.png", FileSize = "1.8 MB", FileType = "PNG", BadgeVariant = ZenBadgeVariant.Warning }
                },
                Tags = { "Design" }
            };

            var emailDraft = new EmailItemModel
            {
                SenderName = "Andrew Munro",
                SenderEmail = "a.munro@vellumui.io",
                SenderInitials = "AM",
                SenderAvatarColor = "#00D2FF",
                Recipients = "Team <team@vellumui.io>",
                Subject = "Draft: VellumUI Release 1.2 Notes & Upgrade Guide",
                Snippet = "Draft notes summarizing all breaking changes, new controls (ZenCallout, ZenTabControl, ZenStatCard)...",
                Body = "Hi Team,\n\nHere is the initial draft of our v1.2 release announcement.\n\nIncluded Features:\n- New high-density email client demo page showcasing full suite of controls\n- Vector icon geometries for email, stars, attachments, and navigation\n- Responsive sidebar drawer with compact toggle\n- Accessible high-contrast dark theme presets",
                FormattedDate = "Nov 11",
                DateTime = DateTime.Now.AddDays(-3),
                Folder = "Drafts",
                Category = "Draft",
                CategoryVariant = ZenBadgeVariant.Muted,
                IsUnread = false,
                IsStarred = false,
                HasAttachments = false,
                Tags = { "Work" }
            };

            AllEmails.Add(email1);
            AllEmails.Add(email2);
            AllEmails.Add(email3);
            AllEmails.Add(email4);
            AllEmails.Add(email5);
            AllEmails.Add(emailDraft);

            _selectedFolder = Folders.FirstOrDefault();
            ApplyFilter();
            SelectedEmail = FilteredEmails.FirstOrDefault();
            UpdateFolderCounts();
        }

        private void UpdateFolderCounts()
        {
            foreach (var folder in Folders)
            {
                if (folder.Name == "Starred")
                {
                    folder.UnreadCount = AllEmails.Count(e => e.IsStarred && e.IsUnread);
                }
                else
                {
                    folder.UnreadCount = AllEmails.Count(e => e.Folder.Equals(folder.Name, StringComparison.OrdinalIgnoreCase) && e.IsUnread);
                }
            }
        }

        private void ApplyFilter()
        {
            FilteredEmails.Clear();
            var folderName = SelectedFolder?.Name ?? "Inbox";

            IEnumerable<EmailItemModel> query = AllEmails;

            if (folderName.Equals("Starred", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(e => e.IsStarred);
            }
            else
            {
                query = query.Where(e => e.Folder.Equals(folderName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(SelectedFilter) && !SelectedFilter.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                switch (SelectedFilter.ToLowerInvariant())
                {
                    case "unread":
                        query = query.Where(e => e.IsUnread);
                        break;
                    case "starred":
                        query = query.Where(e => e.IsStarred);
                        break;
                    case "attachments":
                        query = query.Where(e => e.HasAttachments);
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                var q = SearchQuery.Trim().ToLowerInvariant();
                query = query.Where(e =>
                    e.SenderName.ToLowerInvariant().Contains(q) ||
                    e.SenderEmail.ToLowerInvariant().Contains(q) ||
                    e.Subject.ToLowerInvariant().Contains(q) ||
                    e.Snippet.ToLowerInvariant().Contains(q) ||
                    e.Body.ToLowerInvariant().Contains(q));
            }

            foreach (var email in query.OrderByDescending(e => e.DateTime))
            {
                FilteredEmails.Add(email);
            }

            if (SelectedEmail == null || !FilteredEmails.Contains(SelectedEmail))
            {
                SelectedEmail = FilteredEmails.FirstOrDefault();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
