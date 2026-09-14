# Smart-X Telemetry and Mesh Management Platform

A C# WPF desktop client and ASP.NET Core Minimal API backend built for IoT sensor data ingestion, mesh routing validation, and asynchronous telemetry streaming.

## System Architecture

The solution is divided into two primary projects:

- **WPF Desktop Client (`SmartX_Telemetry`)**: Manages UI navigation, local data transformation, recursive network inspection, and real-time streaming displays.
- **ASP.NET Core Web API (`SmartX_Telemetry.Api`)**: Handles data validation endpoints, batch payload ingestion, and log processing.

```text
SmartX_Telemetry/
├── Models/
│   ├── TelemetryPacket.cs       # Generic packet wrapper
│   ├── SmartMeter.cs            # Domain model with operator overloading (+, >)
│   └── DeviceNode.cs            # Recursive tree node model
├── Services/
│   ├── ApiClientService.cs      # Asynchronous HTTP API gateway client
│   ├── TelemetryBatchProcessor.cs # Jagged array transformation logic
│   ├── ConfigurationValidator.cs  # Recursive hierarchy validation algorithm
│   └── TelemetryLoggerService.cs # Thread-safe file logging engine
├── Views/
│   ├── IngestionView.xaml       # Sensor registration & regex validation UI
│   ├── CommandStreamView.xaml  # Dynamic multithreaded simulation UI
│   └── MeshRoutingView.xaml     # Recursive WPF TreeView network routing UI
├── MainWindow.xaml              # Multi-pillar WPF navigation shell
└── Program.cs / Web API         # ASP.NET Core Minimal API backend
