using SmartX_Telemetry.Views;
using System.Windows;

namespace SmartX_Telemetry
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Wire up the button click event
            BtnIngestion.Click += BtnIngestion_Click;

            // Load the ingestion view by default on startup
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
    }
}