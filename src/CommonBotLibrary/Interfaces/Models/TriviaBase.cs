namespace CommonBotLibrary.Interfaces.Models
{
    public abstract class TriviaBase
    {
        public string Category { get; set; }
        public string Question { get; set; }

        public override string ToString() => Question;
    }
}
