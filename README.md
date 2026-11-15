# Vellum UI — High-Density Dark Slate WPF UI Framework

Vellum UI (VellumUI) is a modern, high-density WPF UI framework designed for technical applications, developer tools, telemetry dashboards, and data-intensive interfaces.

---

## Key Design Principles & Specifications

- **Minimal Margins & Padding**: Maximizes usable screen real estate (up to 40% higher data density).
- **Minimal / Zero Border Radiuses**: Clean, sharp 0–1px engineering aesthetic.
- **Dark Blue / Slate Palette**: Deep slate background `#0F172A` with layered `#1E293B` surfaces and `#090D16` canvas.
- **Primary & Secondary Colors**:
  - **Primary**: Electric Cyan / Sky (`#0EA5E9` / `#38BDF8`)
  - **Secondary**: Electric Violet (`#8B5CF6` / `#A78BFA`)
  - **Status Accents**: Success Emerald (`#10B981`), Warning Amber (`#F59E0B`), Danger Rose (`#F43F5E`), Info Cyan (`#06B6D4`).
- **Bright & Vibrant Foregrounds**: Ultra-crisp `#F8FAFC` primary typography paired with high-contrast vibrant text helpers (`#38BDF8`, `#4ADE80`, `#C084FC`, `#FDE047`, `#FB7185`).
- **Top Menus & Left Menus**: Dedicated controls for both horizontal header navigation (`VellumTopMenu`, `VellumTopMenuItem`) and collapsible vertical navigation drawers (`VellumLeftNav`, `VellumNavItem`, `VellumNavSection`).
- **Demonstration Showcase Application**: Built-in interactive showcase app (`VellumUI.Demo`) demonstrating all controls, tables, inputs, navigation, containers, and theme resources.

---

## Quick Start Guide

### 1. Reference Vellum UI in your WPF Project

Add a reference to `VellumUI` via NuGet package:

```powershell
dotnet add package VellumUI
```

Or reference `VellumUI.csproj` directly in your project.

### 2. Include Theme Resources in `App.xaml`

```xml
<Application x:Class="MyProject.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/VellumUI;component/Themes/VellumTheme.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

### 3. Use VellumWindow with Custom TitleBar and Top Navigation

```xml
<controls:VellumWindow x:Class="MyProject.MainWindow"
                       xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                       xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                       xmlns:controls="clr-namespace:VellumUI.Controls;assembly=VellumUI"
                       Title="My High Density App"
                       WindowIcon="{StaticResource Zen.Icon.Geometry.Layers}"
                       Width="1100" Height="700">
    <controls:VellumWindow.TitleBarContent>
        <controls:VellumTopMenu>
            <controls:VellumTopMenuItem Header="File">
                <MenuItem Header="New" />
                <MenuItem Header="Open" />
                <MenuItem Header="Save" />
            </controls:VellumTopMenuItem>
            <controls:VellumTopMenuItem Header="View" />
            <controls:VellumTopMenuItem Header="Tools" />
        </controls:VellumTopMenu>
    </controls:VellumWindow.TitleBarContent>

    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto" />
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>

        <!-- Left Navigation Sidebar -->
        <controls:VellumLeftNav Grid.Column="0">
            <controls:VellumNavSection Header="WORKSPACE" />
            <controls:VellumNavItem Header="Dashboard" Icon="{StaticResource Zen.Icon.Geometry.Dashboard}" IsSelected="True" />
            <controls:VellumNavItem Header="Data Tables" Icon="{StaticResource Zen.Icon.Geometry.Table}" />
            <controls:VellumNavItem Header="Settings" Icon="{StaticResource Zen.Icon.Geometry.Settings}" />
        </controls:VellumLeftNav>

        <!-- Main Content -->
        <Border Grid.Column="1" Padding="8">
            <controls:VellumCard Header="OVERVIEW" ShowTopAccent="True" HighlightBrush="{StaticResource Zen.Brush.Primary}">
                <Button Content="Primary Action" Style="{StaticResource Zen.Style.Button.Primary}" />
            </controls:VellumCard>
        </Border>
    </Grid>
</controls:VellumWindow>
```

---

## Included Controls & Styles

| Category | Elements |
|---|---|
| **Window & Layout** | `VellumWindow`, `VellumCard`, `VellumStatCard`, `VellumCallout`, `GroupBox`, `Border` |
| **Navigation** | `VellumTopMenu`, `VellumTopMenuItem`, `VellumLeftNav`, `VellumNavItem`, `VellumNavSection`, `Menu`, `MenuItem`, `TabControl`, `TabItem` |
| **Buttons** | `Zen.Style.Button.Primary`, `Zen.Style.Button.Secondary`, `Zen.Style.Button.Surface`, `Zen.Style.Button.Outline`, `Zen.Style.Button.Ghost`, `Zen.Style.Button.Success`, `Zen.Style.Button.Danger`, `Zen.Style.Button.Icon` |
| **Form Inputs** | `TextBox`, `PasswordBox`, `VellumSearchBox`, `VellumDatePicker`, `VellumColorPicker`, `ComboBox`, `ComboBoxItem` |
| **Toggles & Selection** | `CheckBox`, `RadioButton`, `VellumToggleSwitch`, `VellumSegmentedControl`, `Slider` |
| **Data & Tables** | `DataGrid`, `DataGridColumnHeader`, `DataGridCell`, `DataGridRow`, `ListView`, `ListBox`, `TreeView`, `VellumProgressBar` |
| **Indicators & Badges** | `VellumBadge` (Primary, Secondary, Success, Warning, Danger, Info, Muted, Outline), `VellumTag`, `VellumStatusIndicator`, `ToolTip`, `ContextMenu`, `Expander` |

---

## Running the Demonstration App

```powershell
dotnet run --project src/VellumUI.Demo/VellumUI.Demo.csproj
```
