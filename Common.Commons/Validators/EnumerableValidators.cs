using FluentValidation;

namespace Common.Commons.Validators;

public static class EnumerableValidators
{
    public static IRuleBuilderOptions<T, IEnumerable<TItem>> NotDuplicated<T, TItem>(
        this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder)
    {
        return ruleBuilder.Must(p => p == null||p.Distinct().Count()==p.Count());
        
    }

    public static IRuleBuilderOptions<T, IEnumerable<TItem>> NotContains<T, TItem>(
        this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder,TItem comparedValue)
    {
        return ruleBuilder.Must(p => p == null || !p.Contains(comparedValue));

    }
    public static IRuleBuilderOptions<T, IEnumerable<TItem>> Contains<T, TItem>(this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder, TItem comparedValue)
    {
        return ruleBuilder.Must(p => p == null || p.Contains(comparedValue));
    }
}