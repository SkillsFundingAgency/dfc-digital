namespace DFC.Digital.Data.Model
{
    public class DfcOnetAttributesData : OnetEntity
    {
        public string Attribute { get; set; }

        public string ElementDescription { get; set; }

        public string ElementName { get; set; }

        public decimal Value { get; set; }
    }
}