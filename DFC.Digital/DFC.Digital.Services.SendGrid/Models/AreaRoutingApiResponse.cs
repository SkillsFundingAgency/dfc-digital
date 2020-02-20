namespace DFC.Digital.Services.SendGrid.Models
{
    public class AreaRoutingApiResponse
    {
        public string TouchpointID { get; set; }

        public string Area { get; set; }

        public string TelephoneNumber { get; set; }

        public string SMSNumber { get; set; }

        public string EmailAddress { get; set; }
    }
}