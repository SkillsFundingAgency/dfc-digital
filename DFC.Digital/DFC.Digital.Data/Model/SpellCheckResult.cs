namespace DFC.Digital.Data.Model
{
    public class SpellCheckResult
    {
        public string OriginalTerm { get; set; }

        public string CorrectedTerm { get; set; }

        public bool HasCorrected { get; set; }
    }
}