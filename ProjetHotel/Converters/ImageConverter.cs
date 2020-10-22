using ProjetHotel.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjetHotel.Converters
{
    class PhotoGouvConverter : IValueConverter
    {
        private string Path = "/ProjetHotel;component/Assets/Photos/";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return $"{Path}gouvernante.png";
            Utilisateur g = (Utilisateur)value;
            return $"{Path}{g.Nom}.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class PhotoHotelConverter : IValueConverter
    {
        private string Path = "/ProjetHotel;component/Assets/hotels/";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return $"{Path}hotel.png";
            Hotel h = (Hotel)value;
            return $"{Path}{h.Nom}.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
