using Business.Helpers;
using Business.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using Xunit;

namespace MessageCenter.Test
{
    public class MessageServiceTest:BaseTest
    {

        string topic = "service123.MyTopic";
        string serviceId = "service123";
        string body = "Hello kafka";
        string senderId = "Boss";
        string subscriberId = "Boss";
        string targetId = "Jimmy";

        public MessageServiceTest():base()
        {
            _messageService = Server.Host.Services.GetService<IMessageService>();
            _subscribeService = Server.Host.Services.GetService<ISubscribeService>();
        }


        [Fact]
        public async void GetMessagesWithPageShouldOk()
        {
            int pageCount = 0;
            var messages = await _messageService.GetMessagesAsync(null, null, null, 0, 0, out pageCount);
            Assert.True(messages.Count > 0);

            messages = await _messageService.GetMessagesAsync(topic, null, null, 0, 0, out pageCount);
            Assert.True(
                messages.Count > 0 &&
                messages.Exists(m => m.Topic == topic && m.Body == body));

        }

        [Fact]
        public async void ReceiveSubscribeMessageShouldOK()
        {
            dynamic mo = new { Topic = topic, Body = body, ServiceId = serviceId, SenderId = senderId, SenderTime = "2018-12-01", SubscriberId = subscriberId, TargetId = targetId };
            var message = JsonConvert.SerializeObject(mo);
            var result = await _messageService.SubscribeAsync(message);

            Assert.True(result);
        }

        [Fact]  
        public async void ReceiveNotifyMessageShouldOk()
        {
            dynamic mo = new { Topic = topic, Body = body, ServiceId = serviceId, SenderId = targetId, SenderTime = DateTime.UtcNow};
            var message = JsonConvert.SerializeObject(mo);
            var result = await _messageService.NotifyAsync(message);
            var count = 0;
            var userMessages = await _messageService.GetUserMessagesAsync(subscriberId, serviceId,null, StaticKeys.MessageTypeId_Notify, 0, 0, out count);

            Assert.True(count>0);
            Assert.True(userMessages.Exists(um=>um.ReceiverId==subscriberId));
            Assert.NotEmpty(userMessages);
        }


        [Fact]
        public async void ReceiveAnnounceMessageShouldOk()
        {
            dynamic mo = new { Topic = topic, Body = body, ServiceId = serviceId, SenderId = senderId, SenderTime = "2018-12-01" };
            var message = JsonConvert.SerializeObject(mo);
            var result = await _messageService.AnnounceAsync(message);

            Assert.True(result);
        }


        private IMessageService _messageService;
        private ISubscribeService _subscribeService;

    }
}