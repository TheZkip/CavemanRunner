using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MonoGame.Framework.WindowsPhone;

namespace CavemanRunner
{
    public partial class MainMenuPage : PhoneApplicationPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }

        private void QuitGame(object sender, RoutedEventArgs e)
        {
            Application.Current.Terminate();
        }
    }
}