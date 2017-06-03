namespace CommonBotLibrary.Interfaces.Models
{
    public abstract class DefinitionBase
    {
        public string Word { get; set; }
        public string Definition { get; set; }

        public override string ToString() => Definition;
    }
}
