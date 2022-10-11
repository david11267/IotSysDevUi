using System.Collections.Generic;
using System.Threading.Tasks;
using IotSysDevUi.MVVM.Models;
using Microsoft.Azure.Devices;

namespace IotSysDevUi.MVVM.ViewModels;

internal class KitchenViewModel
{
    public string Title { get; set; } = "Kitchen";
    public string Temperature { get; set; } = "23 °C";
    public string Humidity { get; set; } = "34 %";

}