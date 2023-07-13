using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Soda.Pineapple.Services;

public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
{
    private static int _mMarker = 0;

    /// <summary>
    /// 改变模型映射，只要Create返回的值跟上次缓存的值不一样，EFCore就认为模型已经更新，需要重新加载
    /// </summary>
    public static void ChangeTableMapping()
    {
        Interlocked.Increment(ref _mMarker);
    }

    public object Create(DbContext context, bool designTime)
    {
        return (context.GetType(), _mMarker);
    }
}