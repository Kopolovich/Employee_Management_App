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
/// Interaction logic for CreateAccountWindow.xaml
/// </summary>
public partial class CreateAccountWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public CreateAccountWindow()
    {
        User = new();
        InitializeComponent();
    }

    public BO.User User
    {
        get { return (BO.User)GetValue(UserProperty); }
        set { SetValue(UserProperty, value); }
    }
    // Using a DependencyProperty as the backing store for User.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty UserProperty =
        DependencyProperty.Register("User", typeof(BO.User), typeof(CreateAccountWindow), new PropertyMetadata(null));

    private void Button_Click_CreateAccount(object sender, RoutedEventArgs e)
    {
        if (User.Id == 0 || User.Id < 100000000 || User.Id > 999999999)
            MessageBox.Show("Please enter a valid Id containing 9 digits to log in", "", MessageBoxButton.OK);
        else if (User.Password == null || User.Password.Length < 6 || User.Password.Length > 14)
            MessageBox.Show("Please enter a valid password containing between 6 and 14 characters to log in", "", MessageBoxButton.OK);
        else
        {
            if (User.Role == BO.UserRole.Admin)
            {
                try
                {
                    s_bl.User.Create(User);
                    MessageBox.Show("Account successfully created", "", MessageBoxButton.OK);
                    Close();
                    new AdminWindow().Show();
                }
                catch
                {
                    MessageBox.Show("You already have an account", "", MessageBoxButton.OK);
                    Close();
                }
            }
            else
            {
                try
                {
                    s_bl.Engineer.Read(User.Id);
                    s_bl.User.Create(User);
                    MessageBox.Show("Account successfully created", "", MessageBoxButton.OK);
                    Close();
                    new EngineerUserWindow(User.Id).Show();
                    
                }
                catch (BO.BlDoesNotExistException)
                {
                    MessageBox.Show("You are not registered as an engineer \nplease reach out to your admin for further information", "", MessageBoxButton.OK);
                    Close();
                }
                catch (BO.BlAlreadyExistsException)
                {
                    MessageBox.Show("You already have an account", "", MessageBoxButton.OK);
                    Close();
                }
            }
        }
        
    }

}
