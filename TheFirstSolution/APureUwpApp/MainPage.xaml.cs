using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using APureUwpApp.Logger;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace APureUwpApp
{
    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static int Pageclickcounter = 0;

        public MainPage()
        {
            this.InitializeComponent();
            MyConsoleTxtBlock.Text += $"Main Page initilized.{Environment.NewLine}";
            MyConsoleTxtBlock.Text += $"Applications Data folder: {ApplicationData.Current.LocalFolder.Path}{Environment.NewLine}";
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            var tu = new MyTileUpdater();
            ulong size = ((MyLogger)((App)App.Current).Logger).GetCurrentLogFileSize();
            tu.UpdateTile(new List<string> { $"{Pageclickcounter++} Clicks", $"{size} bytes MyLogFile.txt",  $"{DateTime.Now.ToString("HH:mm:ss dd.MM.yy")} - LastUpdate"});
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            MyConsoleTxtBlock.Text += $"Click #{Pageclickcounter} {Environment.NewLine}";
            ((App)App.Current).Logger.Log($"Button click nr: {Pageclickcounter++}" );
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            MyConsoleTxtBlock.Text = $"{Environment.NewLine}";
        }
    }
}
