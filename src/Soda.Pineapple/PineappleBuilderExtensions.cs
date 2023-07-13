using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Soda.Pineapple.Services;

namespace Soda.Pineapple;

public static class PineappleBuilderExtensions
{
    public static void AddSodaPineapple(this IServiceCollection services)
    {
        var builder = new PineappleBuilder();
        builder.Services.AddSingleton<IMultipleTypeBuilderService, MultipleTypeBuilderService>();

        builder.Build();
    }

    public static void UseSodaPineapple(this IApplicationBuilder app)
    {
    }
}