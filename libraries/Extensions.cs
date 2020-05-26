// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Streaming;
using Microsoft.Bot.Streaming.Payloads;
using Newtonsoft.Json;

namespace Microsoft.Bot.Connector.DirectLine
{
    internal static class Extensions
    {
        public static async Task<T> ReadBodyAsJsonAsync<T>(this ReceiveResponse response)
        {
            // The first stream attached to a ReceiveRequest is always the ReceiveRequest body.
            // Any additional streams must be defined within the body or they will not be
            // attached properly when processing activities.
            try
            {
                T returnValue = default(T);
                string streamContent = await response.ReadBodyAsStringAsync().ConfigureAwait(false);
                if (streamContent != null)
                {
                    returnValue = JsonConvert.DeserializeObject<T>(streamContent);
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<String> ReadBodyAsStringAsync(this ReceiveResponse response)
        {
            // The first stream attached to a ReceiveRequest is always the ReceiveRequest body.
            // Any additional streams must be defined within the body or they will not be
            // attached properly when processing activities.
            try
            {
                var contentStream = response.Streams.FirstOrDefault();
                if (contentStream != null)
                {
                    return await ReadBuffer(contentStream).ConfigureAwait(false);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static async Task<string> ReadBuffer(IContentStream contentStream)
        {
            var length = contentStream.Length ?? 100;
            StringBuilder outputBuilder = new StringBuilder(length);
            char[] c = new char[length];
            int readCount = 0;
            using (var reader = new StreamReader(contentStream.Stream, Encoding.UTF8))
            {
                do
                {
                    readCount = await reader.ReadAsync(c, 0, c.Length);
                    outputBuilder.Append(c, 0, readCount);
                } while (readCount > 0);
            }
            return outputBuilder.ToString();
        }
    }
}
