using MicroAngels.AuthServer.Services;
using MicroAngels.IdentityServer.Extensions;
using MicroAngels.IdentityServer.Services;
using MicroAngels.IdentityServer.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroAngels.AuthServer
{
	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // add mvc,include filters and etc..
            services.AddMvcCore()
            .AddControllersAsServices()
            .AddAuthorization()
            .AddJsonFormatters();

			services.AddIdentityServer(x =>
				{
					//x.IssuerUri = "http://identity";
					//x.PublicOrigin = "http://identity";
				})
				.AddDeveloperSigningCredential()
				.UseMysql(opt =>
				{
					opt.ConnectionStrings = Configuration["Database:Mysql:Conn"];  // "Database=idsDemo;Data Source=192.168.1.9;User Id=root;Password=abc123456;port=3306"; //"Database=idsDemo;Data Source=192.168.1.9;User Id=root;Password=abc123456;pooling=false;CharSet=utf8;port=3306";
				})
				.UseValidateService<UserValidateService>()
				.UseClaimsGrantService<UserClaimGrantService>()
				.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
				.AddProfileService<UserClaimsProfileService>(); // add claims into user profile （such as context）
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IApplicationLifetime lifeTime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseIdentityServer();
        }
    }
}
