using System;
using System.Collections.Generic;
using System.Linq;
using ExtraUniRx.Operators;
using UniRx;
using UniRx.Operators;

namespace ExtraUniRx.Operators
{
    public class CacheAllObservable<TValue> : OperatorObservableBase<TValue>
    {
        private IConnectableObservable<TValue> Source { get; set; }

        private List<TValue> CachedValueList { get; set; }

        public CacheAllObservable(IObservable<TValue> source) : base(source.IsRequiredSubscribeOnCurrentThread())
        {
            Source = source.Publish();
            CachedValueList = new List<TValue>();
        }

        private bool HasSubscribedForCache { get; set; }

        protected override IDisposable SubscribeCore(IObserver<TValue> observer, IDisposable cancel)
        {
            observer = new CacheAll(observer, cancel);
            if (!HasSubscribedForCache)
            {
                Source.Connect();
                Source.Subscribe(CachedValueList.Add);
                HasSubscribedForCache = true;
            }
            var disposable = Source.Subscribe(observer.OnNext, observer.OnError);
            if (CachedValueList.Any())
            {
                CachedValueList.ForEach(observer.OnNext);
            }

            return disposable;
        }

        private class CacheAll : OperatorObserverBase<TValue, TValue>
        {
            public CacheAll(IObserver<TValue> observer, IDisposable cancel) : base(observer, cancel)
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
        public static IObservable<TValue> CacheAll<TValue>(this IObservable<TValue> source)
        {
            return new CacheAllObservable<TValue>(source);
        }
    }
}