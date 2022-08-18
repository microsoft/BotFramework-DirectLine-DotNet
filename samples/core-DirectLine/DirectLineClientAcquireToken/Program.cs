namespace DirectLineSampleClient
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector.DirectLine;
    using Models;
    using Newtonsoft.Json;

    public class Program
    {
        private static string directLineSecret = null;
        private static string token = null; // First initialized when a conversation is started.
        private static string siteId = ""; // Add Site ID from Direct Line config channel here.
        private static string botId = ""; // Add bot ID here
        private static string fromUser = "DirectLineSampleClientUser";

        public static void Main(string[] args)
        {
            StartBotConversation().Wait();
        }

        private static async Task StartBotConversation()
        {
            token = await TokenHelper.GetTokenAsync();

            // if you are using a region-specific endpoint, change the uri and uncomment the code
            var directLineUri = "https://directline.scratch.botframework.com/"; // endpoint in Azure Public Cloud
            // If using token then first parameter of DirectLineClientCredentials must always be null
            DirectLineClient client = new DirectLineClient(new Uri(directLineUri), new DirectLineClientCredentials(null, token, null));

            var conversation = await client.Conversations.StartConversationAsync(siteId);

            new System.Threading.Thread(async () => await ReadBotMessagesAsync(client, conversation.ConversationId)).Start();

            // This is just to help validate that token refresh works by refreshing the conversion with a new AAD token.
            // The next activity post will be sent with a new token.
            // In an ideal project this should be in a token refresh loop that keeps on updating the AAD token token before it expires
            var conv = await client.Tokens.RefreshTokenAsync(conversation.ConversationId, () => TokenHelper.GetTokenAsync());

            if (!string.Equals(conversation?.ConversationId, conv?.ConversationId))
            {
                throw new Exception("Token not successfully refreshed. New conversation created.");
            }

            if (string.Equals(conversation?.Token, conv?.Token))
            {
                throw new Exception("Token not successfully refreshed. No new refresh token.");
            }

            // Update the token in case it is to be used in here and hereafter
            if (!(string.IsNullOrEmpty(conv?.Token) && string.IsNullOrEmpty(conv?.ConversationId)))
            {
                token = conv.Token;
            }

            Console.Write("Command> ");

            while (true)
            {
                string input = Console.ReadLine().Trim();

                if (input.ToLower() == "exit")
                {
                    break;
                }
                else
                {
                    if (input.Length > 0)
                    {
                        Activity userMessage = new Activity
                        {
                            From = new ChannelAccount(fromUser),
                            Text = input,
                            Type = ActivityTypes.Message
                        };

                        await client.Conversations.PostActivityAsync(conversation.ConversationId, userMessage);
                    }
                }
            }
        }

        private static async Task ReadBotMessagesAsync(DirectLineClient client, string conversationId)
        {
            string watermark = null;

            while (true)
            {
                var activitySet = await client.Conversations.GetActivitiesAsync(conversationId, watermark);
                watermark = activitySet?.Watermark;

                var activities = from x in activitySet.Activities
                                 where x.From.Id == botId
                                 select x;

                foreach (Activity activity in activities)
                {
                    Console.WriteLine(activity.Text);

                    if (activity.Attachments != null)
                    {
                        foreach (Attachment attachment in activity.Attachments)
                        {
                            switch (attachment.ContentType)
                            {
                                case "application/vnd.microsoft.card.hero":
                                    RenderHeroCard(attachment);
                                    break;

                                case "image/png":
                                    Console.WriteLine($"Opening the requested image '{attachment.ContentUrl}'");

                                    Process.Start(attachment.ContentUrl);
                                    break;
                            }
                        }
                    }  else
                    {
                        Console.WriteLine(activity?.AsMessageActivity());
                    }

                    Console.Write("Command> ");
                }

                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            }
        }

        private static void RenderHeroCard(Attachment attachment)
        {
            const int Width = 70;
            Func<string, string> contentLine = (content) => string.Format($"{{0, -{Width}}}", string.Format("{0," + ((Width + content.Length) / 2).ToString() + "}", content));

            var heroCard = JsonConvert.DeserializeObject<HeroCard>(attachment.Content.ToString());

            if (heroCard != null)
            {
                Console.WriteLine("/{0}", new string('*', Width + 1));
                Console.WriteLine("*{0}*", contentLine(heroCard.Title));
                Console.WriteLine("*{0}*", new string(' ', Width));
                Console.WriteLine("*{0}*", contentLine(heroCard.Text));
                Console.WriteLine("{0}/", new string('*', Width + 1));
            }
        }
    }
}
