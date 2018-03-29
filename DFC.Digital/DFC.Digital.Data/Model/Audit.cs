using System;

namespace DFC.Digital.Data.Model
{
    public class Audit
    {
        public Guid CorrelationId { get; set; }

        public DateTime Timestamp { get; set; }

        public object Data { get; set; }
    }
}