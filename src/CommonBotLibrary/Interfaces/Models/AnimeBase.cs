namespace CommonBotLibrary.Interfaces.Models
{
    public abstract class AnimeBase
    {
        public string Title { get; set; }
        public string English { get; set; }
        public string Synonyms { get; set; }
        public int Episodes { get; set; }
        public double Score { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Image { get; set; }
        public string Synopsis { get; set; }

        public override string ToString() => Title;
    }
}
