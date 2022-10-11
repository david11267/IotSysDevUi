using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IotSysDevUi.MVVM.Models;
using Microsoft.Azure.Devices;

namespace IotSysDevUi.MVVM.Views
{
    /// <summary>
    /// Interaction logic for KitchenView.xaml
    /// </summary>
    public partial class KitchenView : UserControl
    {
        private readonly RegistryManager registryManager = RegistryManager.CreateFromConnectionString("HostName=SharedHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=MAZ7jkUzHFnph4OvNsuvjcJxQLS0dcHRa7TV5g0/Rzw=");
        private List<DeviceItem> devices = new List<DeviceItem>();
        public KitchenView()
        {
            InitializeComponent();
        }

        private async Task<List<DeviceItem>> GetDevices()
        {
            var _devices = new List<DeviceItem>();
            var result = registryManager.CreateQuery("SELECT * FROM devices");
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
                        deviceItem.DeviceName = (string)device.Properties.Reported["deviceName"];
                    }
                    catch { }

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
                    _devices.Add(deviceItem);
                }


            }

            return _devices;
        }
    }
}
