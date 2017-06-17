using System.Linq;

namespace CommonBotLibrary.Extensions
{
    public static class StringExtensions
    {
        public static string NoPunctuation(this string s)
            => new string(s.Where(c => !char.IsPunctuation(c)).ToArray());
    }
}
