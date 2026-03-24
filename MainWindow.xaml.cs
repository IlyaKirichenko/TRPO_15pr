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

namespace TRPO_KI_15pr_ElectronicShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigated += (s, e) =>
            {
                var page = MainFrame.Content as Page;
                if (page != null)
                {
                    this.Title = page.Title;
                }
            };
            MainFrame.Navigate(new Pages.LoginPage());
        }
    }
}