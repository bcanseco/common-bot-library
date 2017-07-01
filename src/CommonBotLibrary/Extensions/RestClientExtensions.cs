using System.Net.Http;
using System.Threading.Tasks;
using CommonBotLibrary.Exceptions;
using RestSharp.Portable;

namespace CommonBotLibrary.Extensions
{
    public static class RestClientExtensions
    {
        /// <summary>
        ///   Executes an HTTP request that will throw custom exceptions for 401 and 404.
        /// </summary>
        /// <exception cref="InvalidCredentialsException"></exception>
        /// <exception cref="ResultNotFoundException"></exception>
        public static async Task<IRestResponse> ExecuteAsync(
            this RestClientBase client, IRestRequest request)
        {
            try
            {
                return await client.Execute(request);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("401"))
            {
                const string msg = "401 was returned for the given username and password.";
                throw new InvalidCredentialsException(msg, ex);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("403"))
            {
                const string msg = "403 was returned for the given request.";
                throw new InvalidCredentialsException(msg, ex);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("404"))
            {
                const string msg = "404 was returned for the given query.";
                throw new ResultNotFoundException(msg, ex);
            }
        }
    }
}
