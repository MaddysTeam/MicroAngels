﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace FileService
{
	public class Program
	{

		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseKestrel(options =>
				{
					options.Limits.MaxRequestBodySize = null;
				})
				.UseUrls("http://*:6000")
				.UseStartup<Startup>();

	}
}
