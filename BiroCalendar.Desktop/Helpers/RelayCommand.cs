using System.Windows.Input;

namespace BiroCalendar.Desktop.Helpers;

public class RelayCommand : ICommand
{
    private Func<object?, bool> CanExecuteCallback;
    private Action<object?> ExecuteCallback;

    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Func<object?, bool> canExecuteCallback, Action<object?> executeCallback)
    {
        CanExecuteCallback = canExecuteCallback;
        ExecuteCallback = executeCallback;
    }

    public bool CanExecute(object? parameter)
    {
        return CanExecuteCallback(parameter);
    }

    public void Execute(object? parameter)
    {
        ExecuteCallback(parameter);
    }

    public void Changed()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
