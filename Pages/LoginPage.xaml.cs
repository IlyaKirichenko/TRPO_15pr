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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TRPO_KI_15pr_ElectronicShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginManager_Click(object sender, RoutedEventArgs e)
        {
            string CorrectPin = "1234";
            string PinCodeField = PinCode.Password; 
            if (string.IsNullOrWhiteSpace(PinCodeField))
            {
                MessageBox.Show("Заполните поле с пин-кодом");
                return;
            }

            if (PinCodeField == CorrectPin)
            {
             
                NavigationService.Navigate(new Pages.MainPage(1));
            }
            else
            { 
                MessageBox.Show("Не верно набран пин-код");
                return;
            } 
        }

        private void LoginUser_Click(object sender, RoutedEventArgs e)
        {
             NavigationService.Navigate(new Pages.MainPage(0));
        }
    }
}
