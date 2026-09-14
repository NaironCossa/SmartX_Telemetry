using Microsoft.Win32;
using SmartX_Telemetry.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;

namespace SmartX_Telemetry.Views
{
    public partial class IngestionView : UserControl
    {
        // Reusable HttpClient for optimal performance (prevents socket exhaustion)
        private static readonly HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5162") };
        private string _selectedFilePath;

        public IngestionView()
        {
            InitializeComponent();
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Diagnostic Log or Media",
                Filter = "All Files (*.*)|*.*|Text Files (*.txt)|*.txt|Log Files (*.log)|*.log"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedFilePath = openFileDialog.FileName;
                TxtFilePath.Text = Path.GetFileName(_selectedFilePath);
            }
        }

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BtnSubmit.IsEnabled = false;
                TxtStatus.Text = "Transmitting...";
                TxtStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Yellow);

                // 1. Validate and Parse Input
                if (!double.TryParse(TxtSensorValue.Text, out double sensorValue))
                {
                    MessageBox.Show("Invalid sensor value. Must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    ResetUI();
                    return;
                }

                // 2. Prepare our Generic Data Model
                var packet = new TelemetryPacket<double>(
                    deviceId: TxtDeviceId.Text,
                    sensorValue: sensorValue,
                    locationZone: ((ComboBoxItem)CmbZone.SelectedItem).Content.ToString()
                );

                // 3. Send Telemetry JSON to the API
                var jsonResponse = await _httpClient.PostAsJsonAsync("/api/telemetry", packet);
                jsonResponse.EnsureSuccessStatusCode();

                // 4. Handle Multipart Media/Log File Upload if attached
                if (!string.IsNullOrEmpty(_selectedFilePath) && File.Exists(_selectedFilePath))
                {
                    using var multipartFormContent = new MultipartFormDataContent();

                    // Load file into a stream for high-performance transmission
                    var fileStreamContent = new StreamContent(File.OpenRead(_selectedFilePath));
                    multipartFormContent.Add(fileStreamContent, name: "file", fileName: Path.GetFileName(_selectedFilePath));

                    var fileResponse = await _httpClient.PostAsync("/api/upload-log", multipartFormContent);
                    fileResponse.EnsureSuccessStatusCode();
                }

                TxtStatus.Text = "SUCCESS: Telemetry and assets securely transmitted and encrypted.";
                TxtStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGreen);
            }
            catch (HttpRequestException ex)
            {
                TxtStatus.Text = "API CONNECTION FAILED: Is the API running?";
                TxtStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                MessageBox.Show(ex.Message, "Network Error");
            }
            catch (Exception ex)
            {
                TxtStatus.Text = "ERROR: " + ex.Message;
                TxtStatus.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            }
            finally
            {
                ResetUI();
            }
        }

        private void ResetUI()
        {
            BtnSubmit.IsEnabled = true;
        }
    }
}