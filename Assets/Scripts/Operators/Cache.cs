using System;
using ExtraUniRx.Operators;
using UniRx;
using UniRx.Operators;

namespace ExtraUniRx.Operators
{
    public class CacheObservable<TValue> : OperatorObservableBase<TValue>, ICachedObservable<TValue>
    {
        private IConnectableObservable<TValue> Source { get; set; }

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

        public TValue Value
        {
            get { return CachedValue; }
        }

        private bool HasSetCachedValue { get; set; }

        private bool HasCompleted { get; set; }

        public CacheObservable(IObservable<TValue> source) : base(source.IsRequiredSubscribeOnCurrentThread())
        {
            Source = source.Publish();
        }

        private bool HasSubscribedForCache { get; set; }

        protected override IDisposable SubscribeCore(IObserver<TValue> observer, IDisposable cancel)
        {
            observer = new Cache(observer, cancel);
            if (!HasSubscribedForCache)
            {
                Source.Connect();
                Source.Subscribe(
                    x => CachedValue = x,
                    () =>
                    {
                        HasCompleted = true;
                        observer.OnCompleted();
                    }
                );
                HasSubscribedForCache = true;
            }

            var disposable = Source.Subscribe(observer.OnNext, observer.OnError);
            if (HasSetCachedValue)
            {
                observer.OnNext(CachedValue);
            }

            if (HasCompleted)
            {
                observer.OnCompleted();
            }

            return disposable;
        }

        private class Cache : OperatorObserverBase<TValue, TValue>
        {
            public Cache(IObserver<TValue> observer, IDisposable cancel) : base(observer, cancel)
            {
            }

            public override void OnNext(TValue value)
            {
                try
                {
                    observer.OnNext(value);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public override void OnError(Exception error)
            {
                try
                {
                    observer.OnError(error);
                }
                finally
                {
                    Dispose();
                }
            }

            public override void OnCompleted()
            {
                try
                {
                    observer.OnCompleted();
                }
                finally
                {
                    Dispose();
                }
            }
        }
    }
}

namespace ExtraUniRx
{
    public static partial class ObservableExtensions
    {
        public static ICachedObservable<TValue> Cache<TValue>(this IObservable<TValue> source)
        {
            return new CacheObservable<TValue>(source);
        }
    }
}