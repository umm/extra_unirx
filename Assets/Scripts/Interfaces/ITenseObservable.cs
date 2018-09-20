using System;

namespace ExtraUniRx
{
    public interface ITenseObservable : IObservable<Tense>
    {
    }

    public interface ITenseObservable<T> : IObservable<Tuple<T, Tense>>
    {
    }
}