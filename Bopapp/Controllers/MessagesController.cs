﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Web.Services.Description;
using Bopapp.Dialogs;
using System;
using System.Text;
using System.Web;
using JiebaNet.Segmenter;

namespace Bopapp
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// 默认为POST方法，请求被映射到该函数。
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                //  await Conversation.SendAsync(activity, () => new EchoDialog());                
                // await Conversation.SendAsync(activity, () => new LuisDialog());
                DictionaryTree.LoadDic(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + "Synonymous.txt");
                Cache.LoadCache(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + "cache.txt");

                await Conversation.SendAsync(activity, () => new FinDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                return message.CreateReply("typing received!");
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }
            return message.CreateReply("something received!"); ;
        }
    }
}