using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Common;
using SqlSugar;

namespace Business.Models
{

    /// <summary>
    /// 消息topic 用于管理
    /// </summary>
    public class Topic
    {
        public string Id { get; set; }

        [Required]
        public string ServiceId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime CreateTime { get; set; }

        [SugarColumn(IsIgnore=true)]
        public bool IsValidate => !ServiceId.IsNullOrEmpty() && !Name.IsNullOrEmpty();
    }


    /// <summary>
    /// 消息实体
    /// </summary>
    public class Message
    {
        public string Id { get; set; }   
        public string Topic { get;set; }
        public string TopicId { get;set; }
        public string TypeId { get;set; }
        public string Body { get;set; }
        public string StatusId { get;set;  }
        public string SenderId { get;set; }
        public string ServiceId { get;set; }
        public DateTime SendTime { get;set; }
        public DateTime ReceiveTime { get;set; }
        public TimeSpan Timeout { get;set; }
        public string SubscriberId { get; set; }
        public string TargetId { get; set; }

        //TODO
        public bool IsTimeout { get { return ReceiveTime + Timeout > DateTime.UtcNow; } }

        public bool IsValidate=>
               !Topic.IsNullOrEmpty() 
            && !Body.IsNullOrEmpty() 
            && !SenderId.IsNullOrEmpty()
            && !ServiceId.IsNullOrEmpty()
            && SendTime < DateTime.UtcNow;
        
    }


    /// <summary>
    /// 订阅
    /// </summary>
    public class Subscribe
    {
        public Subscribe() { }
        public Subscribe(string id,string serviceId, string topicId,string subscriberId,string targetId)
        {
            Id = id;
            ServiceId = serviceId;
            TopicId = topicId;
            TargetId = targetId;
            ServiceId = serviceId;
        }

        public string Id { get; private set; }
        public string SubscriberId { get; private set; }
        public string TopicId { get; private set; }
        public string TargetId { get; private set; }
        public string ServiceId { get; private set; }
        //public string TipsId { get; set; }
    }

    /// <summary>
    /// 用户消息
    /// </summary>
    public class UserMessage
    {
        public string Id { get; }
        //public string ServiceId { get; set; }
        public string MessageId { get; set; }
        public string ReceiverId { get; set; }
        public string StatusId { get; set; }
    }


    public enum MessageType
    {
        Announce,  // 公告
        Secret,    // 私信
        Notice ,   // 提醒
        Subscribe, // 订阅
    }

}
