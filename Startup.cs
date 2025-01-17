using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace school_project
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

     
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<schoolprojectDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), b=>b.MigrationsAssembly("schoolproject.Data")));
            services.AddScoped<DbContext>(provider => provider.GetService<schoolprojectDbContext>());
            services.AddControllers();
            
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}
