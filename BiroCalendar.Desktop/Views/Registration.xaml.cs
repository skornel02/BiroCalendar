using System.Net;
using System.Net.Http.Json;
using System.Windows;

namespace BiroCalendar.Desktop.Views;

/// <summary>
/// Interaction logic for Registration.xaml
/// </summary>
public partial class Registration : Window
{
    public Registration()
    {
        InitializeComponent();
    }

    private async void HandleRegistration(object sender, RoutedEventArgs e)
    {
        var hasEmailError = false;
        var hasPasswordError = false;
        var hasPasswordAgainError = false;

        var email = EmailAddressInput.Text;
        var password = PasswordInput.Password;
        var passwordAgain = PasswordAgainInput.Password;

        if (string.IsNullOrEmpty(email))
        {
            hasEmailError = true;
            EmailErrorLabel.Content = "Email megadása kötelező!";
        }
        if (!email.Contains('@'))
        {
            hasEmailError = true;
            EmailErrorLabel.Content = "Nem megfelelő e-mail formátum!";
        }

        if (string.IsNullOrEmpty(password) || password.Length < 8 || password.Length > 64)
        {
            hasPasswordError = true;
            PasswordErrorLabel.Content = "8-64 karakter közötti jelszó megadása kötelező";
        }

        if (passwordAgain != password)
        {
            hasPasswordAgainError = true;
            PasswordAgainErrorLabel.Content = "Az ellenőrző jelszónak egyeznie kell az eredetivel!";
        }


        EmailErrorLabel.Visibility = hasEmailError ? Visibility.Visible : Visibility.Hidden;
        PasswordErrorLabel.Visibility = hasPasswordError? Visibility.Visible : Visibility.Hidden;
        PasswordAgainErrorLabel.Visibility = hasPasswordAgainError ? Visibility.Visible : Visibility.Hidden;
        var hasError = hasEmailError || hasPasswordError || hasPasswordAgainError;

        if (hasError)
        {
            return;
        }

        var client = Globals.Client.Value;

        try
        {
            var result = await client.PostAsync($"/account/register?email={WebUtility.UrlEncode(email)}&password={WebUtility.UrlEncode(password)}", null);
            if (result.IsSuccessStatusCode)
            {
                MessageBox.Show("Kérlek jelentkezz be!", "Sikeres regisztráció!", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                var error = await result.Content.ReadFromJsonAsync<string>();
                MessageBox.Show(error, "Sikertelen regisztráció!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch
        (Exception ex)
        {
            MessageBox.Show(ex.Message, "Sikertelen regisztráció!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
