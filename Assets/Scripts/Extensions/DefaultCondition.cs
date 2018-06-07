using System;
using UniRx;

namespace ExtraUniRx
{
    // ReSharper disable once PartialTypeWithSinglePart 他のファイルでも同一クラスを拡張する
    public static partial class UniRxExtension
    {
        public static IObservable<TSource> IsDefault<TSource>(this IObservable<TSource> source)
        {
            return source.Where(x => Equals(x, default(TSource)));
        }

        public static IObservable<TSource> IsDefault<TSource, TValue>(this IObservable<TSource> source, Func<TSource, TValue> selector)
        {
            return source.Where(x => Equals(x, default(TSource)) || Equals(selector(x), default(TValue)));
        }

        public static IObservable<TSource> IsNotDefault<TSource>(this IObservable<TSource> source)
        {
            return source.Where(x => !Equals(x, default(TSource)));
        }

        public static IObservable<TSource> IsNotDefault<TSource, TValue>(this IObservable<TSource> source, Func<TSource, TValue> selector)
        {
            return source.Where(x => !Equals(x, default(TSource)) && !Equals(selector(x), default(TValue)));
        }
    }
}