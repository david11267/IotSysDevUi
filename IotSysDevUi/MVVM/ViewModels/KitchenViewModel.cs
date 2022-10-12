using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using IotSysDevUi.MVVM.Models;
using Microsoft.Azure.Devices;

namespace IotSysDevUi.MVVM.ViewModels;

internal class KitchenViewModel
{
    private readonly ObservableCollection<DeviceItem> _deviceItems;
    private readonly RegistryManager registryManager = RegistryManager.CreateFromConnectionString("HostName=SharedHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=MAZ7jkUzHFnph4OvNsuvjcJxQLS0dcHRa7TV5g0/Rzw=");

    public KitchenViewModel()
    {
        _deviceItems = new ObservableCollection<DeviceItem>();
        
    }
    public string Title { get; set; } = "Kitchen";
    public string Temperature { get; set; } = "23 °C";
    public string Humidity { get; set; } = "34 %";

    public IEnumerable<DeviceItem> DeviceItems => _deviceItems;

    private async Task PopulateDeviceItems()
    {
        var result = registryManager.CreateQuery("SELECT * FROM DEVICES"); 
    }
}