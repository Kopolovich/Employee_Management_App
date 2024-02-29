using BlApi;
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
            User = new();
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

        public BO.User User
        {
            get { return (BO.User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        // Using a DependencyProperty as the backing store for User.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(BO.User), typeof(MainWindow), new PropertyMetadata(null));



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

        private void Window_Activated_Refresh(object sender, EventArgs e)
        {
            CurrentTime = s_bl.Clock;
            User = new();
            User.Password = string.Empty;
        }

        private void Button_Click_ResetClock(object sender, RoutedEventArgs e)
        {
            s_bl.ResetClock();
            CurrentTime = s_bl.Clock;
        }

        private void Button_Click_AddDay(object sender, RoutedEventArgs e)
        {
            s_bl.AddDay();
            CurrentTime = s_bl.Clock;
        }

        private void Button_Click_AddWeek(object sender, RoutedEventArgs e)
        {
            s_bl.AddWeek();
            CurrentTime = s_bl.Clock;
        }

        private void Button_Click_AddMonth(object sender, RoutedEventArgs e)
        {
            s_bl.AddMonth();
            CurrentTime = s_bl.Clock;
        }

        private void Button_Click_AddYear(object sender, RoutedEventArgs e)
        {
            s_bl.AddYear();
            CurrentTime = s_bl.Clock;
        }

        private void Button_Click_LogIn(object sender, RoutedEventArgs e)
        {
            if(User.Id == 0 || User.Id < 100000000 || User.Id > 999999999)
                MessageBox.Show("Please enter a valid Id containing 9 digits to log in", "", MessageBoxButton.OK);
            else if(User.Password == null || User.Password.Length < 6 || User.Password.Length > 14)
                MessageBox.Show("Please enter a valid password containing between 6 and 14 characters to log in", "", MessageBoxButton.OK);
            else
            {
                BO.User existingUser = new();
                try
                {
                    existingUser = s_bl.User.Read(User.Id);
                }
                catch
                {
                    MessageBoxResult mbResult = MessageBox.Show($"User with Id: {User.Id} does not exist \nWould you like to create an account?", "", MessageBoxButton.YesNo);
                    if (mbResult == MessageBoxResult.Yes)
                    {
                        new CreateAccountWindow().ShowDialog();
                    }
                }

                if (existingUser.Password == s_bl.User.HashPassword(User.Password))
                {
                    if (existingUser.Role == BO.UserRole.Admin)
                        new AdminWindow().ShowDialog();
                    else
                        new EngineerUserWindow(existingUser.Id).Show();
                }
                else
                {
                    MessageBox.Show("Incorrect password", "", MessageBoxButton.OK);
                }
            }
            
        }

        private void Button_Click_CreateAccount(object sender, RoutedEventArgs e)
        {
            new CreateAccountWindow().ShowDialog();
        }
    }
}