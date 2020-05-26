// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Linq;

namespace Microsoft.Bot.Connector.DirectLine
{
    /// <summary>
    /// Types of actions
    /// </summary>
    public class ActionTypes
    {
        /// <summary>
        /// Open the supplied URL in the built-in browser
        /// </summary>
        public const string OpenUrl = "openUrl";

        /// <summary>
        /// Post message to bot. ImBack sends the action's Title.
        /// </summary>
        public const string ImBack = "imBack";

        /// <summary>
        /// Post message to bot. PostBack displays the action's Title but sends the Title and Value.
        /// </summary>
        public const string PostBack = "postBack";

        /// <summary>
        /// Open an audio playback container for the supplied URL
        /// </summary>
        public const string PlayAudio = "playAudio";

        /// <summary>
        /// Open a video playback container for the supplied URL
        /// </summary>
        public const string PlayVideo = "playVideo";

        /// <summary>
        /// Show image referenced by URL
        /// </summary>
        public const string ShowImage = "showImage";

        /// <summary>
        /// Download file referenced by url
        /// </summary>
        public const string DownloadFile = "downloadFile";

        /// <summary>
        /// Prompt the user to sign in
        /// </summary>
        public const string Signin = "signin";

        /// <summary>
        /// Initiate a call
        /// </summary>
        public const string Call = "call";
    }
}
