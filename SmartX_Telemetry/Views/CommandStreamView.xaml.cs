using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Threading;
using SmartX_Telemetry.Models;
using SmartX_Telemetry.Services;

namespace SmartX_Telemetry.Views
{
    public partial class CommandStreamView : UserControl
    {
        private TelemetryStreamService _streamService;
        private CancellationTokenSource _cts;

        public CommandStreamView()
        {
            InitializeComponent();
            _streamService = new TelemetryStreamService();
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            BtnStart.IsEnabled = false;
            BtnStop.IsEnabled = true;
            _cts = new CancellationTokenSource();

            // Thread-safe progress handler for UI updates
            var progress = new Progress<TelemetryPacket<double>>(packet =>
            {
                LstTelemetryLog.Items.Add(packet.ToString());
                LstTelemetryLog.ScrollIntoView(packet.ToString());
            });

            try
            {
                LstTelemetryLog.Items.Add("--- SYSTEM STREAM STARTED (BACKGROUND THREAD) ---");
                await _streamService.StartStreamingAsync(progress, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                LstTelemetryLog.Items.Add("--- STREAM STOPPED BY OPERATOR ---");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Stream Error: {ex.Message}");
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            _cts?.Cancel();
            BtnStart.IsEnabled = true;
            BtnStop.IsEnabled = false;
        }
    }
}
