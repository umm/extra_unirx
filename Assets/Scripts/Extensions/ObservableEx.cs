using System.Collections.Generic;
using System;
using UniRx;

namespace ExtraUniRx
{
    public static class ObservableEx
    {
        public static IObservable<TValue> Returns<TValue>(IEnumerable<TValue> values)
        {
            return Observable.Create<TValue>(observer =>
            {
                foreach (var value in values)
                {
                    observer.OnNext(value);
                }

                observer.OnCompleted();
                return Disposable.Empty;
            });
        }
    }
}