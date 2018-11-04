using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace eMailService.Results
{
    public class CustomResponse : IHttpActionResult
    {
        public dynamic Result { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public CustomResponse(HttpStatusCode httpStatusCode, dynamic result)
        {
            Result = result;
            HttpStatusCode = httpStatusCode;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode)
            {
                Content = new ObjectContent<dynamic>(Result, new JsonMediaTypeFormatter())
            };

            return Task.FromResult(response);
        }
    }
}