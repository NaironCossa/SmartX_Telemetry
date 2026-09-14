using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SmartX_Telemetry.Models;

namespace SmartX_Telemetry.Services
{
    public class TelemetryStreamService
    {
        private CancellationTokenSource _cts;

        // Starts background telemetry generation off the main UI thread
        public async Task StartStreamingAsync(IProgress<TelemetryPacket<double>> progress, CancellationToken cancellationToken)
        {
            var random = new Random();
            int packetCounter = 1;

            await Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Generate simulated telemetry readings on a background thread
                    double simulatedTemp = Math.Round(20.0 + (random.NextDouble() * 15.0), 2);

                    var packet = new TelemetryPacket<double>(
                        deviceId: $"ESP32_STREAM_{packetCounter++}",
                        sensorValue: simulatedTemp,
                        locationZone: "Zone_A_RealTime"
                    );

                    // Report progress back to the UI thread safely
                    progress?.Report(packet);

                    // Non-blocking delay between transmissions
                    await Task.Delay(1000, cancellationToken);
                }
            }, cancellationToken);
        }
    }
}