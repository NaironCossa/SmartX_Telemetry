using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SmartX_Telemetry.Exceptions;
using SmartX_Telemetry.Models;

namespace SmartX_Telemetry.Services
{
    public static class TelemetryValidator
    {
        // Validates against basic injection attacks and dangerous characters
        private static bool IsSafeString(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            // Rejects HTML tags, SQL comment dashes, and semicolons
            return !Regex.IsMatch(input, @"(<script|<html|--|;|')");
        }

        public static bool ValidatePacket(TelemetryPacket<double> packet)
        {
            if (!IsSafeString(packet.DeviceId) || !IsSafeString(packet.LocationZone))
            {
                throw new TelemetryValidationException("SECURITY ALERT: Payload contains forbidden characters or is empty.");
            }

            // Domain-specific physics validation (e.g., temperature cannot be below absolute zero)
            if (packet.SensorValue < -273.15)
            {
                throw new TelemetryValidationException("DATA INTEGRITY ALERT: Sensor value is physically impossible (below absolute zero).");
            }

            return true;
        }
    }
}