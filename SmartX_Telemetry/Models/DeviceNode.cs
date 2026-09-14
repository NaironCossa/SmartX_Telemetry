using System;
using System.Collections.Generic;
using System.Text;


namespace SmartX_Telemetry.Models
{
    public class DeviceNode
    {
        public string NodeName { get; set; }
        public bool IsSafelyConfigured { get; set; }
        public List<DeviceNode> Children { get; set; }

        public DeviceNode(string nodeName, bool isSafelyConfigured = true)
        {
            NodeName = nodeName;
            IsSafelyConfigured = isSafelyConfigured;
            Children = new List<DeviceNode>(); // Initialising the generic collection for nested children
        }

        public void AddChild(DeviceNode child)
        {
            Children.Add(child);
        }
    }
}