//using Chloe.Infrastructure;
//using MySql.Data.MySqlClient;
//using System.Data;

//namespace Infrastructure
//{

//    public class MysqlConnectionFactory : IDbConnectionFactory
//    {

//        public MysqlConnectionFactory(string connString)
//        {
//            this.connString = connString;
//        }

//        public IDbConnection CreateConnection()
//        {
//           var conn = new Chloe.MySql.ChloeMySqlConnection(
//                new MySqlConnection(connString)
//                );

//            return conn;
//        }

//        private string connString;

//    }

//}