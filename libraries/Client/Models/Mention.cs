﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Bot.Connector.DirectLine
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    /// <summary>
    /// Mention information (entity type: "mention")
    /// </summary>
    public partial class Mention
    {
        /// <summary>
        /// Initializes a new instance of the Mention class.
        /// </summary>
        public Mention() { }

        /// <summary>
        /// Initializes a new instance of the Mention class.
        /// </summary>
        public Mention(ChannelAccount mentioned = default(ChannelAccount), string text = default(string), string type = default(string))
        {
            Mentioned = mentioned;
            Text = text;
            Type = type;
        }

        /// <summary>
        /// The mentioned user
        /// </summary>
        [JsonProperty(PropertyName = "mentioned")]
        public ChannelAccount Mentioned { get; set; }

        /// <summary>
        /// Sub Text which represents the mention (can be null or empty)
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Entity Type (typically from schema.org types)
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

    }
}
