using System.Threading.Tasks;
using AdvertApi.Models.Messages;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Nest;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace WebAdvert.SearchWorkerLambda
{
    public class SearchWorker
    {
        public SearchWorker() : this(ElasticSearchHelper.GetInstance(ConfigurationHelper.Instance))
        {

        }

        private readonly IElasticClient _client;

        public SearchWorker(IElasticClient client)
        {
            _client = client;
        }


        public async Task FunctionHandler(SNSEvent evnt, ILambdaContext context)
        {
            foreach(var record in evnt.Records)
            {
                await ProcessRecordAsync(record, context);
            }
        }

        private async Task ProcessRecordAsync(SNSEvent.SNSRecord record, ILambdaContext context)
        {
            context.Logger.LogLine($"Processed record {record.Sns.Message}");

            var message = JsonConvert.DeserializeObject<AdvertConfirmedMessage>(record.Sns.Message);
            var advertDocument = MappingHelper.Map(message);
            await _client.IndexDocumentAsync(advertDocument);
        }
    }
}
