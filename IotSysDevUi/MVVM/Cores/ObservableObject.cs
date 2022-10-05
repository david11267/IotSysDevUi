using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IotSysDevUi.MVVM;

//WTF DOES THIS DOO
internal class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}