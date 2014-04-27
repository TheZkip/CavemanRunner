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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            JournalEntry lastPage = null;

            try
            {
                lastPage = NavigationService.BackStack.First();
            }
            catch(Exception ex)
            {

            }

            if (lastPage != null && lastPage.Source.ToString().Contains("GamePage.xaml"))
            {
                ResumeButton.Visibility = Visibility.Visible;
            }
            else
            {
                ResumeButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ResumeGame(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
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