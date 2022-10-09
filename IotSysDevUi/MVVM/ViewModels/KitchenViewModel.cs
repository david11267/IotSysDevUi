using System.Collections.Generic;
using System.Threading.Tasks;
using IotSysDevUi.MVVM.Models;
using Microsoft.Azure.Devices;

namespace IotSysDevUi.MVVM.ViewModels;

internal class KitchenViewModel : ObservableObject
{
    private readonly RegistryManager registryManager = RegistryManager.CreateFromConnectionString("HostName=SharedHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=MAZ7jkUzHFnph4OvNsuvjcJxQLS0dcHRa7TV5g0/Rzw=");
  

    public KitchenViewModel()
    {
        Devices = new();
        GetDevices().ConfigureAwait(false);
    }
   

    public string Title { get; set; } = "Kitchen";
    public string Temperature { get; set; } = "23 °C";
    public string Humidity { get; set; } = "34 %";

    public List<DeviceItem> Devices { get; set; } = new();
  

    private async Task GetDevices()
    {
        var result= registryManager.CreateQuery("SELECT * FROM devices");
        if (result.HasMoreResults)
        {
            foreach (var device in await result.GetNextAsTwinAsync())
            {
                var deviceItem = new DeviceItem()
                {
                    DeviceId = device.DeviceId
                };

                try
                {
                    deviceItem.DeviceName = (string) device.Properties.Reported["deviceName"];
                }
                catch{}

                switch (deviceItem.DeviceType.ToLower())
                {
                    case "fan":
                        deviceItem.IconActive = "\uf863";  //these are c# icon names
                        deviceItem.IconInActive = "\uf863";
                        deviceItem.StateActive = "ON";
                        deviceItem.StateInActive = "OFF";
                        break;
                    case "light":
                        deviceItem.IconActive = "\uf672";
                        deviceItem.IconInActive = "\uf0eb";
                        deviceItem.StateActive = "ON";
                        deviceItem.StateInActive = "OFF";
                        break;

                    default:
                        deviceItem.IconActive = "\uf2db";
                        deviceItem.IconInActive = "\uf2db";
                        deviceItem.StateActive = "ENABLE";
                        deviceItem.StateInActive = "DISABLE";
                        break;
                }
                Devices.Add(deviceItem);
            }

            
        }

        OnPropertyChanged();
    }

}