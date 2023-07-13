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
}

internal class MultipleTypeBuilderService : IMultipleTypeBuilderService
{
    private readonly IOptions<PineappleOptions> _options;
    private readonly ConcurrentDictionary<string, MultipleTypeContainer> _typesContainers = new();
    private readonly ModuleBuilder _moduleBuilder;

    public MultipleTypeBuilderService(IOptions<PineappleOptions> options)
    {
        _options = options;
        _moduleBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("_RuntimeTypes"), AssemblyBuilderAccess.Run).DefineDynamicModule("_RuntimeTypes");
    }

    private string Suffix => _options.Value.SplittingRule.GetSuffix();

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