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
/// Interaction logic for TaskWindow.xaml
/// </summary>
public partial class TaskWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public TaskWindow(int id = 0, int state = 0, int engineerId = 0)
    {
        //State = 0: admin adding new task, State = 1: admin updating task, State = 2: engineer choosing task

        EngineerId = engineerId;

        //window in state of admin adding new task
        if (id == 0)
        {
            CurrentTask = new();
            DependenciesToAdd = s_bl.Task.ReadAll().ToList();
            State = 0;
        }

        //window in state of admin updating task or engineer choosing task
        else
            try
            {
                CurrentTask = s_bl.Task.Read(id);
                DependenciesToAdd = s_bl.Task.ReadAll(item => item.Id != CurrentTask.Id
                && (CurrentTask.Dependencies == null ||
                !CurrentTask.Dependencies.Any(dep => dep.Id == item.Id))).ToList();
                if (state != 0) State = state;
                else State = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButton.OK);
            }             

        //creating Observable Collection for dependencies so that changes appear immediately
        if (CurrentTask.Dependencies != null)
        {
            if (CurrentTaskDependencies == null)
                CurrentTaskDependencies = new ObservableCollection<BO.TaskInList>();
            foreach (var dep in CurrentTask.Dependencies)
                CurrentTaskDependencies.Add(dep);
        }

        InitializeComponent();
    }

    public int State { get; set; } = 0;
    public int EngineerId { get; set; } = 0;

    public BO.Task CurrentTask
    {
        get { return (BO.Task)GetValue(CurrentTaskProperty); }
        set { SetValue(CurrentTaskProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTaskProperty =
        DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

    public ObservableCollection<BO.TaskInList>? CurrentTaskDependencies
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(CurrentTaskDependenciesProperty); }
        set { SetValue(CurrentTaskDependenciesProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentTaskDependencies.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentTaskDependenciesProperty =
        DependencyProperty.Register("CurrentTaskDependencies", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));

    public List<BO.TaskInList> DependenciesToAdd
    {
        get { return (List<BO.TaskInList>)GetValue(DependenciesToAddProperty); }
        set { SetValue(DependenciesToAddProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentEngineer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DependenciesToAddProperty =
        DependencyProperty.Register("DependenciesToAdd", typeof(List<BO.TaskInList>), typeof(TaskWindow), new PropertyMetadata(null));

    public BO.TaskInList? DependencyToAdd { get; set; } = null;

    private void Button_Click_AddOrUpdateTask(object sender, RoutedEventArgs e)
    {
        Button? btn = sender! as Button;
        if (btn != null)
        {
            try
            {
                if (CurrentTaskDependencies != null)
                {
                    if (!CurrentTaskDependencies.Any())
                        CurrentTask.Dependencies = null;
                    else
                        CurrentTask.Dependencies = CurrentTaskDependencies.ToList();
                }

                MessageBoxResult mbResult = MessageBoxResult.No;

                if (btn.Content.ToString() == "Add")
                {               
                    s_bl.Task.Create(CurrentTask);
                    //mbResult = MessageBox.Show($"Task added successfully! \nWould you like to assign a engineer to this task?", "", MessageBoxButton.YesNo);
                    MessageBox.Show($"Task added successfully!" ,"", MessageBoxButton.OK);
                }

                else if(btn.Content.ToString() == "Update")
                {
                    s_bl.Task.Update(CurrentTask);
                    mbResult = MessageBox.Show($"Task updated successfully! \nWould you like to assign a engineer to this task?", "", MessageBoxButton.YesNo);
                }
                else
                {
                    s_bl.Engineer.AssignTaskToEngineer(EngineerId, new BO.TaskInEngineer() { Id = CurrentTask.Id, Alias = CurrentTask.Alias });
                    MessageBox.Show($"Task assigned, Good luck!");
                }

                if (mbResult == MessageBoxResult.Yes)
                {
                    new EngineerListWindow(CurrentTask.Id).ShowDialog();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
            Close();
        }
    }

    private void Button_Click_DeleteDependency(object sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;

        if (btn.DataContext is BO.TaskInList && State!=2)
        {
            BO.TaskInList deleteMe = (BO.TaskInList)btn.DataContext;
            CurrentTaskDependencies!.Remove(deleteMe);
            DependenciesToAdd = s_bl.Task.ReadAll(item => item.Id != CurrentTask.Id
                && (CurrentTaskDependencies == null ||
                !CurrentTaskDependencies.Any(dep => dep.Id == item.Id))).ToList();
        }
    }

    private void Button_Click_AddDependency(object sender, RoutedEventArgs e)
    {
        if (DependencyToAdd != null)
        {
            if (CurrentTaskDependencies == null)
                CurrentTaskDependencies = new ObservableCollection<BO.TaskInList>();

            if (!CurrentTaskDependencies.Contains(DependencyToAdd))
            {
                CurrentTaskDependencies.Add(DependencyToAdd);
                DependenciesToAdd = s_bl.Task.ReadAll(item => item.Id != CurrentTask.Id
                && (CurrentTaskDependencies == null ||
                !CurrentTaskDependencies.Any(dep => dep.Id == item.Id))).ToList();
            }          
        }           
    }

}
