using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ProjetHotel.Converters
{
    public class StarColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var loginLength = value.ToString().Length;
            if (loginLength > int.Parse(parameter.ToString())) return new SolidColorBrush(Colors.Navy);
            return new SolidColorBrush(Colors.Silver);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
