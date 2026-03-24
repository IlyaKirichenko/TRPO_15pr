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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TRPO_KI_15pr_ElectronicShop.Models;
using TRPO_KI_15pr_ElectronicShop.Service;

namespace TRPO_KI_15pr_ElectronicShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ManageBrands.xaml
    /// </summary>
    public partial class ManageBrands : Page
    {
        private ElectricalShopKirichenkoContext db = DBService.Instance.Context;
        private ObservableCollection<Brand> brands = new ObservableCollection<Brand>();
        private Brand selectedBrand = null;
        public ManageBrands()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBrands();
        }
        private void LoadBrands()
        {
            brands.Clear();
            var data = db.Brands.ToList();
            foreach (var cat in data)
            {
                brands.Add(cat);   
            }
            lvBrands.ItemsSource = brands;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название бренда");
                return;
            }

            var existingBrand = db.Brands.FirstOrDefault(b => b.Name == txtName.Text);
            if (existingBrand != null)
            {
                MessageBox.Show("Бренд с таким названием уже существует");
                return;
            }

            var brand = new Brand
            {
                Name = txtName.Text
            };

            db.Brands.Add(brand);
            db.SaveChanges();

            MessageBox.Show("Бренд добавлен");
            txtName.Text = "";
            LoadBrands();
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBrand == null)
            {
                MessageBox.Show("Выберите бренда");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название бренда");
                return;
            }

            selectedBrand.Name = txtName.Text;
            db.SaveChanges();

            MessageBox.Show("Бренд обновлен");
            LoadBrands();
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBrand == null)
            {
                MessageBox.Show("Выберите бренд");
                return;
            }
            var result = MessageBox.Show($"Удалить '{selectedBrand.Name}'?", "Подтверждение", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
            try
            {
                db.Brands.Remove(selectedBrand);
                db.SaveChanges();
                MessageBox.Show("Бренд удален");
                LoadBrands();
            }
            catch
            {
                MessageBox.Show("Нельзя удалить бренд, к ней привязаны товары");
            }
            }
            
        }

        private void lvBrands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBrand = lvBrands.SelectedItem as Brand;
            if (selectedBrand != null)
            {
                txtName.Text = selectedBrand.Name;
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
