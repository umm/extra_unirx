using System;
using System.Collections.Generic;
using UniRx;

namespace ExtraUniRx
{
    /// <summary>
    /// EmptyBehaviorSubject is BehaviorSubject which holds nothing on the beginning.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class EmptyBehaviorSubject<TValue> : ISubject<TValue>, IDisposable
    {
        private bool IsDisposed = false;
        private bool IsFirstEmitted = false;
        private TValue Value = default(TValue);
        private IList<IObserver<TValue>> ObserverList = new List<IObserver<TValue>>();

        public void OnCompleted()
        {
            if (this.IsDisposed) return;

            foreach (var observer in this.ObserverList)
            {
                observer.OnCompleted();
            }

            this.Dispose();
        }

        public void OnError(Exception error)
        {
            if (this.IsDisposed) return;

            foreach (var observer in this.ObserverList)
            {
                observer.OnError(error);
            }

            this.Dispose();
        }

        public void OnNext(TValue value)
        {
            if (this.IsDisposed) return;

            this.IsFirstEmitted = true;
            this.Value = value;

            foreach (var observer in this.ObserverList)
            {
                observer.OnNext(value);
            }
        }

        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            if (this.IsDisposed) return new Subscription(this);

            this.ObserverList.Add(observer);

            if (this.IsFirstEmitted)
            {
                observer.OnNext(this.Value);
            }

            return new Subscription(this);
        }

        public void Dispose()
        {
            this.IsDisposed = true;
            this.ObserverList.Clear();
        }

        class Subscription : IDisposable
        {
            private EmptyBehaviorSubject<TValue> subject;

            public Subscription(EmptyBehaviorSubject<TValue> subject)
            {
                this.subject = subject;
            }

            public void Dispose()
            {
                this.subject.Dispose();
            }
        }
    }
}