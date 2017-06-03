namespace CommonBotLibrary.Interfaces.Models
{
    public abstract class SymbolBase
    {
        public string Symbol { get; set; }
        public string Name { get; set; }

        public override string ToString() => Symbol;
    }
}
