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
    /// Someone has updated their contact list
    /// </summary>
    public interface IContactRelationUpdateActivity : IActivity
    {

        /// <summary>
        /// Add|remove
        /// </summary>
        string Action { get; set; }
    }
}
