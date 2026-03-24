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
    /// Логика взаимодействия для ManageCategories.xaml
    /// </summary>
    public partial class ManageCategories : Page
    {
        private ElectricalShopKirichenkoContext db = DBService.Instance.Context;
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        private Category selectedCategory = null;
        public ManageCategories()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
        }
        private void LoadCategories()
        {
            categories.Clear();
            var data = db.Categories.ToList();
            foreach (var cat in data)
            {
                categories.Add(cat);
            }
            lvCategories.ItemsSource = categories;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название категории");
                return;
            }

            var category = new Category
            {
                Name = txtName.Text
            };

            db.Categories.Add(category);
            db.SaveChanges();

            MessageBox.Show("Категория добавлена");
            txtName.Text = "";
            LoadCategories();
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCategory == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название категории");
                return;
            }

            selectedCategory.Name = txtName.Text;
            db.SaveChanges();

            MessageBox.Show("Категория обновлена");
            LoadCategories();
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCategory == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }
            var result = MessageBox.Show($"Удалить '{selectedCategory.Name}'?", "Подтверждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    db.Categories.Remove(selectedCategory);
                    db.SaveChanges();
                    MessageBox.Show("Категория удалена");
                    LoadCategories();
                }
                catch
                {
                    MessageBox.Show("Нельзя удалить категорию, к ней привязаны товары");
                }
            }
           
        }

        private void lvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCategory = lvCategories.SelectedItem as Category;
            if (selectedCategory != null)
            {
                txtName.Text = selectedCategory.Name;
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
