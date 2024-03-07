using PL.Engineer;
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
/// Interaction logic for TaskRecyclingBin.xaml
/// </summary>
public partial class TaskRecyclingBin : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public TaskRecyclingBin()
    {
        InitializeComponent();
        TaskList = new ObservableCollection<BO.TaskInList>();
        List<BO.TaskInList> taskListTemp = s_bl.Task.ReadAllNotActive().ToList();
        foreach (var task in taskListTemp)
        {
            TaskList.Add(task);
        }
        SelectedTask = null;
    }

    public ObservableCollection<BO.TaskInList> TaskList
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
        set { SetValue(TaskListProperty, value); }
    }
    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TaskListProperty =
        DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskRecyclingBin), new PropertyMetadata(null));

    public BO.TaskInList? SelectedTask
    {
        get { return (BO.TaskInList)GetValue(SelectedTaskProperty); }
        set { SetValue(SelectedTaskProperty, value); }
    }
    // Using a DependencyProperty as the backing store for SelectedEngineer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectedTaskProperty =
        DependencyProperty.Register("SelectedTask", typeof(BO.TaskInList), typeof(TaskRecyclingBin), new PropertyMetadata(null));

    private void Button_Click_DeleteTask(object sender, RoutedEventArgs e)
    {
        if (SelectedTask != null)
        {
            MessageBoxResult mbResult = MessageBox.Show("Are you sure you want to delete this task permanently?", "", MessageBoxButton.YesNo);
            if (mbResult == MessageBoxResult.Yes)
            {
                try
                {
                    s_bl.Task.Delete(SelectedTask.Id);
                    TaskList.Remove(SelectedTask);
                    SelectedTask = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
                }
            }
        }
    }

    private void Button_Click_RecoverTask(object sender, RoutedEventArgs e)
    {
        if (SelectedTask != null)
        {
            try
            {
                s_bl.Task.RecoverTask(SelectedTask);
                TaskList.Remove(SelectedTask);
                SelectedTask = null;
                MessageBox.Show("Task successfully recovered", "", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }
    }
}
