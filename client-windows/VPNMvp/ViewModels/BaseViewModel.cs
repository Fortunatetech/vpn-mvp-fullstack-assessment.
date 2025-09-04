using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace VPNMvp.ViewModels
{
public class BaseViewModel : INotifyPropertyChanged
{
public event PropertyChangedEventHandler? PropertyChanged;


protected void Raise([CallerMemberName] string? name = null)
{
PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
}
}