using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }

    public class Robot
    {
        public int Id { get; set; }
        public string RobotName { get; set; }
        public string RobotType { get; set; }
    }
}
