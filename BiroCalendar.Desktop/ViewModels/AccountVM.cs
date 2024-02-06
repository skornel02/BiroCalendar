using BiroCalendar.Desktop.Helpers;
using BiroCalendar.Desktop.Views;
using BiroCalendar.Shared.Dtos;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Windows;

namespace BiroCalendar.Desktop.ViewModels;

public class AccountVM : BindableObject
{
    public ObservableCollection<BiroAccountDto> Accounts { get; set; }

    private BiroAccountDto? selectedAccount;
    public BiroAccountDto? SelectedAccount
    {
        get => selectedAccount;
        set
        {
            if (value != selectedAccount)
            {
                selectedAccount = value;
                OnPropertyChanged();
                DeleteCommand.Changed();
                OpenCommand.Changed();
            }
        }
    }

    public RelayCommand DeleteCommand { get; set; }
    public RelayCommand OpenCommand { get; set; }

    public AccountVM()
    {
        Accounts = [];
        DeleteCommand = new(_ => selectedAccount is not null, HandleDelete);
        OpenCommand = new(_ => selectedAccount is not null, HandleOpen);

        _ = Update();
    }

    public async Task Update()
    {
        var accountRequest = await Globals.Client.Value.GetAsync("/api/BiroAccount");
        var nextAccounts = await accountRequest.Content.ReadFromJsonAsync<List<BiroAccountDto>>();

        Accounts.Clear();
        nextAccounts?.ForEach(Accounts.Add);
    }

    public void HandleOpen(object? param)
    {
        var calendarWindow = new Calendar(SelectedAccount.Id);
        calendarWindow.Show();
    }

    public async void HandleDelete(object? param)
    {
        var result = MessageBox.Show("Biztos törölni szeretnéd?", "Törlés", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

        if (result == MessageBoxResult.No)
        {
            return;
        }

        // todo: do delete stuff.

        await Update();
    }
}
