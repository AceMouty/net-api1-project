using Microsoft.Extensions.DependencyInjection;
using Posts.Application.Repositories;

namespace Posts.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPostRepository, PostRepository>();
        return services;
    }

    public static IServiceProvider AddDatabase(this IServiceProvider services)
    {
        var repository = services.GetRequiredService<IPostRepository>();
        repository.InitDb();

        return services;
    }
}