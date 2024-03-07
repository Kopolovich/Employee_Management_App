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

namespace PL;

/// <summary>
/// Interaction logic for EngineerUserWindow.xaml
/// </summary>
public partial class EngineerUserWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    
    /// <summary>
    /// ctor - initializes current engineer 
    /// </summary>
    /// <param name="id"> id of engineer user </param>
    public EngineerUserWindow(int id)
    {
        InitializeComponent();
        try
        {
            CurrentEngineer = s_bl.Engineer.Read(id);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            Close();
        }
    }

    /// <summary>
    /// refreshes current engineer to get access to his current task
    /// </summary>
    /// <param name="sender"> wpf control that activated the event </param>
    /// <param name="e"> event args </param>
    private void Window_Activated_Refresh(object sender, EventArgs e)
    {
        CurrentEngineer = s_bl.Engineer.Read(CurrentEngineer.Id);
        
        if (CurrentEngineer.Task != null) //if engineer is currently working on a task
        {
            BO.Task task = s_bl.Task.Read(CurrentEngineer.Task.Id);
            CurrentTask = new BO.TaskInList() //creating taskInList entity
            {
                Id = task.Id,
                Alias = task.Alias,
                Description = task.Description,
                Status = task.Status
            };
        }

        else CurrentTask = null; 
    }

    /// <summary>
    /// Current engineer property
    /// </summary>
    public BO.Engineer CurrentEngineer
    {
        get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
        set { SetValue(CurrentEngineerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentEngineerProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerUserWindow), new PropertyMetadata(null));

    /// <summary>
    /// Current task that engineer is working on
    /// </summary>
    public BO.TaskInList? CurrentTask
    {
        get { return (BO.TaskInList)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.TaskInList), typeof(EngineerUserWindow), new PropertyMetadata(null));

    /// <summary>
    /// updating that engineer completed his task
    /// </summary>
    /// <param name="sender"> wpf control that activated the event </param>
    /// <param name="e"> event args </param>
    private void Button_Click_UpdateCompletedTask(object sender, RoutedEventArgs e)
    {
        BO.Task task = s_bl.Task.Read(CurrentTask!.Id);
        task.CompleteDate = DateTime.Now;
        s_bl.Task.Update(task);
        CurrentTask = null;
    }

    /// <summary>
    /// letting engineer choose a new task to work on
    /// </summary>
    /// <param name="sender"> wpf control that activated the event </param>
    /// <param name="e"> event args </param>
    private void Button_Click_ChooseNewTask(object sender, RoutedEventArgs e)
    {
        if (s_bl.GetProjectStatus() == BO.ProjectStatus.InPlanning)
            MessageBox.Show("Can not assign task to engineer while project is still in planning", "", MessageBoxButton.OK);
        else
            new TaskListWindow(1, CurrentEngineer.Id).ShowDialog(); //opens task list window in choosing mode
    }

    private void Double_Click_showTaskDetails(object sender, MouseButtonEventArgs e)
    {
        if (CurrentTask != null)
            new TaskWindow(CurrentTask.Id, 3).ShowDialog();
    }
}
