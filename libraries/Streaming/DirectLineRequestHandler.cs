using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Streaming;
using Microsoft.Bot.Streaming.Payloads;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Microsoft.Bot.Connector.DirectLine
{
    internal class DirectLineRequestHandler : RequestHandler
    {
        private readonly Action<ActivitySet> _receiveActivities;

        private readonly string _postActivitiesPath;

        public DirectLineRequestHandler(string conversationId, Action<ActivitySet> receiveActivities)
        {
            _receiveActivities = receiveActivities;
            _postActivitiesPath = $"/conversations/{conversationId}/activities";
        }

        public override async Task<StreamingResponse> ProcessRequestAsync(ReceiveRequest request, ILogger<RequestHandler> logger, object context = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (request.Verb == "POST" && request.Path == _postActivitiesPath)
            {
                var activitySet = await ReadOptionalBodyAsJson<ActivitySet>(request).ConfigureAwait(false);

                if (request.Streams.Count > 1)
                {
                    var attachmentDictionary = request.Streams.Skip(1).ToDictionary(a => a.Id);
                    int streamsMappedtoActivitiesCount = 0;
                    foreach (var activity in activitySet.Activities)
                    {
                        if (activity.Attachments == null || activity.Attachments.Count == 0)
                        {
                            continue;
                        }

                        for (int i = 0; i < activity.Attachments.Count(); i++)
                        {
                            if (string.Equals(activity.Attachments[i].ContentType, "bf-stream", StringComparison.InvariantCultureIgnoreCase))
                            {
                                var id = Guid.Parse(activity.Attachments[i].Content.ToString());
                                var stream = attachmentDictionary[id];
                                activity.Attachments[i] = new Attachment() { ContentType = stream.ContentType, Content = stream.Stream };
                                streamsMappedtoActivitiesCount++;
                            }
                        }

                        if (streamsMappedtoActivitiesCount == request.Streams.Count - 1)
                        {
                            break;
                        }
                    }
                }

                _receiveActivities(activitySet);
                return StreamingResponse.OK();
            }
            return StreamingResponse.NotFound();
        }

        private static async Task<T> ReadOptionalBodyAsJson<T>(ReceiveRequest request)
        {
            // The first stream attached to a ReceiveRequest is always the ReceiveRequest body.
            // Any additional streams must be defined within the body or they will not be
            // attached properly when processing activities.
            var contentStream = request.Streams.FirstOrDefault();
            if (contentStream != null)
            {
                var contentString = await ReadBuffer(contentStream).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(contentString);
            }
            return default(T);
        }

        private static async Task<string> ReadBuffer(IContentStream contentStream)
        {
            var length = contentStream.Length ?? 100;
            StringBuilder outputBuilder = new StringBuilder(length);
            char[] c = new char[length];
            int readCount = 0;
            using (var reader = new StreamReader(contentStream.Stream, Encoding.UTF8))
            {
                do
                {
                    readCount = await reader.ReadAsync(c, 0, c.Length);
                    outputBuilder.Append(c, 0, readCount);
                } while (readCount > 0);
            }
            return outputBuilder.ToString();
        }
    }
}
