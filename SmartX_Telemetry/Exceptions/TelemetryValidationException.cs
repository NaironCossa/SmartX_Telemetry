using System;
using System.Collections.Generic;
using System.Text;

namespace SmartX_Telemetry.Exceptions
{
    // Inheriting from the base Exception class
    public class TelemetryValidationException : Exception
    {
        public TelemetryValidationException()
            : base("Critical failure: The telemetry data failed validation.") { }

        public TelemetryValidationException(string message)
            : base(message) { }

        public TelemetryValidationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
