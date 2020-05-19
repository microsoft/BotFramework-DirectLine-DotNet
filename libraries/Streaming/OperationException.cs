using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Bot.Connector.DirectLine
{
    /// <summary>
    /// Thrown when a streaming operation had an error
    /// </summary>
    public class OperationException : Exception
    {
        /// <summary>
        /// The status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// The body of the error
        /// </summary>
        public object Body { get; set; }

        /// <summary>
        /// Creates an OperationException
        /// </summary>
        public OperationException(string message, int statusCode, object body) :
            base(message)
        {
            StatusCode = statusCode;
            Body = body;
        }
    }
}
