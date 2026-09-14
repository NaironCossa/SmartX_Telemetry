using System;
using System.Collections.Generic;
using System.Text;
using SmartX_Telemetry.Models;

namespace SmartX_Telemetry.Services
{
    public class TelemetryBatchProcessor
    {
        // Method to process raw batches of telemetry using Jagged Arrays
        public List<TelemetryPacket<double>> ProcessHistoricalBatches()
        {
            // 1. Jagged Array: Simulating 3 batches of raw telemetry data coming from edge nodes
            double[][] rawTelemetryBatches = new double[3][];
            rawTelemetryBatches[0] = new double[] { 22.5, 23.1, 22.8 }; // Batch 1: Zone A Temps
            rawTelemetryBatches[1] = new double[] { 450.2, 465.8 };      // Batch 2: Zone B Wattage
            rawTelemetryBatches[2] = new double[] { 12.0, 11.5, 12.2, 11.8 }; // Batch 3: Zone C Moisture

            // 2. Optimised Collection: Preparing the generic List<T> to store the formatted data
            List<TelemetryPacket<double>> processedCollection = new List<TelemetryPacket<double>>();

            // 3. Transferring data from the jagged array into the List<T>
            for (int batchIndex = 0; batchIndex < rawTelemetryBatches.Length; batchIndex++)
            {
                for (int readingIndex = 0; readingIndex < rawTelemetryBatches[batchIndex].Length; readingIndex++)
                {
                    double rawValue = rawTelemetryBatches[batchIndex][readingIndex];

                    // Wrapping the raw double into our generic TelemetryPacket<T>
                    var packet = new TelemetryPacket<double>(
                        deviceId: $"ESP32_Node_{batchIndex}_{readingIndex}",
                        sensorValue: rawValue,
                        locationZone: $"Zone_{batchIndex + 1}"
                    );

                    processedCollection.Add(packet);
                }
            }

            return processedCollection; // Returning the highly optimized Generic Collection
        }
    }
}