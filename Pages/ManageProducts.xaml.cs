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
using Microsoft.EntityFrameworkCore;
using TRPO_KI_15pr_ElectronicShop.Models;
using TRPO_KI_15pr_ElectronicShop.Service;

namespace TRPO_KI_15pr_ElectronicShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ManageProducts.xaml
    /// </summary>
    public partial class ManageProducts : Page
    {
        private ElectricalShopKirichenkoContext db = DBService.Instance.Context;
        private ObservableCollection<Product> products = new ObservableCollection<Product>();
        private Product selectedProduct = null;

        public ManageProducts()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategories();
            LoadBrands();
            LoadProducts();
            LoadTags();
        }
        private void LoadCategories()
        {
            var categories = db.Categories.ToList();
            cmbCategory.ItemsSource = categories;
            cmbCategory.DisplayMemberPath = "Name";
            cmbCategory.SelectedValuePath = "Id";
        }
        private void LoadBrands()
        {
            var brands = db.Brands.ToList();
            cmbBrand.ItemsSource = brands;
            cmbBrand.DisplayMemberPath = "Name";
            cmbBrand.SelectedValuePath = "Id";
        }
        private void LoadTags()
        {
            var tags = db.Tags.ToList();
            lbTags.ItemsSource = tags;
            lbTags.DisplayMemberPath = "Name";
            lbTags.SelectedValuePath = "Id";
        }
        private void LoadProducts()
        {
            products.Clear();
            var data = db.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Tags)
                .ToList();
            foreach (var product in data)
            {
                products.Add(product);
            }
            lvProducts.ItemsSource = products;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Введите описание товара");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Введите корректную цену");
                return;
            }

            if (!int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("Введите корректное количество");
                return;
            }
            if (!double.TryParse(txtRating.Text, out double rating))
            {
                MessageBox.Show("Введите корректный рейтинг");
                return;
            }
            if (rating < 0 || rating > 5)
            {
                MessageBox.Show("Рейтинг должен быть от 0 до 5");
                return;
            }

            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию");
                return;
            }

            if (cmbBrand.SelectedItem == null)
            {
                MessageBox.Show("Выберите бренд");
                return;
            }

            var product = new Product
            {
                Name = txtName.Text,
                Description = txtDescription.Text,
                Price = (float)price,
                Stock = stock,
                Rating = rating,
                CreatedAt = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                CategoryId = (int)cmbCategory.SelectedValue,
                BrandId = (int)cmbBrand.SelectedValue
            };
            foreach (Tag tag in lbTags.SelectedItems)
            {
                product.Tags.Add(tag);
            }
            db.Products.Add(product);
            db.SaveChanges();

            MessageBox.Show("Товар добавлен");
            LoadProducts();
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Выберите товар");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара");
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Введите корректную цену");
                return;
            }
            if (!double.TryParse(txtRating.Text, out double rating))
            {
                MessageBox.Show("Введите корректный рейтинг");
                return;
            }
            if (rating < 0 || rating > 5)
            {
                MessageBox.Show("Рейтинг должен быть от 0 до 5");
                return;
            }
            if (!int.TryParse(txtStock.Text, out int stock))
            {
                MessageBox.Show("Введите корректное количество");
                return;
            }

            selectedProduct.Name = txtName.Text;
            selectedProduct.Description = txtDescription.Text;
            selectedProduct.Price = (float)price;
            selectedProduct.Stock = stock;


            if (cmbCategory.SelectedItem != null)
            {
                selectedProduct.CategoryId = (int)cmbCategory.SelectedValue;
            }

            if (cmbBrand.SelectedItem != null)
            {
                selectedProduct.BrandId = (int)cmbBrand.SelectedValue;
            }
            selectedProduct.Tags.Clear();
            foreach (Tag tag in lbTags.SelectedItems)
            {
                selectedProduct.Tags.Add(tag);
            }
            db.SaveChanges();

            MessageBox.Show("Товар обновлен");
            LoadProducts();
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Выберите товар");
                return;
            }

            var result = MessageBox.Show($"Удалить '{selectedProduct.Name}'?", "Подтверждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
            try
                {
                    selectedProduct.Tags.Clear();
                    db.SaveChanges(); 

                    db.Products.Remove(selectedProduct);
                    db.SaveChanges();

                    MessageBox.Show("Товар удален");
                    LoadProducts();
                } catch
                {
                    MessageBox.Show("Ошибка при удалении");
                } 
            }
        }
        private void lvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProduct = lvProducts.SelectedItem as Product;
            if (selectedProduct != null)
            {
                txtName.Text = selectedProduct.Name;
                txtDescription.Text = selectedProduct.Description;
                txtPrice.Text = selectedProduct.Price.ToString();
                txtStock.Text = selectedProduct.Stock.ToString();
                txtRating.Text = selectedProduct.Rating.ToString();
                cmbCategory.SelectedValue = selectedProduct.CategoryId;
                cmbBrand.SelectedValue = selectedProduct.BrandId;

                lbTags.SelectedItems.Clear();
                foreach (var tag in selectedProduct.Tags)
                {
                    lbTags.SelectedItems.Add(tag);
                }
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void txtRating_PreviewTextInput (object sender, TextCompositionEventArgs e)
        {
            if(!char.IsDigit(e.Text, 0) && e.Text != ",")
            {
                e.Handled = true;
            }
        }
    }
}
