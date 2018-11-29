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
                if(null== _current)
                {
                    _current = new MySqlDbContext();
                }

                return _current;
            }
        }

        public MySqlDbContext() { }

        public void Initial(IConfiguration configuration)
        {
            DB = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = configuration["Database:Mysql:Conn"],
                DbType = DbType.MySql,
                IsAutoCloseConnection = true
            });
        }

        public void Stop()
        {
            if (null != DB)
            {
                DB.Close();
            }
        }

        public SqlSugarClient DB;
        public SimpleClient<Robot> RobotDb { get { return new SimpleClient<Robot>(DB); } }
        public SimpleClient<Topic> TopicsDb { get { return new SimpleClient<Topic>(DB); } }
        public SimpleClient<Message> MessageDb { get { return new SimpleClient<Message>(DB); } }
    }

    public class Robot
    {
        public int Id { get; set; }
        public string RobotName { get; set; }
        public string RobotType { get; set; }
    }
}
