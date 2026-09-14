using System;
using System.Collections.Generic;
using System.Text;
using SmartX_Telemetry.Models;

namespace SmartX_Telemetry.Services
{
    public class ConfigurationValidator
    {
        // Recursive method to parse multi-tier configuration profiles
        public List<string> ValidateDeploymentTree(DeviceNode node, string currentPath = "")
        {
            List<string> validationErrors = new List<string>();

            // Build the current hierarchical path (e.g., Facility A -> Zone 1 -> Sub-Zone B)
            string nodePath = string.IsNullOrEmpty(currentPath) ? node.NodeName : $"{currentPath} -> {node.NodeName}";

            // Validation Logic: Check if the current node is safely configured
            if (!node.IsSafelyConfigured)
            {
                validationErrors.Add($"CRITICAL FAULT: {nodePath} - Node configuration is unstable or unsecured.");
            }

            // Recursive Step: Iterate through all child nodes (Base case is reached when Children.Count == 0)
            if (node.Children != null && node.Children.Count > 0)
            {
                foreach (var child in node.Children)
                {
                    // Recursively call the validation on each child, passing the accumulated path
                    validationErrors.AddRange(ValidateDeploymentTree(child, nodePath));
                }
            }

            return validationErrors;
        }
    }
}