using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.Options;
using Soda.Pineapple.Options;

namespace Soda.Pineapple.Services;

public interface IMultipleTypeBuilderService
{
    void GenerateType(Type baseType);

    IEnumerable<Type> GetTypes();

    IEnumerable<Type> GetTypes<T>() where T : class;
}

internal class MultipleTypeBuilderService : IMultipleTypeBuilderService
{
    private Lazy<IOptions<PineappleOptions>> Options => PineappleBuilder.GetService<IOptions<PineappleOptions>>();
    private readonly ConcurrentDictionary<string, MultipleTypeContainer> _typesContainers = new();
    private readonly ModuleBuilder _moduleBuilder;

    public MultipleTypeBuilderService()
    {
        _moduleBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("_PineappleRuntimeTypes"), AssemblyBuilderAccess.Run).DefineDynamicModule("_RuntimeTypes");
    }

    private string Suffix => Options.Value.Value.SplittingRule.GetSuffix();

    public void GenerateType(Type baseType)
    {
        if (_typesContainers.TryGetValue(nameof(baseType), out var container))
        {
        }
        else
        {
            container ??= new MultipleTypeContainer();
            _typesContainers.TryAdd(nameof(baseType), container);
        }

        var type = BuildType(baseType);

        container.Add(type);
    }

    private Type BuildType(Type baseType)
    {
        var typeName = $"{nameof(baseType)}_{Suffix}";
        var type = _moduleBuilder.GetType(typeName, true);
        if (type != null)
        {
            return type;
        }

        var builder = _moduleBuilder.DefineType(typeName, TypeAttributes.Public);
        builder.SetParent(baseType);

        type = builder.CreateTypeInfo().UnderlyingSystemType;
        return type;
    }

    public IEnumerable<Type> GetTypes()
    {
        return _typesContainers.Values.SelectMany(x => x.Types);
    }

    public IEnumerable<Type> GetTypes<T>() where T : class
    {
        if (_typesContainers.TryGetValue(nameof(T), out var container))
        {
            return container.Types;
        }

        return Enumerable.Empty<Type>();
    }
}

internal class MultipleTypeContainer
{
    private readonly ConcurrentDictionary<string, Type> _types = new();

    public MultipleTypeContainer()
    {
    }

    public void Add(Type type)
    {
        _types.TryAdd(nameof(type), type);
    }

    public void Remove(Type type)
    {
        _types.TryRemove(nameof(type), out _);
    }

    public IEnumerable<Type> Types => _types.Values;
}