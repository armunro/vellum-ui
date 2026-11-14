using System;
using VellumUI.Controls;

namespace VellumUI.Demo.Models
{
    public class ServerMetric
    {
        public string ServerId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
        public string Status { get; set; } = "Online";
        public ZenBadgeVariant StatusVariant { get; set; } = ZenBadgeVariant.Success;
        public string Latency { get; set; } = string.Empty;
    }

    public class ProjectTask
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Assignee { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
        public ZenBadgeVariant PriorityVariant { get; set; } = ZenBadgeVariant.Primary;
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class LogMessage
    {
        public string Timestamp { get; set; } = string.Empty;
        public string Level { get; set; } = "INFO";
        public ZenBadgeVariant LevelVariant { get; set; } = ZenBadgeVariant.Info;
        public string Message { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }
}
