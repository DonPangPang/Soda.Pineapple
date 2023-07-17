using Microsoft.Extensions.DependencyInjection;

namespace Soda.Pineapple;

public interface IPineappleBuilder
{
    void Build();
    IPineappleBuilder ReplaceServices<TInterface, TService>() where TInterface : class where TService : class, TInterface;
}

internal class PineappleBuilder:IPineappleBuilder
{
    private static IServiceProvider? Instance { get; set; }

    public IServiceCollection Services { get; set; } = new ServiceCollection();

    public void Build()
    {
        Instance = Services.BuildServiceProvider();
    }

    public static Lazy<T> GetService<T>() where T : notnull
    {
        if (Instance is null) throw new ArgumentNullException();

        return Instance.GetRequiredService<Lazy<T>>();
    }
    
    public IPineappleBuilder ReplaceServices<TInterface, TService>() where TInterface:class where TService:class, TInterface
    {
        var leftTime = Services?.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(TInterface))?.Lifetime;

        switch (leftTime)
        {
            case ServiceLifetime.Singleton:
                Services?.AddSingleton<TInterface, TService>();
                break;
            case ServiceLifetime.Scoped:
                Services?.AddScoped<TInterface, TService>();
                break;
            case ServiceLifetime.Transient:
                Services?.AddTransient<TInterface, TService>();
                break;
            case null:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        Build();

        return this;
    }
}