using Makrisoft.Makfi.Dal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Makrisoft.Makfi.Converters
{
    public class ViewSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value.ToString() + parameter.ToString();
            var v = value.ToString() == parameter.ToString() ||
                    s == $"{ViewEnum.Home}Messagerie" ||
                    s == $"{ViewEnum.Intervention}Messagerie" ||
                    s == $"{ViewEnum.InterventionDetail}Messagerie" ||
                    s == $"{ViewEnum.Chambre}Messagerie" ||
                    s == $"{ViewEnum.InterventionNew}Messagerie" ||
                    s == $"{ViewEnum.Employe}Messagerie" ||
                    s == $"{ViewEnum.Synthese}Messagerie" ||
                    s == $"{ViewEnum.Administration}Messagerie" ||
                    s == $"{ViewEnum.ChambreGroupe}Messagerie" ||
                    s == $"{ViewEnum.Utilisateur}Messagerie" ||
                    s == $"{ViewEnum.Hotel}Messagerie" ||
                    s == $"{ViewEnum.DecoupageNew}Messagerie" ||
                    s == $"{ViewEnum.Decoupage}Messagerie" ||

                    s == $"{ViewEnum.Intervention}Back" ||
                    s == $"{ViewEnum.InterventionDetail}Back" ||
                    s == $"{ViewEnum.Chambre}Back" ||
                    s == $"{ViewEnum.InterventionNew}Back" ||
                    s == $"{ViewEnum.Employe}Back" ||
                    s == $"{ViewEnum.Synthese}Back" ||
                    s == $"{ViewEnum.Administration}Back" ||
                    s == $"{ViewEnum.Utilisateur}Back" ||
                    s == $"{ViewEnum.Hotel}Back" ||
                    s == $"{ViewEnum.ChambreGroupe}Back" ||
                    s == $"{ViewEnum.DecoupageNew}Back" ||
                    s == $"{ViewEnum.Decoupage}Back"
                    ? Visibility.Visible : Visibility.Hidden;
            return v;
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
