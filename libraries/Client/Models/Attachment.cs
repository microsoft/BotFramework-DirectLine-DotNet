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
    /// An attachment within an activity
    /// </summary>
    public partial class Attachment
    {
        /// <summary>
        /// Initializes a new instance of the Attachment class.
        /// </summary>
        public Attachment() { }

        /// <summary>
        /// Initializes a new instance of the Attachment class.
        /// </summary>
        public Attachment(string contentType = default(string), string contentUrl = default(string), object content = default(object), string name = default(string), string thumbnailUrl = default(string))
        {
            ContentType = contentType;
            ContentUrl = contentUrl;
            Content = content;
            Name = name;
            ThumbnailUrl = thumbnailUrl;
        }

        /// <summary>
        /// mimetype/Contenttype for the file
        /// </summary>
        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// Content Url
        /// </summary>
        [JsonProperty(PropertyName = "contentUrl")]
        public string ContentUrl { get; set; }

        /// <summary>
        /// Embedded content
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public object Content { get; set; }

        /// <summary>
        /// (OPTIONAL) The name of the attachment
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// (OPTIONAL) Thumbnail associated with attachment
        /// </summary>
        [JsonProperty(PropertyName = "thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

    }
}
