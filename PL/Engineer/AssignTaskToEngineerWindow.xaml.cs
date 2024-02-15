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
        if (TaskInEngineerList.Any()) 
            CurrentTask = TaskInEngineerList.First();
        else
        {
            MessageBox.Show("No available tasks for this engineer", "", MessageBoxButton.OK);
            Close();
        }
        EngineerId = id;
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
            s_bl.Engineer.AssignTaskToEngineer(EngineerId, CurrentTask);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
        }
    }
}
