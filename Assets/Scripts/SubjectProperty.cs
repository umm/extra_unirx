using System;
using UniRx;

namespace ExtraUniRx
{
    /// <summary>
    /// Interface for SubjectProperty act as ISubject and IReactiveProperty
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface ISubjectProperty<TValue> : ISubject<TValue>, IReactiveProperty<TValue>
    {
    }

    /// <summary>
    /// SubjectProperty is similar to ReactiveProperty.
    ///
    /// Difference:
    /// - Behaviour of IObserver.
    /// - Emit values only if previous value is same.
    ///
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class SubjectProperty<TValue> : ReactiveProperty<TValue>, ISubjectProperty<TValue>
    {
        private TValue internalValue;

        public new TValue Value
        {
            set
            {
                if (!HasValue)
                {
                    HasValue = true;
                }
                this.internalValue = value;
                this.Subject.OnNext(value);
            }
            get { return this.internalValue; }
        }

        public new bool HasValue { get; private set; }

        /// <summary>
        /// Set value without any updates. This is for using initialization
        /// </summary>
        public TValue InternalValue
        {
            set { this.internalValue = value; }
        }

        private ISubject<TValue> Subject { get; } = new Subject<TValue>();

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

        public new IDisposable Subscribe(IObserver<TValue> observer)
        {
            return this.Subject.Subscribe(observer);
        }
    }
}