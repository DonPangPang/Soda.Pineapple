using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Soda.Pineapple.Actions;
using Soda.Pineapple.Extensions;
using Soda.Pineapple.Generators;
using Soda.Pineapple.Options;
using Soda.Pineapple.Services;
using Soda.Pineapple.Services.PublicServices;

namespace Soda.Pineapple;

public static class PineappleBuilderExtensions
{
    public static IPineappleBuilder AddSodaPineapple<TDbContext>(
        this IServiceCollection services, 
        Action<DbContextOptionsBuilder> builderAction,
        ServiceLifetime lifetime = ServiceLifetime.Scoped,
        Action<PineappleOptions>? actionOption = default)
    where TDbContext:DbContext
    {
        #region 公有服务
        services.AddDbContext<PineappleDbContext<TDbContext>>(builderAction, lifetime);
        services.AddTransient(typeof(AutoDbContext<>));
        #endregion

        #region 私有服务
        
        var builder = new PineappleBuilder();
        builder.Services.AddDbContext<PineappleDbContext>(builderAction);
        
        // 懒加载
        builder.Services.AddLazyResolution();
        // MultipleType构建
        builder.Services.AddSingleton<IMultipleTypeBuilderService, MultipleTypeBuilderService>();
        builder.Services.AddScoped<IVirtualTableService, VirtualTableService>();

        var options = new PineappleOptions();
        actionOption?.Invoke(options);
        builder.Services.Configure<PineappleOptions>(opt =>
        {
            opt.SplittingRule = options.SplittingRule;
        });
        
        builder.Services.AddActions();
        builder.Services.AddGenerators();
        builder.Services.AddJobs();
        
        builder.Build();
        #endregion

        return builder;
    }

    private static void AddActions(this IServiceCollection services)
    {
        services.AddTransient(typeof(CreateTableAction));
    }

    private static void AddGenerators(this IServiceCollection services)
    {
        services.AddTransient(typeof(CreateTableGenerator));
    }

    private static void AddJobs(this IServiceCollection services)
    {
        
    }

    public static void UseSodaPineapple(this IApplicationBuilder app)
    {
    }
}