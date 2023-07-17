using Microsoft.Extensions.DependencyInjection;

namespace Soda.Pineapple.Extensions;

internal static class LazyResolutionExtensions
{
    public static IServiceCollection AddLazyResolution(this IServiceCollection services)
    {
        return services.AddTransient(
            typeof(Lazy<>),
            typeof(LazilyResolved<>));
    }
}

internal class LazilyResolved<T> : Lazy<T> where T : notnull
{
    public LazilyResolved(IServiceProvider serviceProvider)
        : base(serviceProvider.GetRequiredService<T>)
    {
    }
}