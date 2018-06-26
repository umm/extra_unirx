using System;
using System.Collections.Generic;
using System.Linq;
using ExtraUniRx.Operators;
using UniRx;
using UniRx.Operators;

namespace ExtraUniRx.Operators
{
    public class CacheAllObservable<TValue> : OperatorObservableBase<TValue>, ICachedObservable<TValue>
    {
        private IConnectableObservable<TValue> Source { get; set; }

        private List<TValue> CachedValueList { get; set; }

        public TValue Value
        {
            get { return CachedValueList.LastOrDefault(); }
        }

        public CacheAllObservable(IObservable<TValue> source) : base(source.IsRequiredSubscribeOnCurrentThread())
        {
            Source = source.Publish();
            CachedValueList = new List<TValue>();
        }

        private bool HasSubscribedForCache { get; set; }

        private Exception ThrownException { get; set; }

        private bool HasCompleted { get; set; }

        protected override IDisposable SubscribeCore(IObserver<TValue> observer, IDisposable cancel)
        {
            observer = new CacheAll(observer, cancel);
            if (!HasSubscribedForCache)
            {
                Source.Connect();
                Source.Subscribe(
                    CachedValueList.Add,
                    ex => ThrownException = ex,
                    () =>
                    {
                        HasCompleted = true;
                        observer.OnCompleted();
                    }
                );
                HasSubscribedForCache = true;
            }
            var disposable = Source.Subscribe(observer.OnNext);
            if (CachedValueList.Any())
            {
                CachedValueList.ForEach(observer.OnNext);
            }

            if (ThrownException != default(Exception))
            {
                observer.OnError(ThrownException);
            }
            else if (HasCompleted)
            {
                observer.OnCompleted();
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
        public static ICachedObservable<TValue> CacheAll<TValue>(this IObservable<TValue> source)
        {
            return new CacheAllObservable<TValue>(source);
        }
    }
}