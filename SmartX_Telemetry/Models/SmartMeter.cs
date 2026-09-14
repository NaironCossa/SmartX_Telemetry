using System;
using System.Collections.Generic;
using System.Text;

namespace SmartX_Telemetry.Models
{
    public class SmartMeter
    {
        public string MeterId { get; set; }
        public double PowerWattage { get; set; }
        public string Zone { get; set; }

        public SmartMeter(string meterId, double powerWattage, string zone)
        {
            MeterId = meterId;
            PowerWattage = powerWattage;
            Zone = zone;
        }

        // Operator Overloading: Allows us to do Meter3 = Meter1 + Meter2 in our API/UI
        public static SmartMeter operator +(SmartMeter a, SmartMeter b)
        {
            // Validates that we aren't adding null meters
            if (a == null || b == null)
            {
                throw new ArgumentNullException("Cannot aggregate a null smart meter.");
            }

            // Creates a new aggregate meter representing the combined load
            return new SmartMeter(
                meterId: $"{a.MeterId}_{b.MeterId}_Aggregated",
                powerWattage: a.PowerWattage + b.PowerWattage,
                zone: a.Zone == b.Zone ? a.Zone : "Mixed_Zone"
            );
        }

        // Overloading the > operator for delta comparisons
        public static bool operator >(SmartMeter a, SmartMeter b)
        {
            return a.PowerWattage > b.PowerWattage;
        }

        public static bool operator <(SmartMeter a, SmartMeter b)
        {
            return a.PowerWattage < b.PowerWattage;
        }
    }
}