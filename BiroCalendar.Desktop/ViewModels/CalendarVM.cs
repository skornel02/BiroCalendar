using BiroCalendar.Desktop.Helpers;
using BiroCalendar.Shared.Dtos;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Json;

namespace BiroCalendar.Desktop.ViewModels;

public class CalendarVM : BindableObject
{
    private int _biroCalendarId;
    public int BiroCalendarId
    {
        get => _biroCalendarId;
        set
        {
            _biroCalendarId = value;
            _ = Update();
        }
    }

    private bool onlyActives = true;
    public bool OnlyActives
    {
        get => onlyActives;
        set
        {
            onlyActives = value;
            OnPropertyChanged();
            _ = Update();
        }
    }

    public ObservableCollection<BiroTaskDto> Tasks { get; init; }

    private DateTime lastAccessed;
    public DateTime LastAccessed
    {
        get => lastAccessed;
        set
        {
            if (lastAccessed != value)
            {
                lastAccessed = value;
                OnPropertyChanged();
            }
        }
    }
    
    public ObservableCollection<string> Keys { get; init; }

    public RelayCommand RefreshCommand { get; init; } 

    public CalendarVM()
    {
        Tasks = [];
        Keys = [];
        RefreshCommand = new(_ => true, HandleRefresh);
    }

    public async Task Update()
    {
        var tasksRequest = await Globals.Client.Value.GetAsync($"/api/BiroAccount/{_biroCalendarId}/tasks?showAll={(!onlyActives ? "true" : "false")}");
        var tasks = await tasksRequest.Content.ReadFromJsonAsync<BiroTaskContainerDto>();

        Tasks.Clear();
        tasks?.Tasks.ForEach(Tasks.Add);

        LastAccessed = tasks?.LastAccessed ?? DateTime.MinValue;
    }

    public async void HandleRefresh(object? param)
    {
        var tasksRequest = await Globals.Client.Value.PostAsync($"/api/BiroAccount/{_biroCalendarId}/tasks/refresh", null);

        await Update();
    }
}
