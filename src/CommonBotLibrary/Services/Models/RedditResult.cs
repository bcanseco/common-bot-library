using System.Diagnostics;
using CommonBotLibrary.Interfaces.Models;
using RedditSharp.Things;

namespace CommonBotLibrary.Services.Models
{
    [DebuggerDisplay("{Url, nq}")]
    public class RedditResult : IWebpage
    {
        public RedditResult(Post post)
        {
            Title = post.Title;
            Url = post.Url.OriginalString;

            Post = post;
        }

        /// <summary>
        ///   The wrapped model returned by RedditSharp with more submission props.
        /// </summary>
        public Post Post { get; }
        public string Url { get; set; }
        public string Title { get; set; }

        public static implicit operator Post(RedditResult redditResult)
            => redditResult.Post;

        public enum PostCategory
        {
            New,
            Controversial,
            Rising,
            Hot,
            Top
        }
    }
}
