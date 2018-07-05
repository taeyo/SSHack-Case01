using System;
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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace sshack
{
    public static class DeleteContainer
    {
        [FunctionName("DeleteContainer")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "requestid/{requestid}")]HttpRequestMessage req,
            [Blob("{requestid}", FileAccess.Read)] CloudBlobContainer blobContainer,
            string requestid,
            ILogger log)
        {
            await blobContainer.DeleteAsync();
            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
