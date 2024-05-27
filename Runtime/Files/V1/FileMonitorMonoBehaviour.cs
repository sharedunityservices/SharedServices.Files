using System.Collections.Generic;
using UnityEngine;

namespace SharedServices.Files.V1
{
    public class FileMonitorMonoBehaviour : MonoBehaviour
    {
        private readonly List<FileMonitor> _fileMonitors = new();
        
        public void AddFileMonitor(FileMonitor fileMonitor)
        {
            _fileMonitors.Add(fileMonitor);
        }
        
        public void RemoveFileMonitor(FileMonitor fileMonitor)
        {
            _fileMonitors.Remove(fileMonitor);
        }
        
        private void Update()
        {
            foreach (var fileMonitor in _fileMonitors) 
                fileMonitor.Update();
        }
    }
}