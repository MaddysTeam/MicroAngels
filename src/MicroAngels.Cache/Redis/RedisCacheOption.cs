using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Cache.Redis
{

	public class RedisCacheOption
	{
		private string v1;
		private int v2;
		private string v3;
		private int v4;
		private int v5;

		public RedisCacheOption(string host,int port ,int defaultDB,long timeoutSeconds)
			:this(host,port,null,defaultDB,0,timeoutSeconds)
		{

		}


		public RedisCacheOption(string host, int port, string password, int defaultDB,long poolSize, long timeoutSeconds)
		{
			Host = host;
			Port = port;
			Password = password;
			DefaultDB = defaultDB;
		}

		public string Conn => string.Format("{0}:{1},password={2},defaultDatabase={3},poolsize={4}",Host,Port,Password, DefaultDB, PoolSize);


		public string Host { get; private set; }
		public int Port { get; private set; }
		public int DefaultDB { get; private set; }
		public string Password { get; private set; }
		public int PoolSize { get; private set; }
		public long TimeoutSeconds { get; private set; }

	}

}
