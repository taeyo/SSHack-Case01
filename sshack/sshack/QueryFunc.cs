using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace sshack
{
    public static class QueryFunc
    {
        [FunctionName("QueryFunc")]
        public static async Task Run(
            [QueueTrigger("request-queue")]BaseInfo info,
            Binder binder,
            ILogger log)
        {
            string fileName = $"{info.Command.FromTime.ToString("yyyyMMddHHmmss")}.json";
            string outputLocation = $"{info.RequestID}/{fileName}";

            var records = populateData(info.RequestID, 200000);

            log.LogInformation($"[INFO] {fileName} Started : {DateTime.Now.ToString()}");
            #region JsonSerialize and Create Blob
            //JsonSerializer js = new JsonSerializer();
            //using (TextWriter blob = await binder.BindAsync<TextWriter>(
            //    new BlobAttribute(outputLocation, FileAccess.Write)))
            //{
            //    js.Serialize(blob, records);
            //}
            #endregion

            using (Stream stream = SerializeToStream(records))
            {
                stream.Position = 0;
                using (Stream destination = await binder.BindAsync<CloudBlobStream>(
                    new BlobAttribute(outputLocation, FileAccess.Write)))
                {
                    await stream.CopyToAsync(destination);
                }
            }

            //using (MemoryStream stream = SerializeToStream(records))
            //{
            //    stream.Position = 0;
            //    await CreateBlob(stream, info.RequestID, fileName);
            //}

            log.LogInformation($"[INFO] {fileName} Ended : {DateTime.Now.ToString()}");
        }

        public static MemoryStream SerializeToStream(object o)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
        }

        static async Task CreateBlob(MemoryStream stream, string ContainerName, string fileName)
        {
            BlobRequestOptions options = new BlobRequestOptions
            {
                ParallelOperationThreadCount = 8,
                DisableContentMD5Validation = true,
                StoreBlobContentMD5 = false
            };
            string constr = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(constr);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            cloudBlobClient.DefaultRequestOptions = options;
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(ContainerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();

            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            blockBlob.StreamWriteSizeInBytes = 4 * 1024 * 1024;

            await blockBlob.UploadFromStreamAsync(stream, null, options, null);
        }

        private static List<EventData> populateData(string rid, int count)
        {
            List<EventData> datalist = new List<EventData>();

            int datacount = count;
            EventData ed = new EventData();
            for (int i = 0; i < datacount; i++)
            {
                ed = new EventData()
                {
                    LINE = rid,
                    AREA = "ETHC",
                    EQP_ID = "ELPHZ28",
                    EQP_INDEX = "42756",
                    PARAM_VALUE = "0.01",
                    UPPER_SPEC = null,
                    LOWER_SPEC = null,
                    ERD_PARAM_INDEX = "3071803",
                    COL_NAME = "_RF_FORWARD_POWER_STD_T",
                    CH_PARAM_NAME = "_RF_FORWARD_POWER_STD_T",
                    PARAM_INDEX = "3071803",
                    PARAM_NAME = "_RF_FORWARD_POWER_STD_T",
                    ACT_TIME = "2018-07-02 23:28:07",
                    SLOT_NO = "0",
                    CH_STEP = "1",
                    LOT_ID = "CHB038.1",
                    PROC_PLAN_ID = "KCHE",
                    PART_ID = "K9GBGD8U0D-CAT",
                    PPID = "2F9KCHE.HE170P",
                    RECIPEID = "HY170P",
                    STEPSEQ = "H3120020",
                    WAFER_NO = "0",
                    CH_ID = "0",
                    CH_NAME = null,
                    PARRERN = "V",
                    DATA_INDEX = "DATA_INDEX"
                };
                datalist.Add(ed);
            }

            return datalist;
        }
    }
}
