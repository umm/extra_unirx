using System;

namespace ExtraUniRx
{
    public interface ITenseObserver : IObserver<Tense>
    {
    }

    public interface ITenseObserver<T> : IObserver<Tuple<T, Tense>>
    {
    }
}