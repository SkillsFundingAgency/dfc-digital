﻿namespace DFC.Digital.Data.Interfaces
{
    public enum ServiceState
    {
        Red,
        Green,
        Amber
    }

    public class ServiceStatus
    {
        public string Name { get; set; }

        public ServiceState Status { get; set; }

        public string CheckCorrelationId { get; set; }
    }
}