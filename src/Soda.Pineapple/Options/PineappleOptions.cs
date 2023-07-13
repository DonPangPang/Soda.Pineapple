using Microsoft.Extensions.Options;

namespace Soda.Pineapple.Options;

/// <summary>
/// <remarks>
/// 应包含的配置:
///  1. 分表规则
/// </remarks>
/// </summary>
public class PineappleOptions : IOptions<PineappleOptions>
{
    /// <summary>
    /// 分表规则(默认:月)
    /// </summary>
    public ITableSplittingRule SplittingRule { get; set; } = new SplitBaseOnDate(DateMode.YearMonth);

    public PineappleOptions Value => this;
}