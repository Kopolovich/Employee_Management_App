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

        private void Button_Click_Show_Engineers(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void Button_Click_Init_DB(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Are you sure you want to initialize the Data Base?", "", MessageBoxButton.YesNo);
            if(mbResult == MessageBoxResult.Yes)
                s_bl.Initialize();
        }
    }
}