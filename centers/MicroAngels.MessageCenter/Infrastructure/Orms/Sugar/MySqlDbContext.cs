using Business.Models;
using Microsoft.Extensions.Configuration;
using SqlSugar;

namespace Infrastructure.Orms.Sugar
{
    public class MySqlDbContext
    {
        private static MySqlDbContext _current;
        public static MySqlDbContext Current
        {
            get
            {
                if (null == _current)
                {
                    _current = new MySqlDbContext();
                }

                return _current;
            }
        }

        public MySqlDbContext() { }

        public void Initial(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlSugarClient DB
        {
            get
            {
                return new SqlSugarClient(new ConnectionConfig
                {
                    ConnectionString = _configuration["Database:Mysql:Conn"],
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true
                });
            }
        }


        public SimpleClient<Topic> TopicsDb { get { return new SimpleClient<Topic>(DB); } }
        public SimpleClient<Message> MessageDb { get { return new SimpleClient<Message>(DB); } }
        public SimpleClient<UserMessage> UserMessageDb { get { return new SimpleClient<UserMessage>(DB); } }
        public SimpleClient<Subscribe> SubscribeDb { get { return new SimpleClient<Subscribe>(DB); } }
        private static IConfiguration _configuration;
    }

}
