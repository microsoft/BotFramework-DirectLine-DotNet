..\..\rep -find:"localhost:8011" -replace:"directline.botframework.com" swagger.json

rd /s /q Client
..\..\packages\autorest.0.16.0\tools\AutoRest -namespace Microsoft.Bot.Connector.DirectLine -input swagger.json -outputDirectory Client -AddCredentials -ClientName DirectLineClient

@rem == Fix up FromProperty and namespaces ==
..\..\rep -find:"FromProperty" -replace:"From" -r *.cs
..\..\rep -find:"namespace Microsoft.Bot.Connector.DirectLine.Models" -replace:"namespace Microsoft.Bot.Connector.DirectLine" -r *.cs
..\..\rep -find:"using Models;" -replace:"" -r *.cs
..\..\rep -find:"public DateTime? LocalTimestamp { get; set; }" -replace:"public DateTimeOffset? LocalTimestamp { get; set; }" -r *.cs

@rem == Add "contentType" parameter to Upload method ==
..\..\rep^
    -find:"Task<HttpOperationResponse<ResourceResponse>> UploadWithHttpMessagesAsync(string conversationId, System.IO.Stream file, string userId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));"^
    -replace:"Task<HttpOperationResponse<ResourceResponse>> UploadWithHttpMessagesAsync(string conversationId, System.IO.Stream file, string userId = default(string), string contentType = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));"^
	-r IConversations.cs

..\..\rep^
    -find:"public async Task<HttpOperationResponse<ResourceResponse>> UploadWithHttpMessagesAsync(string conversationId, System.IO.Stream file, string userId = default(string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))"^
    -replace:"public async Task<HttpOperationResponse<ResourceResponse>> UploadWithHttpMessagesAsync(string conversationId, System.IO.Stream file, string userId = default(string), string contentType = null, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))"^
    -r Conversations.cs

..\..\rep^
    -find:"_file.Headers.ContentType = new MediaTypeHeaderValue(\"application/octet-stream\");"^
    -replace:"_file.Headers.ContentType = new MediaTypeHeaderValue(contentType ?? \"application/octet-stream\");"^
    -r Conversations.cs

..\..\rep^
    -find:"public static ResourceResponse Upload(this IConversations operations, string conversationId, System.IO.Stream file, string userId = default(string))"^
    -replace:"public static ResourceResponse Upload(this IConversations operations, string conversationId, System.IO.Stream file, string userId = default(string), string contentType = null)"^
    -r ConversationsExtensions.cs

..\..\rep^
    -find:"return Task.Factory.StartNew(s => ((IConversations)s).UploadAsync(conversationId, file, userId), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();"^
    -replace:"return Task.Factory.StartNew(s => ((IConversations)s).UploadAsync(conversationId, file, userId, contentType), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();"^
    -r ConversationsExtensions.cs

..\..\rep^
    -find:"public static async Task<ResourceResponse> UploadAsync(this IConversations operations, string conversationId, System.IO.Stream file, string userId = default(string), CancellationToken cancellationToken = default(CancellationToken))"^
    -replace:"public static async Task<ResourceResponse> UploadAsync(this IConversations operations, string conversationId, System.IO.Stream file, string userId = default(string), string contentType = null, CancellationToken cancellationToken = default(CancellationToken))"^
    -r ConversationsExtensions.cs

..\..\rep^
    -find:"using (var _result = await operations.UploadWithHttpMessagesAsync(conversationId, file, userId, null, cancellationToken).ConfigureAwait(false))"^
    -replace:"using (var _result = await operations.UploadWithHttpMessagesAsync(conversationId, file, userId, contentType, null, cancellationToken).ConfigureAwait(false))"^
    -r ConversationsExtensions.cs

git checkout -- Client\RestExtensions.cs
