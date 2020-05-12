namespace Microsoft.Bot.Connector.DirectLine
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Net.Http.Headers;
    using Microsoft.Bot.Streaming.Transport.WebSockets;
    using Microsoft.Bot.Streaming.Transport;
    using Microsoft.Bot.Streaming;


    /// <summary>
    /// Conversations operations.
    /// </summary>
    public partial class StreamingConversations : IStreamingConversations
    {
        /// <summary>
        /// Initializes a new instance of the StreamingConversations class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the service client.
        /// </param>
        public StreamingConversations(DirectLineClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            this.Client = client;
        }

        /// <summary>
        /// Gets a reference to the DirectLineClient
        /// </summary>
        public DirectLineClient Client { get; private set; }

        // Whether the client thinks it is currently connected.
        public bool IsConnected
        {
            get
            {
                return SocketClient != null ? SocketClient.IsConnected : false;
            }
        }

        public event DisconnectedEventHandler Disconnected;

        private WebSocketClient SocketClient { get; set; }

        private string GetBaseWebSocketUrl()
        {
            if (Client.BaseUri.Host == "localhost")
            {
                return $"ws://{Client.BaseUri.Host}:{Client.BaseUri.Port}{Client.BaseUri.AbsolutePath}";
            }
            return $"wss://{Client.BaseUri.Host}{Client.BaseUri.AbsolutePath}";
        }

        public async Task ConnectAsync(string conversationId, Action<ActivitySet> receiveActivities, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (SocketClient != null)
            {
                throw new InvalidOperationException("Connection is already open");
            }

            var token = (Client.Credentials as DirectLineClientCredentials)?.Authorization;
            var url = $"{GetBaseWebSocketUrl()}v3/directline/conversations/connect?token={token}&conversationId={conversationId}";

            SocketClient = new WebSocketClient(url, new DirectLineRequestHandler(conversationId, receiveActivities));

            SocketClient.Disconnected += SocketClient_Disconnected;
            await SocketClient.ConnectAsync().ConfigureAwait(false);
        }

        private void SocketClient_Disconnected(object sender, DisconnectedEventArgs e)
        {
            Disconnected?.Invoke(this, e);
        }

        public async Task<Conversation> StartConversationAsync(TokenParameters tokenParameters = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (SocketClient == null)
            {
                throw new InvalidOperationException("Connection is not opened.");
            }

            var request = new StreamingRequest()
            {
                Verb = "POST",
                Path = "/v3/directline/conversations"
            };

            if (tokenParameters != null)
            {
                request.SetBody(tokenParameters);
            }

            var response = await SocketClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode != 200 && response.StatusCode != 201)
            {
                var body = response.ReadBodyAsStringAsync().ConfigureAwait(false);
                var ex = new OperationException(
                    $"Operation returned an invalid status code '{response.StatusCode}'",
                    response.StatusCode,
                    body);
                throw ex;
            }

            var conversation = await response.ReadBodyAsJsonAsync<Conversation>().ConfigureAwait(false);

            return conversation;
        }

        public async Task<Conversation> ReconnectToConversationAsync(string conversationId, string watermark = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (SocketClient == null)
            {
                throw new InvalidOperationException("Connection is not opened.");
            }

            var response = await SocketClient.SendAsync(new StreamingRequest()
            {
                Verb = "GET",
                Path = $"/v3/directline/conversations/{conversationId}"
            }).ConfigureAwait(false);

            if (response.StatusCode != 200)
            {
                var body = response.ReadBodyAsStringAsync().ConfigureAwait(false);
                var ex = new OperationException(
                    $"Operation returned an invalid status code '{response.StatusCode}'",
                    response.StatusCode,
                    body);
                throw ex;
            }

            var conversation = await response.ReadBodyAsJsonAsync<Conversation>().ConfigureAwait(false);

            return conversation;
        }

        public async Task<ResourceResponse> PostActivityAsync(string conversationId, Activity activity, CancellationToken cancellationToken = default(CancellationToken), params AttachmentStream[] attachmentStreams)
        {
            if (SocketClient == null)
            {
                throw new InvalidOperationException("Connection is not opened.");
            }

            var request = new StreamingRequest()
            {
                Verb = "POST",
                Path = $"/v3/directline/conversations/{conversationId}/activities"
            };

            request.SetBody(activity);

            if (attachmentStreams != null && attachmentStreams.Length > 0)
            {
                foreach (var stream in attachmentStreams)
                {
                    var contentStream = new StreamContent(stream.ContentStream);
                    contentStream.Headers.TryAddWithoutValidation(HeaderNames.ContentType, stream.ContentType);
                    request.AddStream(contentStream);
                }
            }

            var response = await SocketClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode != 200 && response.StatusCode != 204)
            {
                var body = await response.ReadBodyAsStringAsync().ConfigureAwait(false);
                var ex = new OperationException(
                    $"Operation returned an invalid status code '{response.StatusCode}'",
                    response.StatusCode,
                    body);
                throw ex;
            }

            var resourceResponse = await response.ReadBodyAsJsonAsync<ResourceResponse>().ConfigureAwait(false);

            return resourceResponse;
        }

        public async Task<ResourceResponse> UpdateActivityAsync(string conversationId, string activityId, Activity activity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (SocketClient == null)
            {
                throw new InvalidOperationException("Connection is not opened.");
            }

            var request = new StreamingRequest()
            {
                Verb = "PUT",
                Path = $"/v3/directline/conversations/{conversationId}/activities/{activityId}"
            };

            request.SetBody(activity);

            var response = await SocketClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode != 200)
            {
                var body = await response.ReadBodyAsStringAsync().ConfigureAwait(false);
                var ex = new OperationException(
                    $"Operation returned an invalid status code '{response.StatusCode}'",
                    response.StatusCode,
                    body);
                throw ex;
            }

            var resourceResponse = await response.ReadBodyAsJsonAsync<ResourceResponse>().ConfigureAwait(false);

            return resourceResponse;
        }

        public void Disconnect()
        {
            if (SocketClient != null)
            {
                SocketClient.Disconnect();
                SocketClient = null;
            }
        }

        public async Task<ResourceResponse> UploadAttachmentsAsync(string conversationId, string userId, CancellationToken cancellationToken = default(CancellationToken), params AttachmentStream[] attachmentStreams)
        {
            if (SocketClient == null)
            {
                throw new InvalidOperationException("Connection is not opened.");
            }

            if (attachmentStreams == null || attachmentStreams.Length == 0)
            {
                throw new InvalidOperationException("Cannot send attachment streams, because no attachments were supplied.");
            }

            var request = new StreamingRequest()
            {
                Verb = "PUT",
                Path = $"/v3/directline/conversations/{conversationId}/users/{userId}/upload"
            };

            foreach (var stream in attachmentStreams)
            {
                var contentStream = new StreamContent(stream.ContentStream);
                contentStream.Headers.TryAddWithoutValidation(HeaderNames.ContentType, stream.ContentType);
                request.AddStream(contentStream);
            }

            var response = await SocketClient.SendAsync(request).ConfigureAwait(false);

            if (response.StatusCode != 200)
            {
                var body = await response.ReadBodyAsStringAsync().ConfigureAwait(false);
                var ex = new OperationException(
                    $"Operation returned an invalid status code '{response.StatusCode}'",
                    response.StatusCode,
                    body);
                throw ex;
            }

            var resourceResponse = await response.ReadBodyAsJsonAsync<ResourceResponse>().ConfigureAwait(false);

            return resourceResponse;
        }
    }
}
