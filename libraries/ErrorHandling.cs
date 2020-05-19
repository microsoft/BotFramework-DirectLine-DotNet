using System;
using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Rest;

namespace Microsoft.Bot.Connector.DirectLine
{
    public static class ErrorHandling
    {
        public static ObjectT HandleError<ObjectT>(this HttpOperationResponse<ObjectT> result)
        {
            if (!result.Response.IsSuccessStatusCode)
            {
                ErrorResponse errorResponse = result.Body as ErrorResponse;
                throw new HttpOperationException(String.IsNullOrEmpty(errorResponse?.Error?.Message) ? result.Response.ReasonPhrase : errorResponse.Error.Message)
                {
                    Request = result.Request.ForException(),
                    Response = result.Response.ForException(),
                    Body = result.Body
                };
            }
            return (ObjectT)result.Body;
        }
    }
}
