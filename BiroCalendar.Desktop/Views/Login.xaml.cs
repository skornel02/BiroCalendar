using System.Net;
using System.Net.Http.Json;
using System.Windows;

namespace BiroCalendar.Desktop.Views;

/// <summary>
/// Interaction logic for LoginWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private async void HandleLogin(object sender, RoutedEventArgs e)
    {
        var username = UsernameTextbox.Text;
        var password = PasswordTextbox.Password;

        var client = Globals.Client.Value;

        try
        {
            var result = await client.PostAsync($"/account/login?email={WebUtility.UrlEncode(username)}&password={WebUtility.UrlEncode(password)}", null);
            if (result.IsSuccessStatusCode)
            {
                MessageBox.Show("Átirányítás a főoldalra...", "Sikeres bejelentkezés!", MessageBoxButton.OK, MessageBoxImage.Information);
                var accountWindow = new Accounts();
                accountWindow.Show();
                Close();
            }
            else
            {
                var error = await result.Content.ReadFromJsonAsync<string>();
                MessageBox.Show(error, "Sikertelen bejelentkezés!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch
        (Exception ex)
        {
            MessageBox.Show(ex.Message, "Sikertelen bejelentkezés!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void HandleRegister(object sender, RoutedEventArgs e)
    {
        var registerWindow = new Registration();
        registerWindow.Owner = this;
        registerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        var result = registerWindow.ShowDialog();
    }
}
