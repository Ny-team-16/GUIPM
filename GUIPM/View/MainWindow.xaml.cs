using System.Windows;
using System.Windows.Media.Imaging;

namespace GUIPM
{
    public partial class MainWindow : Window
    {
        private readonly LoginViewModel loginViewModel;
        private readonly MainViewModel mainViewModel;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            loginViewModel = new LoginViewModel();
            loginViewModel.LoggedIn += OnLoggedIn;

            mainViewModel = new MainViewModel();

            ContentArea.Content = new LoginView { DataContext = loginViewModel };
        }

        private void OnLoggedIn()
        {
            ContentArea.Content = new MainView { DataContext = mainViewModel };
        }

    }
}