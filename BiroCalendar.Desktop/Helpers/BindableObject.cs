using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BiroCalendar.Desktop.Helpers;

public class BindableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? property = null)
    {
        PropertyChanged?.Invoke(this, new(property));
    }
}
