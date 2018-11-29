using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;

namespace Business.Models
{

    /// <summary>
    /// 消息topic 用于管理
    /// </summary>
    public class Topic
    {
        public string TopicId { get; }
        public string ServiceId { get; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string CreateTime { get; }
    }


    /// <summary>
    /// 消息实体
    /// </summary>
    public class Message
    {
        public string Id { get; }   
        public string TopicId { get; }
        public string Token { get; }
        public string Type { get; }
        public string Body { get; }
        public string Status { get; protected set; }
        //public string TipsId { get; }
        public string SenderId { get; }
        public string SenderServiceId { get; }
        public string LevelId { get; }
        public DateTime SendTime { get; }
        public DateTime ReceiveTime { get; }
        public TimeSpan Timeout { get; }

        //TODO
        public bool IsTimeout { get { return ReceiveTime + Timeout > DateTime.UtcNow; } }

        public void SetStatus(string status)
        {
            Status = status;
        }

    }


    /// <summary>
    /// 订阅
    /// </summary>
    public class Suscribe
    {
        public string Id { get;  }
        public string TopicId { get; set; }
        public string TargetId { get; set; }
        public string TipsId { get; set; }
    }

    /// <summary>
    /// 用户消息
    /// </summary>
    public class UserMessage
    {
        public string Id { get; }
        public string ServiceId { get; set; }
        public string MessageId { get; set; }
        public string ReceiverId { get; set; }
        public string IsReceive { get; set; }
        public string StatusId { get; set; }
    }

    public class MessageSearchOptions
    {
       
    }



    ///// <summary>
    ///// 接受结果
    ///// </summary>
    //public class ReceiveResult
    //{
    //    public bool IsSuccess { get; set; }
    //    public Message Current { get; }
    //}



}
