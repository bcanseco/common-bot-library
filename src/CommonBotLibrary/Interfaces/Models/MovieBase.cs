namespace CommonBotLibrary.Interfaces.Models
{
    public abstract class MovieBase
    {
        public string Title { get; set; }
        public string Year { get; set; }

        public override string ToString() => Title;
    }
}
