using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace GUIPM
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string username;
        private string password;
        private string loginMessage;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string LoginMessage
        {
            get { return loginMessage; }
            set
            {
                loginMessage = value;
                OnPropertyChanged(nameof(LoginMessage));
            }
        }

        public ICommand LoginCommand { get; }

        public event Action LoggedIn;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            Console.WriteLine($"Attempting login with Username: {Username}, Password: {Password}"); // Debugging line

            // Replace with your actual login logic
            if (Username == "user" && Password == "password")
            {
                LoginMessage = "Login successful!";
                LoggedIn?.Invoke();
            }
            else
            {
                LoginMessage = "Login failed. Please check your credentials.";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
