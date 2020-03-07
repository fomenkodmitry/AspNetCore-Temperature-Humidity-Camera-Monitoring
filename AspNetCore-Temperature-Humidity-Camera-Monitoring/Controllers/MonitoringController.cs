using System;
using Iot.Device.DHTxx;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore_Temperature_Humidity_Camera_Monitoring.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonitoringController : ControllerBase
    {

        [HttpGet]
        public ActionResult Get()
        {
            using var dht = new Dht22(4);
            Console.WriteLine($"Temperature: {dht.Temperature.Celsius:0.0}°C, Humidity: {dht.Humidity:0.0}%");
            return Ok(new
            {
                Temperature = dht.Temperature.Celsius.ToString("0.0"),
                Humidity = dht.Humidity.ToString("0.0")
            });
        }
    }
}