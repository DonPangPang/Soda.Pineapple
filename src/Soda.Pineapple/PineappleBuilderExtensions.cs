using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Soda.Pineapple.Extensions;
using Soda.Pineapple.Options;
using Soda.Pineapple.Services;

namespace Soda.Pineapple;

public static class PineappleBuilderExtensions
{
    public static void AddSodaPineapple(this IServiceCollection services)
    {
        var builder = new PineappleBuilder();
        // 懒加载
        builder.Services.AddLazyResolution();
        // MultipleType构建
        builder.Services.AddSingleton<IMultipleTypeBuilderService, MultipleTypeBuilderService>();
        builder.Services.ConfigureOptions<PineappleOptions>();

        builder.Build();
    }

    public static void UseSodaPineapple(this IApplicationBuilder app)
    {
    }
}