using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace sshack
{
    public static class CheckProgress
    {
        [FunctionName("CheckProgress")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "requestid/{requestid}")]HttpRequestMessage req,
            [Blob("{requestid}", FileAccess.Read)] CloudBlobContainer blobContainer,
            string requestid,
            ILogger log)
        {
            string div = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "div", true) == 0)
                .Value;

            if (string.IsNullOrEmpty(div))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a 'div' on the query string");
            }

            BlobContinuationToken continuationToken = new BlobContinuationToken();
            List<IListBlobItem> results = new List<IListBlobItem>();
            do
            {
                var response = await blobContainer.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                results.AddRange(response.Results);
            }
            while (continuationToken != null);

            var list = new List<string>();
            foreach(CloudBlob item in results)
            {
                list.Add($"{requestid}/{item.Name}");
            }
            return results.Count == int.Parse(div)
                ? req.CreateResponse(HttpStatusCode.OK, list)
                : req.CreateResponse(HttpStatusCode.Accepted, list);
        }
    }
}
