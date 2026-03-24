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
    /// Логика взаимодействия для ManagerPanel.xaml
    /// </summary>
    public partial class ManagerPanel : Page
    {
        public ManagerPanel()
        {
            InitializeComponent();
        }

        private void ManageProducts_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.ManageProducts());
        }
        private void ManageCategories_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.ManageCategories());
        }

        private void ManageBrands_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.ManageBrands());
        }

        private void ManageTags_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.ManageTags());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
