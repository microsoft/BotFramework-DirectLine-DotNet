// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Bot.Connector.DirectLine
{
    public partial class Activity :
        IActivity,
        IConversationUpdateActivity,
        IContactRelationUpdateActivity,
        IMessageActivity,
        ITypingActivity,
        IEndOfConversationActivity,
        IEventActivity,
        IInvokeActivity,
        IMessageUpdateActivity,
        IMessageDeleteActivity,
        IInstallationUpdateActivity
    {

        /// <summary>
        /// Extension data for overflow of properties
        /// </summary>
        [JsonExtensionData(ReadData = true, WriteData = true)]
        public JObject Properties { get; set; } = new JObject();

        /// <summary>
        /// Content-type for an Activity
        /// </summary>
        public const string ContentType = "application/vnd.microsoft.activity";

        /// <summary>
        /// Take a message and create a reply message for it with the routing information 
        /// set up to correctly route a reply to the source message
        /// </summary>
        /// <param name="text">text you want to reply with</param>
        /// <param name="locale">language of your reply</param>
        /// <returns>message set up to route back to the sender</returns>
        public Activity CreateReply(string text = null, string locale = null)
        {
            Activity reply = new Activity();
            reply.Type = ActivityTypes.Message;
            reply.Timestamp = DateTime.UtcNow;
            reply.From = new ChannelAccount(id: Recipient.Id, name: Recipient.Name);
            reply.Recipient = new ChannelAccount(id: From.Id, name: From.Name);
            reply.ReplyToId = Id;
            reply.ServiceUrl = ServiceUrl;
            reply.ChannelId = ChannelId;
            reply.Conversation = new ConversationAccount(isGroup: Conversation.IsGroup, id: Conversation.Id, name: Conversation.Name);
            reply.Text = text ?? string.Empty;
            reply.Locale = locale ?? Locale;
            reply.Attachments = new List<Attachment>();
            reply.Entities = new List<Entity>();
            return reply;
        }

        /// <summary>
        /// Create an instance of the Activity class with IMessageActivity masking
        /// </summary>
        public static IMessageActivity CreateMessageActivity() { return new Activity(ActivityTypes.Message); }

        /// <summary>
        /// Create an instance of the Activity class with IContactRelationUpdateActivity masking
        /// </summary>
        public static IContactRelationUpdateActivity CreateContactRelationUpdateActivity() { return new Activity(ActivityTypes.ContactRelationUpdate); }

        /// <summary>
        /// Create an instance of the Activity class with IInstallationUpdateActivity masking
        /// </summary>
        public static IInstallationUpdateActivity CreateInstallationUpdateActivity() { return new Activity(ActivityTypes.InstallationUpdate); }

        /// <summary>
        /// Create an instance of the Activity class with IConversationUpdateActivity masking
        /// </summary>
        public static IConversationUpdateActivity CreateConversationUpdateActivity() { return new Activity(ActivityTypes.ConversationUpdate); }

        /// <summary>
        /// Create an instance of the Activity class with ITypingActivity masking
        /// </summary>
        public static ITypingActivity CreateTypingActivity() { return new Activity(ActivityTypes.Typing); }

        /// <summary>
        /// Create an instance of the Activity class with IActivity masking
        /// </summary>
        public static IActivity CreatePingActivity() { return new Activity(ActivityTypes.Ping); }

        /// <summary>
        /// Create an instance of the Activity class with IEndOfConversationActivity masking
        /// </summary>
        public static IEndOfConversationActivity CreateEndOfConversationActivity() { return new Activity(ActivityTypes.EndOfConversation); }

        /// <summary>
        /// Create an instance of the Activity class with an IEventActivity masking
        /// </summary>
        public static IEventActivity CreateEventActivity() { return new Activity(ActivityTypes.Event); }

        /// <summary>
        /// Create an instance of the Activity class with IInvokeActivity masking
        /// </summary>
        public static IInvokeActivity CreateInvokeActivity() { return new Activity(ActivityTypes.Invoke); }

        /// <summary>
        /// True if the Activity is of the specified activity type
        /// </summary>
        protected bool IsActivity(string activity) { return string.Compare(Type?.Split('/').First(), activity, true) == 0; }

        /// <summary>
        /// Return an IMessageActivity mask if this is a message activity
        /// </summary>
        public IMessageActivity AsMessageActivity() { return IsActivity(ActivityTypes.Message) ? this : null; }

        /// <summary>
        /// Return an IContactRelationUpdateActivity mask if this is a contact relation update activity
        /// </summary>
        public IContactRelationUpdateActivity AsContactRelationUpdateActivity() { return IsActivity(ActivityTypes.ContactRelationUpdate) ? this : null; }

        /// <summary>
        /// Return an IInstallationUpdateActivity mask if this is a installation update activity
        /// </summary>
        public IInstallationUpdateActivity AsInstallationUpdateActivity() { return IsActivity(ActivityTypes.InstallationUpdate) ? this : null; }

        /// <summary>
        /// Return an IConversationUpdateActivity mask if this is a conversation update activity
        /// </summary>
        public IConversationUpdateActivity AsConversationUpdateActivity() { return IsActivity(ActivityTypes.ConversationUpdate) ? this : null; }

        /// <summary>
        /// Return an ITypingActivity mask if this is a typing activity
        /// </summary>
        public ITypingActivity AsTypingActivity() { return IsActivity(ActivityTypes.Typing) ? this : null; }

        /// <summary>
        /// Return an IEndOfConversationActivity mask if this is an end of conversation activity
        /// </summary>
        public IEndOfConversationActivity AsEndOfConversationActivity() { return IsActivity(ActivityTypes.EndOfConversation) ? this : null; }

        /// <summary>
        /// Return an IEventActivity mask if this is an event activity
        /// </summary>
        public IEventActivity AsEventActivity() { return IsActivity(ActivityTypes.Event) ? this : null; }

        /// <summary>
        /// Return an IInvokeActivity mask if this is an invoke activity
        /// </summary>
        public IInvokeActivity AsInvokeActivity() { return IsActivity(ActivityTypes.Invoke) ? this : null; }

        /// <summary>
        /// Returns IMessageUpdateActivity if this is a message update activity, null otherwise
        /// </summary>
        public IMessageUpdateActivity AsMessageUpdateActivity() { return IsActivity(ActivityTypes.MessageUpdate) ? this : null; }

        /// <summary>
        /// Returns IMessageDeleteActivity if this is a message delete activity, null otherwise
        /// </summary>
        public IMessageDeleteActivity AsMessageDeleteActivity() { return IsActivity(ActivityTypes.MessageDelete) ? this : null; }

        /// <summary>
        /// Check if the message has content
        /// </summary>
        /// <returns>Returns true if this message has any content to send</returns>
        public bool HasContent()
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(Summary))
            {
                return true;
            }

            if (Attachments != null && Attachments.Any())
            {
                return true;
            }

            if (ChannelData != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get mentions 
        /// </summary>
        /// <returns></returns>
        public Mention[] GetMentions()
        {
            return Entities?.Where(entity => string.Compare(entity.Type, "mention", ignoreCase: true) == 0).Select(e => e.Properties.ToObject<Mention>()).ToArray() ?? new Mention[0];
        }

        /// <summary>
        /// Is there a mention of Id in the Text Property 
        /// </summary>
        /// <param name="id">ChannelAccount.Id</param>
        /// <returns>true if this id is mentioned in the text</returns>
        public bool MentionsId(string id)
        {
            return GetMentions().Where(mention => mention.Mentioned.Id == id).Any();
        }

        /// <summary>
        /// Is there a mention of Recipient.Id in the Text Property 
        /// </summary>
        /// <returns>true if this id is mentioned in the text</returns>
        public bool MentionsRecipient()
        {
            return GetMentions().Where(mention => mention.Mentioned.Id == Recipient.Id).Any();
        }

        /// <summary>
        /// Remove recipient mention text from Text property
        /// </summary>
        /// <returns>new .Text property value</returns>
        public string RemoveRecipientMention()
        {
            return RemoveMentionText(Recipient.Id);
        }

        /// <summary>
        /// Replace any mention text for given id from Text property
        /// </summary>
        /// <param name="id">id to match</param>
        /// <returns>new .Text property value</returns>
        public string RemoveMentionText(string id)
        {
            foreach (var mention in GetMentions().Where(mention => mention.Mentioned.Id == id))
            {
                Text = Regex.Replace(Text, mention.Text, "", RegexOptions.IgnoreCase);
            }
            return Text;
        }

        /// <summary>
        /// Get channeldata as typed structure
        /// </summary>
        /// <typeparam name="TypeT">type to use</typeparam>
        /// <returns>typed object or default(TypeT)</returns>
        public TypeT GetChannelData<TypeT>()
        {
            if (ChannelData == null)
            {
                return default(TypeT);
            }

            return ((JObject)ChannelData).ToObject<TypeT>();
        }

        /// <summary>
        /// Return the "major" portion of the activity
        /// </summary>
        /// <returns>normalized major portion of the activity, aka message/... will return "message"</returns>
        public string GetActivityType()
        {
            var type = Type.Split('/').First();
            return GetActivityType(type);
        }

        public static string GetActivityType(string type)
        {
            if (string.Equals(type, ActivityTypes.Message, StringComparison.OrdinalIgnoreCase))
            {
                return ActivityTypes.Message;
            }

            if (string.Equals(type, ActivityTypes.ContactRelationUpdate, StringComparison.OrdinalIgnoreCase))
            {
                return ActivityTypes.ContactRelationUpdate;
            }

            if (string.Equals(type, ActivityTypes.ConversationUpdate, StringComparison.OrdinalIgnoreCase))
            {
                return ActivityTypes.ConversationUpdate;
            }

            if (string.Equals(type, ActivityTypes.Typing, StringComparison.OrdinalIgnoreCase))
            {
                return ActivityTypes.Typing;
            }

            if (string.Equals(type, ActivityTypes.Ping, StringComparison.OrdinalIgnoreCase))
            {
                return ActivityTypes.Ping;
            }

            return $"{char.ToLower(type[0])}{type.Substring(1)}";
        }
    }
}
