using System.Collections.Generic;
using IotSysDevUi.MVVM.Models;

namespace IotSysDevUi.MVVM.ViewModels;

public class KitchenViewModel
{
    public string Title { get; set; } = "Kitchen";
    public string Temperature { get; set; } = "23 °C";
    public string Humidity { get; set; } = "34 %";

    public List<DeviceItem> Devices { get; set; } = new()
    {
    new DeviceItem{DeviceId="Device 1", DeviceName="IntelliLight", DeviceType="Light", StateActive="ON",StateInActive="OFF" },
        new DeviceItem { DeviceId = "Device 2", DeviceName = "IntelliFan", DeviceType = "Fan", StateActive = "ON", StateInActive = "OFF" },
        new DeviceItem { DeviceId = "Device 3", DeviceName = "IntelliLedstrip", DeviceType = "Led strip", StateActive = "ON", StateInActive = "OFF" }
    };

}