using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;

namespace MicroAngels.ORM.Suger
{

	public class MySqlContext
	{
		private static MySqlContext _current;
		public static MySqlContext Current
		{
			get
			{
				if (null == _current)
				{
					_current = new MySqlContext();
				}

				return _current;
			}
		}

		public MySqlContext() { }

		public void Initial(IConfiguration configuration)
		{
			_configuration = configuration;

			DB = new SqlSugarClient(new ConnectionConfig
			{
				ConnectionString = _configuration["Database:Mysql:Conn"],
				DbType = DbType.MySql,
				IsAutoCloseConnection = true,
				InitKeyType = InitKeyType.Attribute,
				//TODO: slave connection only for readeable SlaveConnectionConfigs=new List<SlaveConnectionConfig> { }
				ConfigureExternalServices = new ConfigureExternalServices()
				{
					//TODO:redis cache if possible
					//DataInfoCacheService = new SugarRedisCache();
				}
			});
		}

		public SqlSugarClient DB { get; protected set; }


		public void InitTable(Type type)
		{
			Current.DB.CodeFirst.InitTables(type);
		}

		public void InitTables(Type[] types)
		{
			Current.DB.CodeFirst.InitTables(types);
		}

		private IConfiguration _configuration;

	}

}
