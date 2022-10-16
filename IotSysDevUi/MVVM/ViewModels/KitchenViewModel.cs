using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using IotSysDevUi.MVVM.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;

namespace IotSysDevUi.MVVM.ViewModels;

internal class KitchenViewModel
{
    private DispatcherTimer timer;
    private ObservableCollection<DeviceItem> _deviceItems;
    private List<DeviceItem> _tempList;
    private readonly RegistryManager registryManager = RegistryManager.CreateFromConnectionString("HostName=SharedHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=MAZ7jkUzHFnph4OvNsuvjcJxQLS0dcHRa7TV5g0/Rzw=");

    public KitchenViewModel()
    {
        _tempList = new List<DeviceItem>();
        _deviceItems = new ObservableCollection<DeviceItem>();
        PopulateDeviceItemsAsync().ConfigureAwait(false);
        SetInterval(TimeSpan.FromSeconds(3));
        
    }

    
    public string Title { get; set; } = "Kitchen";
    public string Temperature { get; set; } = generateRandomNumber(10,22)+" °C";
    public string Humidity { get; set; } =  generateRandomNumber(1,99)+" %";
    public IEnumerable<DeviceItem> DeviceItems => _deviceItems;

    private static string generateRandomNumber(int minValue, int maxValue)
    {
        Random rnd = new Random();
        return rnd.Next(minValue, maxValue).ToString();
    }

    private void SetInterval(TimeSpan interval)
    {
        timer = new DispatcherTimer()
        {
            Interval = interval
        };

        timer.Tick += new EventHandler(timer_tick);
        timer.Start();
    }

    private async void timer_tick(object sender, EventArgs e)
    {
        await PopulateDeviceItemsAsync();
        await UpdateDeviceItemsAsync();
    }


    private async Task UpdateDeviceItemsAsync()
    {
        _tempList.Clear();

        foreach (var item in _deviceItems)
        {
            var device = await registryManager.GetDeviceAsync(item.DeviceId);
            if (device == null)
                _tempList.Add(item);
        }

      

        foreach (var item in _tempList)
        {
            _deviceItems.Remove(item);
        }
    }

    private async Task PopulateDeviceItemsAsync()
    {
        //var result = registryManager.CreateQuery("select * from devices where location = 'kitchen'");
        var result = registryManager.CreateQuery("SELECT * FROM devices where properties.reported.location = 'kitchen'"); //where properties.reported.location = 'kitchen'

        if (result.HasMoreResults)
        {
            foreach (Twin twin in await result.GetNextAsTwinAsync())
            {
                var device = _deviceItems.FirstOrDefault(x => x.DeviceId == twin.DeviceId);

                if (device == null)
                {
                    device = new DeviceItem
                    {
                        DeviceId = twin.DeviceId,
                    };

                    try { device.DeviceName = twin.Properties.Reported["deviceName"]; }
                    catch { device.DeviceName = device.DeviceId; }
                    try { device.DeviceType = twin.Properties.Reported["deviceType"]; }
                    catch { }


                    switch (device.DeviceType.ToLower())
                    {
                        case "fan":
                            device.IconActive = "\uf863";
                            device.IconInActive = "\uf863";
                            device.StateActive = "ON";
                            device.StateInActive = "OFF";
                            break;

                        case "light":
                            device.IconActive = "\uf672";
                            device.IconInActive = "\uf0eb";
                            device.StateActive = "ON";
                            device.StateInActive = "OFF";
                            break;

                        default:
                            device.IconActive = "\uf2db";
                            device.IconInActive = "\uf2db";
                            device.StateActive = "ENABLE";
                            device.StateInActive = "DISABLE";
                            break;

                        
                    }
                    _deviceItems.Add(device);
                }
                else { }
            }
        }
        else
        {
            _deviceItems.Clear();
        }
    }
}