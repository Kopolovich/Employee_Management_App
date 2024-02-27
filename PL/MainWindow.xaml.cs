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



        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }
        // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));

            

        /// <summary>
        /// initializing data base
        /// </summary>
        /// <param name="sender"> wpf control that activated the event </param>
        /// <param name="e"> event args </param>
        private void Button_Click_Init_DB(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Are you sure you want to initialize the Data Base? \n all existing data will be permanently deleted", "", MessageBoxButton.YesNo);
            if(mbResult == MessageBoxResult.Yes)
                s_bl.Initialize();
        }

        /// <summary>
        /// Resetting data base
        /// </summary>
        /// <param name="sender"> wpf control that activated the event </param>
        /// <param name="e"> event args </param>
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
            new EngineerUserWindow(279660300).Show();
        }

        private void Window_Activated_Refresh(object sender, EventArgs e)
        {
            CurrentTime = s_bl.Clock;
        }

        private void Button_Click_ResetClock(object sender, RoutedEventArgs e)
        {
            s_bl.ResetClock();
        }

        private void Button_Click_AddDay(object sender, RoutedEventArgs e)
        {
            s_bl.AddDay();
        }

        private void Button_Click_AddWeek(object sender, RoutedEventArgs e)
        {
            s_bl.AddWeek();
        }

        private void Button_Click_AddMonth(object sender, RoutedEventArgs e)
        {
            s_bl.AddMonth();
        }

        private void Button_Click_AddYear(object sender, RoutedEventArgs e)
        {
            s_bl.AddYear();
        }

        
    }
}