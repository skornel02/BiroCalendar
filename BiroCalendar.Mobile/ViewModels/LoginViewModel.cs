using BiroCalendar.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BiroCalendar.Mobile.ViewModels;
public class LoginViewModel : BindableObject
{

    private string _email = "";
    public string Email
    {
        get => _email;
        set {
            _email = value;
            OnPropertyChanged();
        }
    }

    private string _password = "";
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoginCommand { get; private set; }

    public LoginViewModel()
    {
        LoginCommand = new Command(HandleLoginCommand);
    }

    private async void HandleLoginCommand()
    {
        var result = await ApiService.Login(Email, Password);

        if (result is not null)
        {
            await Shell.Current.DisplayAlert("Login failed!", result, "ok");
        }
        else
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
    }
}
