using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUIPM
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            LoadImage();
        }

        private void LoadImage()
        {
            var imageUrl = "https://images.ctfassets.net/f2vbu16fzuly/1b4ORoZpPgQVDQP6KSEX4z/f718b7702b155c6a49d9c7ae171ae3e8/Isologotipo__letra_oscura__-_Nicolas_De_Paul.png";
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imageUrl, UriKind.Absolute);
            bitmap.EndInit();

            MyImage.Source = bitmap;
        }
    }
}
