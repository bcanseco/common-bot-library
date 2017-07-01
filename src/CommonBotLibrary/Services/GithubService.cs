using System.Net;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using CommonBotLibrary.Extensions;
using CommonBotLibrary.Interfaces;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace CommonBotLibrary.Services
{
    public class GithubService : IVersionControlService
    {
        /// <summary>
        ///   Constructs an <see cref="IVersionControlService"/> implementation that accesses Github.
        /// </summary>
        /// <param name="username">Defaults to username in <see cref="Tokens"/> if null.</param>
        /// <param name="repository">Defaults to repository in <see cref="Tokens"/> if null.</param>
        /// <param name="oauth">Defaults to oauth in <see cref="Tokens"/> if null.</param>
        /// <exception cref="InvalidCredentialsException"></exception>
        public GithubService(string username = null, string repository = null, string oauth = null)
        {
            Username = username ?? Tokens.Github?.Username;
            Repository = repository ?? Tokens.Github?.Repository;
            OAuth = oauth ?? Tokens.Github?.OAuth;

            if (string.IsNullOrWhiteSpace(Username)
                || string.IsNullOrWhiteSpace(Repository)
                || string.IsNullOrWhiteSpace(OAuth))
            {
                var msg = $"Github tokens are required. Did you call {nameof(Tokens.LoadAsync)}?";
                throw new InvalidCredentialsException(msg);
            }
        }

        private string Username { get; }
        private string Repository { get; }
        private string OAuth { get; }

        /// <summary>
        ///   Creates a new issue.
        /// </summary>
        /// <param name="title">The title of the issue (required).</param>
        /// <param name="body">The body of the issue.</param>
        /// <param name="labels">A collection of labels to attempt to apply to the issue.</param>
        /// <returns>True if the issue was successfully created, false otherwise.</returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        /// <exception cref="ResultNotFoundException">
        ///   Thrown if the repository and/or username was not found.
        /// </exception>
        /// <exception cref="InvalidCredentialsException"></exception>
        /// <seealso href="https://help.github.com/articles/github-terms-of-service/">TOS</seealso>
        public async Task<bool> CreateIssueAsync(string title, string body = null, string[] labels = null)
        {
            using (var client = new RestClient("https://api.github.com"))
            {
                var resource = $"repos/{Username}/{Repository}/issues?access_token={OAuth}";
                var request = new RestRequest(resource, Method.POST);
                request.AddJsonBody(new {title, body, labels});

                var response = await client.ExecuteAsync(request);

                return response.StatusCode == HttpStatusCode.Created;
            }
        }
    }
}
