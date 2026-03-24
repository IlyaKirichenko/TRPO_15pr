using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace TRPO_KI_15pr_ElectronicShop.Converters
{
    public class LessThanConverters : IValueConverter
    { 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int stock && stock < 10)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffd129"));
            }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00b4c4"));
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

       
    }
}
