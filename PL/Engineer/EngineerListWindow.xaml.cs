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

        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        public BO.EngineerExperienceForFilter Level { get; set; } = BO.EngineerExperienceForFilter.All;

        public int TaskId { get; set; } = 0;

        private void ComboBox_SelectionChanged_FilterEngineerByLevel(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Level == BO.EngineerExperienceForFilter.All) ?
                s_bl.Engineer.ReadAll() : s_bl.Engineer.ReadAll(item => item.Level == (BO.EngineerExperience)Level);

        }

        private void Button_Click_OpenEngineerWindow_Add(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }

       private void ListView_DoubleClick_UpdateEngineer(object sender, RoutedEventArgs e)
        {
            BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
            if (engineer != null)
            {
                if(TaskId == 0)
                    new EngineerWindow(engineer.Id).ShowDialog();
                else
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

        private void Window_Activated_Refresh(object sender, EventArgs e)
        {
            try 
            {
                if (TaskId == 0)
                    EngineerList = s_bl.Engineer.ReadAll();
                else
                {
                    BO.Task currentTask = s_bl.Task.Read(TaskId);
                    EngineerList = s_bl.Engineer.ReadAll(engineer => engineer.Level >= currentTask.Complexity && engineer.Task == null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void MouseRight_assignTask(object sender, EventArgs e)
        {
            BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
            if (engineer != null && TaskId == 0)
                new TaskListWindow(1, engineer.Id).ShowDialog();
        }
    }
}
