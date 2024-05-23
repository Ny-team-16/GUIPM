using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GUIPM
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
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

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is LoginViewModel viewModel)
            {
                passwordBox.PasswordChanged += (s, args) =>
                {
                    if (passwordBox.Password != viewModel.Password)
                    {
                        viewModel.Password = passwordBox.Password;
                    }
                };

                viewModel.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == nameof(viewModel.Password) && passwordBox.Password != viewModel.Password)
                    {
                        passwordBox.Password = viewModel.Password;
                    }
                };
            }
        }
    }
}
