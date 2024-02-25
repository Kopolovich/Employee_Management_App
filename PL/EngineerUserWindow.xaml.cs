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
    public EngineerUserWindow(int id)
    {
        InitializeComponent();
        CurrentEngineer = s_bl.Engineer.Read(id);   
    }

    private void Window_Activated_Refresh(object sender, EventArgs e)
    {
        CurrentEngineer = s_bl.Engineer.Read(CurrentEngineer.Id);
        if (CurrentEngineer.Task != null)
        {
            BO.Task task = s_bl.Task.Read(CurrentEngineer.Task.Id);
            CurrentTask = new BO.TaskInList()
            {
                Id = task.Id,
                Alias = task.Alias,
                Description = task.Description,
                Status = task.Status,
            };
        }
        else CurrentTask = null;
    }

    public BO.Engineer CurrentEngineer
    {
        get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
        set { SetValue(CurrentEngineerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentEngineerProperty =
        DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(EngineerUserWindow), new PropertyMetadata(null));

    public BO.TaskInList? CurrentTask
    {
        get { return (BO.TaskInList)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.TaskInList), typeof(EngineerUserWindow), new PropertyMetadata(null));

    private void Button_Click_UpdateCompletedTask(object sender, RoutedEventArgs e)
    {
        BO.Task task = s_bl.Task.Read(CurrentTask!.Id);
        task.CompleteDate = DateTime.Now;
        s_bl.Task.Update(task);
        CurrentTask = null;
    }

    private void Button_Click_ChooseNewTask(object sender, RoutedEventArgs e)
    {
        
        new TaskListWindow(1, CurrentEngineer.Id).ShowDialog();

    }


}
