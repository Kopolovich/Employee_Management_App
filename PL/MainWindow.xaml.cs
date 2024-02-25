using PL.Engineer;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent(); 
        }

     

        private void Button_Click_Init_DB(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Are you sure you want to initialize the Data Base? \n all existing data will be permanently deleted", "", MessageBoxButton.YesNo);
            if(mbResult == MessageBoxResult.Yes)
                s_bl.Initialize();
        }

        private void Button_Click_Reset_DB(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Are you sure you want to reset the Data? \n all existing data will be permanently deleted", "", MessageBoxButton.YesNo);
            if (mbResult == MessageBoxResult.Yes)
                s_bl.Reset();
        }

        private void Button_Click_ShowAdminWindow(object sender, RoutedEventArgs e)
        {
            new AdminWindow().Show();
        }

        private void Button_Click_ShowEngineerUserWindow(object sender, RoutedEventArgs e)
        {
            new EngineerUserWindow(268522587).Show();
        }
    }
}