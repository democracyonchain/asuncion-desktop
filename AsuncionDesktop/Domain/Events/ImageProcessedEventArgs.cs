using System;
using AsuncionDesktop.Domain.Entities;

namespace AsuncionDesktop.Domain.Events
{
    public class ImageProcessedEventArgs : EventArgs
    {
        public Acta Acta { get; set; }
        public string FilePath { get; set; }
    }
}
