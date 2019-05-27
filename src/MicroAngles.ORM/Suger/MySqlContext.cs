using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngles.ORM.Suger
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

		private IConfiguration _configuration;

	}

}
