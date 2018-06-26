using UniRx;

namespace ExtraUniRx
{
    public interface ICachedObservable<TValue> : IObservable<TValue>
    {
        TValue Value { get; }
    }
}