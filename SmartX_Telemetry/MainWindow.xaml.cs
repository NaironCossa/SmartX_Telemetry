using SmartX_Telemetry.Views;
using System.Windows;

namespace SmartX_Telemetry
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Log application launch
            Services.TelemetryLoggerService.LogInfo("Smart-X Application Gateway initialized successfully.");

            BtnIngestion.Click += BtnIngestion_Click;
            MainFrame.Navigate(new IngestionView());
        }
        private void BtnIngestion_Click(object sender, RoutedEventArgs e)
        {
            // Navigate the central frame to our custom UserControl
            MainFrame.Navigate(new IngestionView());
        }

        private void BtnCommandStream_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CommandStreamView());
        }

        private void BtnMeshRouting_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MeshRoutingView());
        }
    }
}