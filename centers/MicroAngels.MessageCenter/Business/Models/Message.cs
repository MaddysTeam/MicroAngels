using MicroAngels.Core;
using SqlSugar;
using System;

namespace Business
{

	/// <summary>
	/// 消息topic 用于管理
	/// </summary>
	public class Topic
    {
		[SugarColumn(IsPrimaryKey =true,Length =50)]
        public string Id { get; set; }

		[SugarColumn(IsNullable = false)]
		public string ServiceId { get; set; }

        [SugarColumn(IsNullable =false)]
        public string Name { get; set; }

		[SugarColumn(IsNullable = true)]
        public string Description { get; set; }

		[SugarColumn(IsNullable = false)]
		public DateTime CreateTime { get; set; }

        [SugarColumn(IsIgnore = true)]
        public bool IsValidate => !ServiceId.IsNullOrEmpty() && !Name.IsNullOrEmpty();
    }


    /// <summary>
    /// 消息实体
    /// </summary>
    public class Message
    {
		[SugarColumn(IsPrimaryKey = true, Length = 50)]
		public string Id { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public string TopicId { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public string TypeId { get; set; }

		[SugarColumn(IsNullable = false, Length = 200)]
		public string Title { get; set; }

        public string Body { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public string StatusId { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public string SenderId { get; set; }

		[SugarColumn(IsNullable = false, Length = 50)]
		public string ServiceId { get; set; }

        public DateTime SendTime { get; set; }

        public DateTime ReceiveTime { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string Topic { get; set; }

        [SugarColumn(IsIgnore = true)]
        public TimeSpan Timeout { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string SubscriberId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string TargetId { get; set; }

        //TODO
        [SugarColumn(IsIgnore = true)]
        public bool IsTimeout { get { return ReceiveTime + Timeout > DateTime.UtcNow; } }

		[SugarColumn(IsIgnore = true)]
		public bool IsValidate =>
			   !Topic.IsNullOrEmpty()
			&& !Body.IsNullOrEmpty()
			&& !SenderId.IsNullOrEmpty()
			&& !ServiceId.IsNullOrEmpty();
            //&& SendTime < DateTime.UtcNow;

    }


    /// <summary>
    /// 订阅
    /// </summary>
    public class Subscribe
    {
        public Subscribe() { }
        public Subscribe(string id, string serviceId, string topicId, string subscriberId, string targetId)
        {
            Id = id;
            ServiceId = serviceId;
            TopicId = topicId;
            TargetId = targetId;
            SubscriberId = subscriberId;
        }

        public string Id { get; set; }
        public string SubscriberId { get; set; }
        public string TopicId { get; set; }
        public string TargetId { get; set; }
        public string ServiceId { get; set; }
        //public string TipsId { get; set; }
    }

    /// <summary>
    /// 用户消息
    /// </summary>
    public class UserMessage
    {
        public string Id { get; set; }
        public string ServiceId { get; set; } // 接收时用，表默认示默人在某个service 中已收某条消息，新增时为空
        public string MessageId { get; set; }
        public string ReceiverId { get; set; }
        public string StatusId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public Message Message { get; set; }

        [SugarColumn(IsIgnore = true)]
        public bool IsValidate => !MessageId.IsNullOrEmpty() && !ReceiverId.IsNullOrEmpty() && !StatusId.IsNullOrEmpty()    ;
    }


    public enum MessageType
    {
        Announce,  // 公告
        Secret,    // 私信
        Notice,   // 提醒
        Subscribe, // 订阅
    }

}
