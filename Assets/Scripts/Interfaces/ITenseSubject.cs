using System;
using UniRx;

namespace ExtraUniRx
{
    public interface ITenseSubject : ISubject<Tense>, ITenseObserver, ITenseObservable
    {
    }

    public interface ITenseSubject<T> : ISubject<Tuple<T, Tense>>, ITenseObserver<T>, ITenseObservable<T>
    {
    }
}