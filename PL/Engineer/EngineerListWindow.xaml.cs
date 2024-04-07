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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerListWindow(int taskId = 0)
        {
            TaskId = taskId;
            InitializeComponent();
        }

        /// <summary>
        /// refreshes engineer list to show changes 
        /// </summary>
        /// <param name="sender"> wpf control that activated the event </param>
        /// <param name="e"> event args </param>
        private void Window_Activated_Refresh(object sender, EventArgs e)
        {
            try
            {
                //regular state
                if (TaskId == 0)
                {
                    IEnumerable<BO.Engineer> EngineerListTemp = s_bl.Engineer.ReadAll();
                    EngineerList = new ObservableCollection<BO.Engineer>();
                    foreach (var engineer in EngineerListTemp)
                    {
                        EngineerList.Add(engineer);
                    }
                }
                //state of choosing engineer for task    
                else
                {
                    BO.Task currentTask = s_bl.Task.Read(TaskId);
                    IEnumerable<BO.Engineer> EngineerListTemp = s_bl.Engineer.ReadAll(engineer => engineer.Level >= currentTask.Complexity && engineer.Task == null);
                    EngineerList = new ObservableCollection<BO.Engineer>();
                    foreach (var engineer in EngineerListTemp)
                    {
                        EngineerList.Add(engineer);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        public ObservableCollection<BO.Engineer> EngineerList
        {
            get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        //selected level to filter by
        public BO.EngineerExperienceForFilter Level { get; set; } = BO.EngineerExperienceForFilter.All;

        public int TaskId { get; set; } = 0; //if the window is in the state of choosing an engineer for a task

        //updating engineer list according to selected filter
        private void ComboBox_SelectionChanged_FilterEngineerByLevel(object sender, SelectionChangedEventArgs e)
        {
            List<BO.Engineer> EngineerListTemp = (Level == BO.EngineerExperienceForFilter.All) ?
                s_bl.Engineer.ReadAll().ToList() : s_bl.Engineer.ReadAll(item => item.Level == (BO.EngineerExperience)Level).ToList();

            EngineerList = new ObservableCollection<BO.Engineer>();
            foreach (var engineer in EngineerListTemp)
            {
                EngineerList.Add(engineer);
            }
        }

        //opening engineer window in adding mode
        private void Button_Click_OpenEngineerWindow_Add(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }

        //opening engineer window to update or assigning engineer to task
       private void ListBox_DoubleClick_UpdateEngineer(object sender, RoutedEventArgs e)
        {
            BO.Engineer? engineer = (sender as ListBox)?.SelectedItem as BO.Engineer;
            if (engineer != null)
            {
                if(TaskId == 0) //update engineer by admin
                    new EngineerWindow(engineer.Id).ShowDialog();
                else // if in state of choosing engineer for task
                {
                    try
                    {
                        BO.Task currentTask = s_bl.Task.Read(TaskId);
                        s_bl.Engineer.AssignTaskToEngineer(engineer.Id, new BO.TaskInEngineer() { Id = currentTask.Id, Alias = currentTask.Alias });
                        MessageBox.Show("Engineer successfully assigned to task", "", MessageBoxButton.OK);                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
                    }

                    Close();
                }                    
            }                
        }

        //if window is in regular mode, right click to assign task to engineer
        private void MouseRight_assignTask(object sender, EventArgs e)
        {
            if (s_bl.GetProjectStatus() == BO.ProjectStatus.InPlanning)
                MessageBox.Show("Can not assign task to engineer while project is still in planning", "", MessageBoxButton.OK);
            
            else
            {
                BO.Engineer? engineer = (sender as ListBox)?.SelectedItem as BO.Engineer;
                if (engineer != null && TaskId == 0)
                {
                    if (engineer.Task != null)
                        MessageBox.Show("Can not assign a new task to engineer before he finishes working on his current task", "", MessageBoxButton.OK);
                    else
                        new TaskListWindow(1, engineer.Id).ShowDialog();
                }
                    
            }           
        }

        //if window is in regular mode, delete engineer by clicking on bin
        private void Button_Click_DeleteEngineer(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;

            if (TaskId == 0 && tb.DataContext is BO.Engineer)
            {
                try
                {
                    BO.Engineer deleteMe = (BO.Engineer)tb.DataContext;
                    s_bl.Engineer.Delete(deleteMe.Id);
                    EngineerList.Remove(deleteMe);
                }              

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
                }
            }          

        }

        private void Click_RecyclingBin(object sender, MouseButtonEventArgs e)
        {
            new EngineerRecyclingBin().ShowDialog();
        }
    }
}
