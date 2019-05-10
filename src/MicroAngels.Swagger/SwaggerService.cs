using MicroAngels.Core.Service;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroAngels.Swagger
{

    public class SwaggerService : IService
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
        public string Version { get; set; }
        public Uri Address { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Group { get; set; }
        public string Doc { get; set; }
        public string HealthStatus { get; set; }
        public string XMLPath { get; set; }
    }


    public class SwaggerUIOptions
    {
        public bool IsShowExtensions { get; set; }
		public IDocumentFilter DocumentFilter { get; set; }
	}

}
