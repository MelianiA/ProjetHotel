using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.Tools;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Makrisoft.Makfi
{
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Database Check
            var s = MakfiData.Init(Makfi.Properties.Settings.Default.MakfiConnectionString);
            if (s != "") { MessageBox.Show(s, "Contrôle base de données"); Shutdown(); return; }

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            // StartupUri
            StartupUri = new Uri("Views/MainView.xaml", UriKind.Relative);
        }

    }
}
