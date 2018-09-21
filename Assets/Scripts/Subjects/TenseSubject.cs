using System;
using UniRx;

namespace ExtraUniRx
{
    public sealed class TenseSubject : ITenseSubject, IDisposable, IOptimizedObservable<Tense>
    {
        private ISubject<Tense> Subject { get; } = new Subject<Tense>();

        public void OnCompleted()
        {
            Subject.OnCompleted();
        }

        public void OnError(Exception error)
        {
            Subject.OnError(error);
        }

        public void OnNext(Tense value)
        {
            Subject.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<Tense> observer)
        {
            return Subject.Subscribe(observer);
        }

        public void Dispose()
        {
            ((IDisposable) Subject).Dispose();
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return Subject.IsRequiredSubscribeOnCurrentThread();
        }
    }

    public sealed class TenseSubject<T> : ITenseSubject<T>, IDisposable, IOptimizedObservable<Tuple<T, Tense>>
    {
        private ISubject<Tuple<T, Tense>> Subject { get; } = new Subject<Tuple<T, Tense>>();

        public void OnCompleted()
        {
            Subject.OnCompleted();
        }

        public void OnError(Exception error)
        {
            Subject.OnError(error);
        }

        public void OnNext(Tuple<T, Tense> value)
        {
            Subject.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<Tuple<T, Tense>> observer)
        {
            return Subject.Subscribe(observer);
        }

        public void Dispose()
        {
            ((IDisposable) Subject).Dispose();
        }

        public bool IsRequiredSubscribeOnCurrentThread()
        {
            return Subject.IsRequiredSubscribeOnCurrentThread();
        }
    }
}