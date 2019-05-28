using Business.Models;
using MicroAngels.ORM.Suger;
using Microsoft.Extensions.Configuration;
using SqlSugar;

namespace Infrastructure.Orms.Sugar
{

	public partial class MySqlDbContext
	{

		public SqlSugarClient DB => MySqlContext.Current.DB;

		public SimpleClient<Topic> TopicsDb { get { return new SimpleClient<Topic>(DB); } }
		public SimpleClient<Message> MessageDb { get { return new SimpleClient<Message>(DB); } }
		public SimpleClient<UserMessage> UserMessageDb { get { return new SimpleClient<UserMessage>(DB); } }
		public SimpleClient<Subscribe> SubscribeDb { get { return new SimpleClient<Subscribe>(DB); } }

	}

}
