using System;
using System.IO;
using UnityEngine;

namespace SharedServices.Files.V1
{
    public class FileMonitor
    {
        private string _path;
        private string _contents;
        private long _lastModified;
        
        private event Action<string> ContentsChanged_Private;
        
        public event Action<string> ContentsChanged
        {
            add
            {
                ContentsChanged_Private += value;
                value.Invoke(_contents);
            }
            remove
            {
                ContentsChanged_Private -= value;
            }
        }
        
        public FileMonitor(string path)
        {
            _path = path;
            _contents = File.ReadAllText(path);
            _lastModified = File.GetLastWriteTime(path).Ticks;
        }

        public void Update()
        {
            var lastModified = File.GetLastWriteTime(_path).Ticks;
            if (lastModified == _lastModified) return;
            try
            {
                _contents = File.ReadAllText(_path);
                _lastModified = lastModified;
                ContentsChanged_Private?.Invoke(_contents);
            }
            catch (IOException e)
            {
                Debug.LogWarning(e.ToString());
            }
        }
    }
}