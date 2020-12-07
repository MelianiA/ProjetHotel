using Makrisoft.Makfi.Dal;
using Makrisoft.Makfi.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace Makrisoft.Makfi
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Reference_ViewModel.Main = new MainViewModel();
            InitializeComponent();
            DataContext = Reference_ViewModel.Main;
        }
 
        
    }
}
