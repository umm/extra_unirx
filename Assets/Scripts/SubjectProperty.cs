using System;
using UniRx;

namespace ExtraUniRx
{
    /// <summary>
    /// SubjectProperty is similar to ReactiveProperty.
    ///
    /// Difference:
    /// - Behaviour of IObserver.
    /// - Emit values only if previous value is same.
    ///
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class SubjectProperty<TValue> : ISubject<TValue>
    {
        private TValue internalValue;

        public TValue Value
        {
            set
            {
                this.internalValue = value;
                this.Subject.OnNext(value);
            }
            get { return this.internalValue; }
        }

        /// <summary>
        /// Set value without any updates. This is for using initialization
        /// </summary>
        public TValue InternalValue
        {
            set { this.internalValue = value; }
        }

        private ISubject<TValue> Subject = new Subject<TValue>();

        private IObservable<TValue> Source { get; }

        public void OnCompleted()
        {
            this.Subject.OnCompleted();
        }

        public void OnError(Exception error)
        {
            this.Subject.OnError(error);
        }

        public void OnNext(TValue value)
        {
            this.Value = value;
        }

        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            if (this.Source != default(IObservable<TValue>))
            {
                this.Source.Subscribe(observer);
            }
            return this.Subject.Subscribe(observer);
        }

        public SubjectProperty()
        {
        }

        public SubjectProperty(IObservable<TValue> source) : this()
        {
            this.Source = source
                .Do(OnNext)
                .DoOnError(OnError)
                .DoOnCompleted(OnCompleted);
        }
    }
}