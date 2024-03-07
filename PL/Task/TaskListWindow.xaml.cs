using PL.Engineer;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public TaskListWindow(int state = 0, int id = 0)
    {
        State = state; //state = 0: admin, state = 1: engineer
        EngineerId = id;
        TaskList = new ObservableCollection<BO.TaskInList>();
        InitializeComponent();     
    }

    /// <summary>
    /// refreshes task list to show changes 
    /// </summary>
    /// <param name="sender"> wpf control that activated the event </param>
    /// <param name="e"> event args </param>
    private void Window_Activated_Refresh(object sender, EventArgs e)
    {
        TaskList = new ObservableCollection<BO.TaskInList>();

        //admin can see all tasks
        if (State == 0)
        {
            List<BO.TaskInList> TaskListTemp = s_bl.Task.ReadAll().ToList();
            foreach (var task in TaskListTemp)
            {
                TaskList.Add(task);
            }
        }
            
        //engineer can only see the available tasks for him to choose from
        else
        {
            List<BO.TaskInList> TaskListTemp = (from taskInEngineer in s_bl.Task.ReadTasksForEngineer(EngineerId)
                                                let TaskInList = new BO.TaskInList()
                                                {
                                                    Id = taskInEngineer.Id,
                                                    Alias = taskInEngineer.Alias,
                                                    Description = s_bl.Task.Read(taskInEngineer.Id).Description,
                                                    Status = s_bl.Task.Read(taskInEngineer.Id).Status
                                                }
                                                select TaskInList).ToList();
            foreach (var task in TaskListTemp)
            {
                TaskList.Add(task);
            }
        }     
    }

    public int State { get; set; } //Differentiation between different situations
    public int EngineerId { get; set; } //if the window is in the state of choosing a task for an engineer
    
    public ObservableCollection<BO.TaskInList> TaskList
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));


    // to open task window in adding mode
    private void Button_Click_OpenTaskWindow_Add(object sender, RoutedEventArgs e)
    {
        new TaskWindow().ShowDialog();
    }

    // to open task window in updating or choosing task
    private void ListBox_DoubleClick_UpdateTask(object sender, RoutedEventArgs e)
    {
        BO.TaskInList? task = (sender as ListBox)?.SelectedItem as BO.TaskInList;
        if (task != null)
        {
            if(State == 0) 
                new TaskWindow(task.Id).ShowDialog();
            else
            {
                new TaskWindow(task.Id, 2, EngineerId).ShowDialog();
                Close();
            }
                
        }            
    }

    //for admin only, opens engineer list window to choose engineer to assign to task
    private void MouseRight_assignEngineer(object sender, MouseButtonEventArgs e)
    {
        if(s_bl.GetProjectStatus() == BO.ProjectStatus.InPlanning)
            MessageBox.Show("Can not assign engineer to task while project is still in planning", "", MessageBoxButton.OK);
        else
        {
            BO.TaskInList? task = (sender as ListBox)?.SelectedItem as BO.TaskInList;
            if (task != null && State == 0)
                new EngineerListWindow(task.Id).ShowDialog();
        }
    }

    #region filters
    private void Click_filterByTime(object sender, RoutedEventArgs e)
    {
        MenuItem? menuItem = sender as MenuItem;
        if (menuItem != null)
        {
            TaskList = new ObservableCollection<BO.TaskInList>();
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
                List<BO.TaskInList> TaskListTemp = s_bl.Task.ReadAll(item => item.RequiredEffortTime >= new TimeSpan(10, 0, 0, 0)).ToList();
                foreach (var task in TaskListTemp)
                {
                    TaskList.Add(task);
                }             

            }
            if(endDays != 0)
            {
                List<BO.TaskInList> TaskListTemp = s_bl.Task.ReadAll(item => item.RequiredEffortTime >= new TimeSpan(startDays, 0, 0, 0) && item.RequiredEffortTime <= new TimeSpan(endDays, 0, 0, 0)).ToList();
                foreach (var task in TaskListTemp)
                {
                    TaskList.Add(task);
                }
            }     
        }
    }

    private void Click_filterByStatus(object sender, RoutedEventArgs e)
    {
        MenuItem? obMenuItem = e.OriginalSource as MenuItem;

        if (obMenuItem != null)
        {
            TaskList = new ObservableCollection<BO.TaskInList>();
            List<BO.TaskInList> TaskListTemp = s_bl.Task.ReadAll(item => item.Status == (BO.Status)obMenuItem.Header).ToList();
            foreach (var task in TaskListTemp)
            {
                TaskList.Add(task);
            }
        }
    }

    private void Click_filterByComplexity(object sender, RoutedEventArgs e)
    {
        MenuItem? obMenuItem = e.OriginalSource as MenuItem;

        if (obMenuItem != null)
        {
            TaskList = new ObservableCollection<BO.TaskInList>();
            List<BO.TaskInList> TaskListTemp = s_bl.Task.ReadAll(item => item.Complexity == (BO.EngineerExperience)obMenuItem.Header).ToList();
            foreach (var task in TaskListTemp)
            {
                TaskList.Add(task);
            }
        }
    }

    private void Click_ResetFilters(object sender, RoutedEventArgs e)
    {
        TaskList = new ObservableCollection<BO.TaskInList>();
        List<BO.TaskInList> TaskListTemp = s_bl.Task.ReadAll().ToList();
        foreach (var task in TaskListTemp)
        {
            TaskList.Add(task);
        }
    }

    #endregion

    private void Button_Click_DeleteTask(object sender, MouseButtonEventArgs e)
    {
        TextBlock tb = (TextBlock)sender;

        if (State == 0 && tb.DataContext is BO.TaskInList)
        {
            try
            {
                BO.TaskInList deleteMe = (BO.TaskInList)tb.DataContext;
                s_bl.Task.Delete(deleteMe.Id);
                TaskList.Remove(deleteMe);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }       
        }
    }

    private void Click_RecyclingBin(object sender, MouseButtonEventArgs e)
    {
        new TaskRecyclingBin().ShowDialog();
    }
}
