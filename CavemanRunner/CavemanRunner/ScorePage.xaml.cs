using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;

namespace CavemanRunner
{
    public partial class ScorePage : PhoneApplicationPage
    {
        private IsolatedStorageSettings scoreStorage = IsolatedStorageSettings.ApplicationSettings;

        public ScorePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string highscore = "";
            string score = "";

            try
            {
                highscore = (string)scoreStorage["highscore"];
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                // No preference is saved.
                highscore = "0";
            }

            if (NavigationContext.QueryString.TryGetValue("score", out score))
            {
                if(Convert.ToInt32(score) > Convert.ToInt32(highscore))
                {
                    statusTextBlock.Text = "NEW HIGH SCORE!";
                    youScoredTextBlock.Text += score;
                    scoreStorage["highscore"] = score;
                }
                else
                {
                    statusTextBlock.Text = "HIGH SCORE: " + highscore;
                    youScoredTextBlock.Text += score;
                }
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            NavigationService.Navigate(new Uri("/MainMenuPage.xaml", UriKind.Relative));
        }
    }
}