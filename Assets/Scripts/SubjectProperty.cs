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
    public class SubjectProperty<TValue> : ISubjectProperty<TValue>
    {
        private TValue internalValue;

        public TValue Value
        {
            set
            {
                if (!HasValue)
                {
                    HasValue = true;
                }
                internalValue = value;
                subject.OnNext(value);
            }
            get { return internalValue; }
        }

        public bool HasValue { get; private set; }

        /// <summary>
        /// Set value without any updates. This is for using initialization
        /// </summary>
        public TValue InternalValue
        {
            set { internalValue = value; }
        }

        private readonly ISubject<TValue> subject = new Subject<TValue>();

        public void OnCompleted()
        {
            subject.OnCompleted();
        }

        public void OnError(Exception error)
        {
            subject.OnError(error);
        }

        public void OnNext(TValue value)
        {
            Value = value;
        }

        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            return subject.Subscribe(observer);
        }
    }
}