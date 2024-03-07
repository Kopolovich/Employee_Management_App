using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Effects;

namespace PL;

/// <summary>
/// for task window, returns content for button according to the state
/// </summary>
class ConvertStateToContentTask : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((int)value == 0)
            return "Add";
        else if ((int)value == 2)
            return "Choose Task";
        else if((int)value == 3)
            return "Close";
        else
            return "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// for engineer window, returns content for button according to the state
/// </summary>
class ConvertStateToContentEngineer : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((int)value == 0)
            return "Add";
        else if ((int)value == 2)
            return "Choose Engineer";
        else
            return "Update";
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// converter for fields that are only enabled in adding mode
/// </summary>
class ConvertIdToIsEnabled : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// to hide different wpf controls according to window state
/// </summary>
class ConvertStateToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// to disable different wpf controls according to window state
/// </summary>
class ConvertStateToIsEnabled : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 || (int)value == 1 ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// for engineer list window, changing content according to window state
/// </summary>
class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Double click to update engineer, right click to assign task to engineer" 
                                    : "Double click to choose an engineer for your task";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// for task list window, changing content according to window state
/// </summary>
class ConvertIdToContentTask : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Double click to update task, right click to assign engineer to task"
                                    : "Double click to choose a task";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// for engineer user window, to enable done button only if there is a current task
/// </summary>
class ConvertTaskToIsEnabled : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.TaskInList?)value == null ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// for engineer user window, to toggle inability between 2 buttons
/// </summary>
class ConvertEnableIsDisable : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !(bool)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// for log in and create account, to convert id = 0 to empty string
/// </summary>
class ConvertIdToString: IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "" : value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int id;
        int.TryParse((string)value, out id);
        return id == 0 ? 0 : id;
    }
}


/// <summary>
/// for radio buttons in create account window to bind user's role
/// </summary>
class ConvertRoleToIsChecked : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.UserRole)value == BO.UserRole.Admin ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? BO.UserRole.Admin : BO.UserRole.Engineer;
    }
}


/// <summary>
/// for Gantt chart, to locate task in chart according to scheduled start date
/// </summary>
class ConvertStartDateToWidth : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((DateTime)value - s_bl.ProjectStartDate!).Value.Days * 80;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


/// <summary>
/// for Gantt chart, to define width of task rectangle according to required effort time
/// </summary>
class ConvertDurationToWidth : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((TimeSpan)value).Days * 80;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


/// <summary>
/// for Gantt chart, to define color of task rectangle according to status of task
/// </summary>
class ConvertStatusToColor : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        switch ((BO.Status)value)
        {
            case BO.Status.Scheduled:
                return "#5271FF";
            case BO.Status.OnTrack:
                return "#BF9EFF";
            case BO.Status.Late:
                return "#FF5F4A";
            case BO.Status.Done:
                return "#5CE1E6";
            default:
                return null;
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


/// <summary>
/// for tooltip in Gantt chart
/// </summary>
class ConvertEngineerToText : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.EngineerInTask)value == null ? "Not assigned" : $"Assigned to: {((BO.EngineerInTask)value).Name}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// for recycling bin
/// </summary>
class ConvertListToText : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "No items in recycling bin" : "";       
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// for recycling bin in task list window
/// </summary>
class ConvertProjectStatusToVisibility : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((int)value == 0 && s_bl.GetProjectStatus() == BO.ProjectStatus.InPlanning) ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}