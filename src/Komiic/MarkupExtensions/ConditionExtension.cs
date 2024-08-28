using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Komiic.MarkupExtensions;

public class ConditionExtension : MarkupExtension
{
    /// <summary>
    ///     缓存 Converter 避免无限制的创建
    /// </summary>
    private static readonly Dictionary<Key, IValueConverter> KnownConverters = [];

    public ConditionExtension()
    {
    }

    public ConditionExtension(BindingBase binding) : this()
    {
        Binding = binding;
    }

    public required BindingBase Binding { get; init; }

    public required object OnTrue { get; init; }

    public required object OnFalse { get; init; }

    public IValueConverter? Converter { get; init; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        // 按理解 结果值(True,False) 相同即可使用同一个 ConditionConverter
        var key = new Key(OnTrue, OnFalse);

        var converter = Converter;
        if (converter is null)
        {
            if (!KnownConverters.TryGetValue(key, out converter))
            {
                converter = new ConditionConverter(this);
                KnownConverters.TryAdd(key, converter);
            }
        }

        Binding.Converter = converter;
        return Binding;
    }

    private record Key(object? True, object? False) : IEqualityComparer<Key>
    {
        public bool Equals(Key? x, Key? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null)
            {
                return false;
            }

            if (y is null)
            {
                return false;
            }

            if (x.GetType() != y.GetType())
            {
                return false;
            }

            return Equals(x.True, y.True) && Equals(x.False, y.False);
        }

        public int GetHashCode(Key obj)
        {
            var hashCode = HashCode.Combine(obj.True, obj.False);
            return hashCode;
        }

        public virtual bool Equals(Key? other)
        {
            return Equals(this, other);
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }
    }

    private class ConditionConverter(ConditionExtension condition) : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var canConvert = targetType == condition.OnTrue.GetType() && targetType == condition.OnFalse.GetType();

            if (canConvert && value is bool conditionValue)
            {
                return conditionValue ? condition.OnTrue : condition.OnFalse;
            }

            if (canConvert)
            {
                return value is not null ? condition.OnTrue : condition.OnFalse;
            }

            return BindingOperations.DoNothing;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return BindingOperations.DoNothing;
        }
    }
}