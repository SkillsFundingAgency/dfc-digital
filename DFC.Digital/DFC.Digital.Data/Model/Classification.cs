using DFC.Digital.Data.Interfaces;
using System;

namespace DFC.Digital.Data.Model
{
    public class Classification : IDigitalDataModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        //public string Description { get; set; }
    }
}
