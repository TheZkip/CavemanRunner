using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Framework.WindowsPhone;
using CavemanRunner.Resources;
using System.IO;

namespace CavemanRunner
{
    public partial class GamePage : PhoneApplicationPage
    {
        private CavemanRunner _game;

        // Constructor
        public GamePage()
        {
            InitializeComponent();

            _game = XamlGame<CavemanRunner>.Create("", this);
            LoadPattern(new Uri("Patterns/pattern1.txt", UriKind.Relative));
            LoadPattern(new Uri("Patterns/pattern2.txt", UriKind.Relative));
            LoadPattern(new Uri("Patterns/pattern3.txt", UriKind.Relative));
            LoadPattern(new Uri("Patterns/pattern4.txt", UriKind.Relative));
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        public void LoadPattern(Uri fileUri)
        {
            var str = Application.GetResourceStream(fileUri);
            StreamReader reader = new StreamReader(str.Stream);
            _game.GenerateLevelFromString(reader.ReadToEnd());
        }

        private void CavemanRunner_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            //TouchCollection touches = TouchPanel.GetState();
            //_game.touches = touches;

            //if (touches.Count <= 2)
            //{
            //    if (touches.Count == 2)
            //        _game.jumpDoubleTap = true;

            //    e.Complete();
            //}
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
        }
    }
}