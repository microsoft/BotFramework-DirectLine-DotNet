// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.6.2

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace DirectLineBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string replyText = null;
            Attachment attachment = null;

            switch (turnContext.Activity.Text.ToLower())
            {
                case "show me a hero card":
                    replyText = $"Sample message with a HeroCard attachment";

                    attachment = new HeroCard
                    {
                        Title = "Sample Hero Card",
                        Text = "Displayed in the DirectLine client"
                    }.ToAttachment();

                    break;
                case "send me a botframework image":
                    replyText = $"Sample message with an Image attachment";

                    attachment = new Attachment()
                    {
                        ContentType = "image/png",
                        ContentUrl = "https://docs.microsoft.com/en-us/bot-framework/media/how-it-works/architecture-resize.png",
                    };

                    break;
                default:
                    replyText = $"You said '{turnContext.Activity.Text}'";
                    break;
            }

            var reply = MessageFactory.Text(replyText, replyText);
            if(attachment != null)
            {
                reply.Attachments.Add(attachment);
            }
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
