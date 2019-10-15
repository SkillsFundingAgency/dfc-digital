using System;

namespace DFC.Digital.Data.Model
{
    public class MoreInformationLink
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Uri Url { get; set; }

        public string Text { get; set; }

        public string CType { get; set; }
    }
}