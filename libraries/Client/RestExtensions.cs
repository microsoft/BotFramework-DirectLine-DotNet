// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Linq;
using System.Net.Http;
using Microsoft.Rest;

namespace Microsoft.Bot.Connector.DirectLine
{
    public static class RestExtensions
    {
        public static HttpResponseMessageWrapper ForException(this HttpResponseMessage response, string content = "")
        {
            return new HttpResponseMessageWrapper(response, content);
        }

        public static HttpRequestMessageWrapper ForException(this HttpRequestMessage request, string content = "")
        {
            return new HttpRequestMessageWrapper(request, content);
        }
    }
}
