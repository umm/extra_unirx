#if (NET_4_6)
using System;
#else
using UniRx;
#endif

namespace ExtraUniRx
{
    public interface ICachedObservable<TValue> : IObservable<TValue>
    {
        TValue Value { get; }
    }
}