using PL.Engineer;
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
/// Interaction logic for TaskWindow.xaml
/// </summary>
public partial class TaskWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public TaskWindow(int id = 0)
    {
        InitializeComponent();
        if (id == 0)
            CurrentTask = new();            
        else
            try
            {
                CurrentTask = s_bl.Task.Read(id);
                DependenciesToAdd = (List<BO.TaskInList>)s_bl.Task.ReadAll(item => item.Id != CurrentTask.Id 
                && (CurrentTask.Dependencies == null || !CurrentTask.Dependencies.Contains(new BO.TaskInList() { Id = item.Id, Alias = item.Alias, Description = item.Description, Status = item.Status}) )).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButton.OK);
            }

    }

    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

    public BO.TaskInList? DependencyToAdd { get; set; } = null;

    public List<BO.TaskInList> DependenciesToAdd { get; set; } = [];

    private void Button_Click_AddOrUpdateTask(object sender, RoutedEventArgs e)
    {
        Button? btn = sender! as Button;
        if (btn != null)
        {
            try
            {
                if (btn.Content.ToString() == "Add")
                {
                    s_bl.Task.Create(CurrentTask);
                    MessageBox.Show($"Task added successfully");
                }

                else
                {
                    s_bl.Task.Update(CurrentTask);
                    MessageBox.Show($"Task updated successfully");
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }


        }
    }

    private void Click_DeleteDependecy(object sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.DataContext is BO.TaskInList)
        {

            BO.TaskInList deleteme = (BO.TaskInList)btn.DataContext;
            CurrentTask.Dependencies!.Remove(deleteme);
            btn.IsEnabled = false;
            btn.Content = "Deleted";

        }
    }
}
