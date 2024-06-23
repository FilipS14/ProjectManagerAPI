using DataBase.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using DataBase.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Core.Services;
using Project.Database.Repositories;
using Database.Repositories;


namespace API
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddServices();
            services.AddRepository();
            services.AddDbContext<ProjectDbContext>(options =>
                options.UseNpgsql("Host=localhost;Database=ProjectManager;Username=postgres;Password=1q2w3e"));
        }


        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<TaskService>();
        }
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            
        }

    }
}
