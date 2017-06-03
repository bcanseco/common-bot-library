namespace CommonBotLibrary.Interfaces.Models
{
    /// <remarks>
    ///   This is an interface rather than an abstract class because the
    ///   other models may also be webpages. This lets implementations inherit
    ///   both this interface and their less generic base models.
    /// </remarks>
    public interface IWebpage
    {
        string Url { get; }
        string Title { get; }
    }
}
