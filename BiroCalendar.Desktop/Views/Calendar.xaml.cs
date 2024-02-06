using BiroCalendar.Desktop.ViewModels;
using System.Windows;

namespace BiroCalendar.Desktop.Views;
/// <summary>
/// Interaction logic for Calendar.xaml
/// </summary>
public partial class Calendar : Window
{
    public Calendar(int BiroAccountId)
    {
        InitializeComponent();
        ((CalendarVM)DataContext).BiroCalendarId = BiroAccountId;
    }
}
