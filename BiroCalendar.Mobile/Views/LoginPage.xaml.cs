using BiroCalendar.Mobile.ViewModels;

namespace BiroCalendar.Mobile.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginViewModel();
	}
}
