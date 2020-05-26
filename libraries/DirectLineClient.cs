// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Net.Http;

namespace Microsoft.Bot.Connector.DirectLine
{
    public partial class DirectLineClient
    {
        /// <summary>
        /// Create a new instance of the DirectLineClient class
        /// </summary>
        /// <param name="secretOrToken">Optional. The secret or token for the Direct Line site. If null, this setting is read from settings["DirectLineSecret"]</param>
        /// <param name="handlers">Optional. The delegating handlers to add to the http client pipeline.</param>
        public DirectLineClient(string secretOrToken = null, params DelegatingHandler[] handlers)
            : this(handlers)
        {
            this.Credentials = new DirectLineClientCredentials(secretOrToken);
        }

        /// <summary>
        /// Create a new instance of the DirectLineClient class
        /// </summary>
        /// <param name="baseUri">Base URI for the Direct Line service</param>
        /// <param name="credentials">Credentials for the Direct Line service</param>
        /// /// <param name="handlers">Optional. The delegating handlers to add to the http client pipeline.</param>
        public DirectLineClient(Uri baseUri, DirectLineClientCredentials credentials, params DelegatingHandler[] handlers)
            : this(baseUri, handlers)
        {
            this.Credentials = credentials;
        }
    }
}
