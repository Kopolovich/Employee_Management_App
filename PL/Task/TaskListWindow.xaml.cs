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
}
