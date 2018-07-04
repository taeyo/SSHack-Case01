using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace sshack
{
    public static class Gateway
    {
        [FunctionName("Gateway")]
        public static async Task<HttpResponseMessage> Run([
            HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            [Queue("request-queue")] IAsyncCollector<BaseInfo> requestQueue,
            ILogger log)
        {
            RequestInfo requestInfo = await req.Content.ReadAsAsync<RequestInfo>();

            List<BaseInfo> commands = GetDateTimePairs(requestInfo);

            try { 
                foreach (var item in commands){
                    await requestQueue.AddAsync(item);
                }
            }catch(Exception ex)
            {
                //log.Error($"[ERROR] : {ex.InnerException.Message}");
                return req.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
            return req.CreateResponse(HttpStatusCode.OK, "Success");
        }

        public static List<BaseInfo> GetDateTimePairs(RequestInfo info)
        {
            TimeSpan bigInterval = info.Command.ToTime - info.Command.FromTime;
            TimeSpan smallInterval = new TimeSpan(bigInterval.Ticks / info.Div);
            var returnValue = new List<BaseInfo>();
            DateTime temp = info.Command.FromTime;

            while (temp < info.Command.ToTime)
            {
                DateTime currentFromDate = temp;
                DateTime currentToDate = temp + smallInterval;

                // Your logic

                temp = temp + smallInterval;
 
                returnValue.Add(new BaseInfo() {
                    RequestID = info.RequestID,
                    Command = new QueryCommand()
                    {
                        FromTime = currentFromDate,
                        ToTime = currentToDate
                    }
                });
            }

            return returnValue;
        }
    }
}
