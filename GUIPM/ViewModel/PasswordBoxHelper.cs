using System.Windows;
using System.Windows.Controls;

namespace GUIPM
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPassword = DependencyProperty.RegisterAttached(
            "BoundPassword",
            typeof(string),
            typeof(PasswordBoxHelper),
            new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BoundPassword);
        }

        public static void SetBoundPassword(DependencyObject obj, string value)
        {
            obj.SetValue(BoundPassword, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

                if (!(bool)passwordBox.GetValue(UpdatingPassword))
                {
                    passwordBox.Password = (string)e.NewValue;
                }

                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static readonly DependencyProperty UpdatingPassword = DependencyProperty.RegisterAttached(
            "UpdatingPassword",
            typeof(bool),
            typeof(PasswordBoxHelper),
            new PropertyMetadata(false));

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                passwordBox.SetValue(UpdatingPassword, true);
                SetBoundPassword(passwordBox, passwordBox.Password);
                passwordBox.SetValue(UpdatingPassword, false);
            }
        }
    }
}
