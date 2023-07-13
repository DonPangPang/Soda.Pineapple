namespace Soda.Pineapple.Options;

public interface ITableSplittingRule
{
    string GetSuffix();
}

public class SplitBaseOnDate : ITableSplittingRule
{
    private readonly DateMode _mode;

    public SplitBaseOnDate(DateMode mode = DateMode.YearMonth)
    {
        _mode = mode;
    }

    public string GetSuffix()
    {
        return _mode switch
        {
            DateMode.Year => DateTime.Now.ToLocalTime().ToString("yyyy"),
            DateMode.YearMonth => DateTime.Now.ToLocalTime().ToString("yyyyMM"),
            DateMode.YearMonthDay => DateTime.Now.ToLocalTime().ToString("yyyyMMdd"),
            DateMode.YearMonthDayHour => DateTime.Now.ToLocalTime().ToString("yyyyMMddHH"),
            DateMode.YearMonthDayHourMin => DateTime.Now.ToLocalTime().ToString("yyyyMMddHHmm"),
            _ => DateTime.Now.ToLocalTime().ToString("yyyyMM"),
        };
    }
}

public enum DateMode
{
    Year,
    YearMonth,
    YearMonthDay,
    YearMonthDayHour,
    YearMonthDayHourMin,
}