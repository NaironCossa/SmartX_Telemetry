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
using SmartX_Telemetry.Models;
using SmartX_Telemetry.Services;

namespace SmartX_Telemetry.Views
{
    public partial class MeshRoutingView : UserControl
    {
        private DeviceNode _rootNode;
        private ConfigurationValidator _validator;

        public MeshRoutingView()
        {
            InitializeComponent();
            _validator = new ConfigurationValidator();
            BuildSampleTopology();
        }

        private void BuildSampleTopology()
        {
            // Constructing a nested tree representation of the Smart-X mesh
            _rootNode = new DeviceNode("Gateway_Master_Alpha", isSafelyConfigured: true);

            var zoneA = new DeviceNode("SubZone_Hydroponics", isSafelyConfigured: true);
            var zoneB = new DeviceNode("SubZone_PowerStation", isSafelyConfigured: true);

            // Adding nested child devices
            zoneA.AddChild(new DeviceNode("ESP32_Soil_Sensor_01", isSafelyConfigured: true));
            zoneA.AddChild(new DeviceNode("ESP32_Soil_Sensor_02", isSafelyConfigured: false)); // Faulty Node

            zoneB.AddChild(new DeviceNode("SmartMeter_Grid_01", isSafelyConfigured: true));

            _rootNode.AddChild(zoneA);
            _rootNode.AddChild(zoneB);

            // Bind root to WPF TreeView
            TvNetworkTopology.ItemsSource = new List<DeviceNode> { _rootNode };
        }

        private void BtnValidateTree_Click(object sender, RoutedEventArgs e)
        {
            LstValidationResults.Items.Clear();

            // Execute the recursive algorithm created in Step 9
            List<string> errors = _validator.ValidateDeploymentTree(_rootNode);

            if (errors.Count == 0)
            {
                LstValidationResults.Items.Add("SUCCESS: All nodes in the recursive topology are safely configured.");
            }
            else
            {
                foreach (var err in errors)
                {
                    LstValidationResults.Items.Add(err);
                }
            }
        }
    }
}