// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Net.Http;

namespace Microsoft.Bot.Connector.DirectLine
{
    public partial class DirectLineClient
    {
        partial void CustomInitialize()
        {
            this.StreamingConversations = new StreamingConversations(this);
        }

        /// <summary>
        /// Gets the IConversations.
        /// </summary>
        public virtual IStreamingConversations StreamingConversations { get; private set; }
    }
}
