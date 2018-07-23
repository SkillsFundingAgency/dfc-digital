namespace DFC.Digital.Repository.ONET
{
    public class OnetAttribute
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public AttributeType Type { get; internal set; }
        public string OnetOccupationalCode { get; internal set; }
        public string SocCode { get; internal set; }
        public decimal Score { get; internal set; }
    }
}