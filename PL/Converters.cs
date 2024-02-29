using System.Globalization;
using System.Windows;
using System.Windows.Data;

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
                                    : "Double click to choose a task for your engineer";
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