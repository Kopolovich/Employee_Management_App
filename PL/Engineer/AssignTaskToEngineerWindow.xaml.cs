using BO;
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

namespace PL.Engineer;

/// <summary>
/// Interaction logic for AssignTaskToEngineerWindow.xaml
/// </summary>
public partial class AssignTaskToEngineerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public AssignTaskToEngineerWindow(int id)
    {
        InitializeComponent();
        TaskInEngineerList = s_bl.Task.ReadTasksForEngineer(id);
     
                   
        EngineerId = id;
    }

    private void Window_Activated_Refresh(object sender, EventArgs e)
    {
        if (!TaskInEngineerList.Any())
        {
            MessageBox.Show("No available tasks for this engineer", "", MessageBoxButton.OK);
            Close();
        }
    }
    public IEnumerable<BO.TaskInEngineer> TaskInEngineerList
    {
        get { return (IEnumerable<BO.TaskInEngineer>)GetValue(TaskInEngineerListProperty); }
        set { SetValue(TaskInEngineerListProperty, value); }
    }

    public static readonly DependencyProperty TaskInEngineerListProperty =
        DependencyProperty.Register("TaskInEngineerList", typeof(IEnumerable<BO.TaskInEngineer>), typeof(AssignTaskToEngineerWindow), new PropertyMetadata(null));

    public TaskInEngineer? CurrentTask { get; set; }

    public int EngineerId { get; set; }

    private void Button_Click_AssignTaskToEngineer(object sender, RoutedEventArgs e)
    {
        try
        {
            if(CurrentTask != null)
            {
                s_bl.Engineer.AssignTaskToEngineer(EngineerId, CurrentTask);
                MessageBox.Show("Task assigned to engineer successfully", "", MessageBoxButton.OK);
                Close();
            }
            else
            {
                MessageBox.Show("Please choose a task", "", MessageBoxButton.OK);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
        }
    }
}
