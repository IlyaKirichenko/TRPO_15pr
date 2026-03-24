using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public ElectricalShopKirichenkoContext db = DBService.Instance.Context;
        public ObservableCollection<Product> products { get; set; } = new();
        public ICollectionView productsView { get; set; }
        public string searchQuery { get; set; }
        public string filterPriceFrom { get; set; } = null!;
        public string filterPriceTo { get; set; } = null!;

        public int selectedCategoryId = 0;
        public int selectedBrandId = 0;
        public MainPage(int i)
        {
            InitializeComponent();
            productsView = CollectionViewSource.GetDefaultView(products);
            productsView.Filter = FilterForms;
            LoadCategories();
            LoadBrands();
            if (i == 1)
            {
                ManagerButton.Visibility = Visibility.Visible;
            }
            else
            {
                ManagerButton.Visibility = Visibility.Collapsed;
            }
        }
        private void LoadCategories()
        {
            var categories = db.Categories.ToList();
            foreach (var cat in categories)
            {
                var item = new ComboBoxItem();
                item.Content = cat.Name;
                item.Tag = cat.Id;
                CategoryFilter.Items.Add(item);
            }
        }

        private void LoadBrands()
        {
            var brands = db.Brands.ToList();
            foreach (var brand in brands)
            {
                var item = new ComboBoxItem();
                item.Content = brand.Name;
                item.Tag = brand.Id;
                BrandFilter.Items.Add(item);
            }
        }
        public void LoadList(object sender, EventArgs e)
        {
            products.Clear();
            var data = db.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Tags)
                .ToList();
            foreach (var form in db.Products.ToList())
                products.Add(form);

           
        }
        public bool FilterForms(object obj)
        {
            double from = 0;
            double to = double.MaxValue;
            bool fromPriceV = double.TryParse(filterPriceFrom, out from);
            bool ToPriceV = double.TryParse(filterPriceTo, out to);

            
            if (fromPriceV && ToPriceV && from > to)
            {
                PriceError.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                PriceError.Visibility = Visibility.Collapsed;
            }

            if (obj is not Product)
            {
                return false;
            }
            var prod = (Product)obj;

            if (!string.IsNullOrWhiteSpace(searchQuery) && !prod.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (double.TryParse(filterPriceFrom, out double priceFrom))
            {
                if (prod.Price < priceFrom) return false;
            }

            if (double.TryParse(filterPriceTo, out double priceTo))
            {
                if (prod.Price > priceTo) return false;
            }

            if (selectedCategoryId != 0 && prod.CategoryId != selectedCategoryId)
            {
                return false;
            }

            if (selectedBrandId != 0 && prod.BrandId != selectedBrandId)
            {
                return false;
            }

            return true;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productsView == null) return;

            productsView.SortDescriptions.Clear();
            var selected = (ComboBoxItem)((ComboBox)sender).SelectedItem;

            if (selected == null) return;

            switch (selected.Tag.ToString())
            {
                case "Name":
                    productsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                    break;
                case "CheaperFirst":
                    productsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Ascending));
                    break;
                case "ExpensiveFirst": 
                    productsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Descending));
                    break;
                case "QuantityDecrease": 
                    productsView.SortDescriptions.Add(new SortDescription("Stock", ListSortDirection.Descending));
                    break;
                case "QuantityGrowth": 
                    productsView.SortDescriptions.Add(new SortDescription("Stock", ListSortDirection.Ascending));
                    break;
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            productsView.Refresh();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            PriceFromTextBox.Text = "";
            PriceToTextBox.Text = "";

            searchQuery = "";
            filterPriceFrom = "";
            filterPriceTo = "";

            CategoryFilter.SelectedIndex = 0;
            BrandFilter.SelectedIndex = 0;
            selectedCategoryId = 0;
            selectedBrandId = 0;

            productsView.Refresh();
        }

        private void BrandFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = BrandFilter.SelectedItem as ComboBoxItem;
            if (selected != null)
            {
                selectedBrandId = Convert.ToInt32(selected.Tag);
                productsView.Refresh();
            }
        }

        private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = CategoryFilter.SelectedItem as ComboBoxItem;
            if (selected != null)
            {
                selectedCategoryId = Convert.ToInt32(selected.Tag);
                productsView.Refresh();
            }
        }

        private void OpenManegerPanel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.ManagerPanel());
        }

        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
            {
                if (!char.IsDigit(ch))
                {
                    e.Handled = true;
                    return;
                }
            }
        }
    }
}
