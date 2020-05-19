using System;
using System.Linq;

namespace Microsoft.Bot.Connector.DirectLine
{
    /// <summary>
    /// Action types valid for InstallationUpdate activities
    /// </summary>
    public static class InstallationUpdateActionTypes
    {
        /// <summary>
        /// Bot was added
        /// </summary>
        public const string Add = "add";

        /// <summary>
        /// Bot was removed
        /// </summary>
        public const string Remove = "remove";
    }
}
