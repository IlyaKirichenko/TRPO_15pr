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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TRPO_KI_15pr_ElectronicShop.Models;
using TRPO_KI_15pr_ElectronicShop.Service;

namespace TRPO_KI_15pr_ElectronicShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ManageTags.xaml
    /// </summary>
    public partial class ManageTags : Page
    {
        private ElectricalShopKirichenkoContext db = DBService.Instance.Context;
        private ObservableCollection<Tag> tags = new ObservableCollection<Tag>();
        private Tag selectedTag = null;
        public ManageTags()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTags();
        }
        private void LoadTags()
        {
            tags.Clear();
            var data = db.Tags.ToList();
            foreach (var cat in data)
            {
                tags.Add(cat);
            }
            lvTag.ItemsSource = tags;
        }
        private void lvTag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTag = lvTag.SelectedItem as Tag;
            if (selectedTag != null)
            {
                txtName.Text = selectedTag.Name;
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название тега");
                return;
            }

            var existingTag = db.Tags.FirstOrDefault(b => b.Name == txtName.Text);
            if (existingTag != null)
            {
                MessageBox.Show("Тег с таким названием уже существует");
                return;
            }

            var tag = new Tag
            {
                Name = txtName.Text
            };

            db.Tags.Add(tag);
            db.SaveChanges();

            MessageBox.Show("Тег добавлен");
            txtName.Text = "";
            LoadTags();
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTag == null)
            {
                MessageBox.Show("Выберите тег");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название тега");
                return;
            }

            selectedTag.Name = txtName.Text;
            db.SaveChanges();

            MessageBox.Show("Тег обновлен");
            LoadTags();
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTag == null)
            {
                MessageBox.Show("Выберите тег");
                return;
            }
            var result = MessageBox.Show($"Удалить '{selectedTag.Name}'?", "Подтверждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
            try
            {
                db.Tags.Remove(selectedTag);
                db.SaveChanges();
                MessageBox.Show("Тег удален");
                LoadTags();
            }
            catch
            {
                MessageBox.Show("Нельзя удалить тег, к ней привязаны товары");
            }
            }
            
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
