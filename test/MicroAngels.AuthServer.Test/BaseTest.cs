using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using System.Reflection;

namespace MicroAngels.AuthServer.Test
{

    public class BaseTest
    {

		protected Guid SystemId = Guid.Parse("da7f1c57-e62e-4faf-8028-9b848269e437");

		public BaseTest()
		{
			Server = new TestServer(
			WebHost.CreateDefaultBuilder().UseContentRoot(
					GetProjectPath("MicroAngels.sln", "", typeof(Startup).Assembly).
					Replace(@"\MicroAngels.AuthServer", @"\centers\MicroAngels.AuthServer")
				).UseStartup<Startup>()
			);
		}

		protected TestServer Server { get;  }

		protected virtual string GetProjectPath(string slnName, string solutionRelativePath, Assembly startupAssembly)
        {
            string projectName = startupAssembly.GetName().Name;
            string applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, slnName));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName));
                }

                directoryInfo = directoryInfo.Parent;
            } while (directoryInfo.Parent != null); throw new Exception($"Solution root could not be located using application root {applicationBasePath}.");
        }

    }

}
