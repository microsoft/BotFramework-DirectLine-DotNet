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
    /// Parameters for creating a token
    /// </summary>
    public partial class TokenParameters
    {
        /// <summary>
        /// Initializes a new instance of the TokenParameters class.
        /// </summary>
        public TokenParameters() { }

        /// <summary>
        /// Initializes a new instance of the TokenParameters class.
        /// </summary>
        public TokenParameters(ChannelAccount user = default(ChannelAccount), string eTag = default(string))
        {
            User = user;
            ETag = eTag;
        }

        /// <summary>
        /// User account to embed within the token
        /// </summary>
        [JsonProperty(PropertyName = "user")]
        public ChannelAccount User { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "eTag")]
        public string ETag { get; set; }

    }
}
