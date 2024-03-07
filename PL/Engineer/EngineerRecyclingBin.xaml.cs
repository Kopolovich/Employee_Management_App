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

namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerRecyclingBin.xaml
/// </summary>
public partial class EngineerRecyclingBin : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public EngineerRecyclingBin()
    {
        InitializeComponent();
        EngineerList = new ObservableCollection<BO.Engineer>();
        List<BO.Engineer> EngineerListTemp = s_bl.Engineer.ReadAllNotActive().ToList();
        foreach (var engineer in EngineerListTemp)
        {
            EngineerList.Add(engineer);
        }
        SelectedEngineer = null;
    }

    public ObservableCollection<BO.Engineer> EngineerList
    {
        get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
        set { SetValue(EngineerListProperty, value); }
    }
    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EngineerListProperty =
        DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.Engineer>), typeof(EngineerRecyclingBin), new PropertyMetadata(null));

    public BO.Engineer? SelectedEngineer
    {
        get { return (BO.Engineer)GetValue(SelectedEngineerProperty); }
        set { SetValue(SelectedEngineerProperty, value); }
    }
    // Using a DependencyProperty as the backing store for SelectedEngineer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectedEngineerProperty =
        DependencyProperty.Register("SelectedEngineer", typeof(BO.Engineer), typeof(EngineerRecyclingBin), new PropertyMetadata(null));

    private void Button_Click_DeleteEngineer(object sender, RoutedEventArgs e)
    {
        if(SelectedEngineer != null)
        {
            MessageBoxResult mbResult = MessageBox.Show("Are you sure you want to delete this engineer permanently?", "", MessageBoxButton.YesNo);
            if(mbResult == MessageBoxResult.Yes)
            {
                try
                {
                    s_bl.Engineer.Delete(SelectedEngineer.Id);
                    EngineerList.Remove(SelectedEngineer);
                    SelectedEngineer = null;                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
                }
            }
        }
    }

    private void Button_Click_RecoverEngineer(object sender, RoutedEventArgs e)
    {
        if (SelectedEngineer != null)
        {
            try
            {
                s_bl.Engineer.RecoverEngineer(SelectedEngineer);
                EngineerList.Remove(SelectedEngineer);
                SelectedEngineer = null;
                MessageBox.Show("Engineer successfully recovered", "", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }
    }
}
