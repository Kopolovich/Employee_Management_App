using PL.Engineer;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Task;

/// <summary>
/// Interaction logic for TaskListWindow.xaml
/// </summary>
public partial class TaskListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public TaskListWindow()
    {
        InitializeComponent();
    }

    private void Window_Activated_Refresh(object sender, EventArgs e)
    {
        TaskList = s_bl.Task.ReadAll();
    }

    public BO.EngineerExperienceForFilter Level { get; set; } = BO.EngineerExperienceForFilter.All;
    public IEnumerable<BO.TaskInList> TaskList
    {
        get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }

    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

    private void Button_Click_OpenTaskWindow_Add(object sender, RoutedEventArgs e)
    {
        new TaskWindow().ShowDialog();
    }
    private void ListView_DoubleClick_UpdateTask(object sender, RoutedEventArgs e)
    {
        BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
        if (task != null)
            new TaskWindow(task.Id).ShowDialog();
    }

    private void Click_filterByTime(object sender, RoutedEventArgs e)
    {
        MenuItem? menuItem = sender as MenuItem;
        if (menuItem != null)
        {
            int startDays=0, endDays=0;
            if((string)menuItem.Header == "Up to 2 days")
            {
                startDays = 0;
                endDays = 2;
            }
            else if ((string)menuItem.Header == "3-9 days")
            {
                startDays= 3; 
                endDays = 9;
            }
            else if((string)menuItem.Header == "10 and up")
            {
                TaskList = s_bl.Task.ReadAll(item => item.RequiredEffortTime >= new TimeSpan(10, 0, 0, 0));

            }

            TaskList = s_bl.Task.ReadAll(item=> item.RequiredEffortTime >= new TimeSpan(startDays,0,0,0) && item.RequiredEffortTime <= new TimeSpan(endDays, 0, 0, 0));
        }
    }

    private void Click_filterByStatus(object sender, RoutedEventArgs e)
    {
        MenuItem? obMenuItem = e.OriginalSource as MenuItem;

        if (obMenuItem != null)
        {
            TaskList = s_bl.Task.ReadAll(item => item.Status == (BO.Status)obMenuItem.Header);
        }
    }

    private void Click_filterByComplexity(object sender, RoutedEventArgs e)
    {
        MenuItem? obMenuItem = e.OriginalSource as MenuItem;

        if (obMenuItem != null)
        {
            
            TaskList = s_bl.Task.ReadAll(item => item.Complexity == (BO.EngineerExperience)obMenuItem.Header);
        }
    }

    private void Click_ResetFilters(object sender, RoutedEventArgs e)
    {
        TaskList = s_bl.Task.ReadAll();
    }
}
