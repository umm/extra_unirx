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