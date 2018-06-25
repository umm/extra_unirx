using System;
using ExtraUniRx.Operators;
using UniRx;
using UniRx.Operators;

namespace ExtraUniRx.Operators
{
    public class CacheObservable<TValue> : OperatorObservableBase<TValue>
    {
        private IObservable<TValue> Source { get; set; }

        private TValue cachedValue;

        private TValue CachedValue
        {
            get { return cachedValue; }
            set
            {
                cachedValue = value;
                HasSetCachedValue = true;
            }
        }

        private bool HasSetCachedValue { get; set; }

        public CacheObservable(IObservable<TValue> source) : base(source.IsRequiredSubscribeOnCurrentThread())
        {
            Source = source;
        }

        private bool HasSubscribedForCache { get; set; }

        protected override IDisposable SubscribeCore(IObserver<TValue> observer, IDisposable cancel)
        {
            var observable = Source;
            if (!HasSubscribedForCache)
            {
                observable.Subscribe(x => CachedValue = x);
                HasSubscribedForCache = true;
            }

            var disposable = observable.Subscribe(observer.OnNext, observer.OnError);
            if (HasSetCachedValue)
            {
                observer.OnNext(CachedValue);
            }

            return disposable;
        }
    }
}

namespace ExtraUniRx
{
    public static partial class ObservableExtensions
    {
        public static IObservable<TValue> Cache<TValue>(this IObservable<TValue> source)
        {
            return new CacheObservable<TValue>(source);
        }
    }
}