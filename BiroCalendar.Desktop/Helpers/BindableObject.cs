using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BiroCalendar.Desktop.Helpers;

internal class BindableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? property = null)
    {
        PropertyChanged?.Invoke(this, new(property));
    }
}
