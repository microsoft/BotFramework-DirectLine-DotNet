// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Bot.Connector.DirectLine
{
    /// <summary>
    /// A bot was added or removed from a channel
    /// </summary>
    public interface IInstallationUpdateActivity : IActivity
    {
        /// <summary>
        /// Add|remove
        /// </summary>
        string Action { get; set; }
    }
}
