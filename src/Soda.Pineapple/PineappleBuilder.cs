using Microsoft.Extensions.DependencyInjection;

namespace Soda.Pineapple;

internal class PineappleBuilder
{
    private static IServiceProvider? Instance { get; set; }

    public IServiceCollection Services { get; set; } = new ServiceCollection();

    public void Build()
    {
        Instance = Services.BuildServiceProvider();
    }

    public T GetService<T>() where T : notnull
    {
        if (Instance is null) throw new ArgumentNullException();

        return Instance.GetRequiredService<T>();
    }
}