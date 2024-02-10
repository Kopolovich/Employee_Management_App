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
        public EngineerListWindow()
        {
            InitializeComponent();
            //EngineerList = s_bl.Engineer.ReadAll();
        }



        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        public BO.EngineerExperienceForFilter Level { get; set; } = BO.EngineerExperienceForFilter.All;

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
                new EngineerWindow(engineer.Id).ShowDialog();
        }

        private void Window_Activated_Refresh(object sender, EventArgs e)
        {
            EngineerList = s_bl.Engineer.ReadAll();
        }
    }
}
