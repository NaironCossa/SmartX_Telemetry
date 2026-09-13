using System;
using System.Collections.Generic;
using System.Text;

namespace SmartX_Telemetry.Models
{
    // The generic type <T> allows this single class to handle any IoT data type
    public class TelemetryPacket<T>
    {
        public string DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
        public T SensorValue { get; set; }
        public string LocationZone { get; set; }

        // Constructor overloading: Default constructor
        public TelemetryPacket()
        {
            Timestamp = DateTime.Now;
        }

        // Constructor overloading: Parameterized constructor
        public TelemetryPacket(string deviceId, T sensorValue, string locationZone)
        {
            DeviceId = deviceId;
            SensorValue = sensorValue;
            LocationZone = locationZone;
            Timestamp = DateTime.Now;
        }

        // Method to format the packet for dashboard display
        public override string ToString()
        {
            return $"[{Timestamp:HH:mm:ss}] {DeviceId} ({LocationZone}): {SensorValue}";
        }
    }
}
