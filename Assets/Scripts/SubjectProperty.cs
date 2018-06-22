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

        private Action onSubscribe;

        public Action OnSubscribe
        {
            private get { return onSubscribe ?? (onSubscribe = () => { }); }
            set { onSubscribe = value; }
        }

        public IDisposable Subscribe(IObserver<TValue> observer)
        {
            return this.Subject.DoOnSubscribe(OnSubscribe).Subscribe(observer);
        }
    }

    public static class SubjectPropertyExtensions
    {
        public static SubjectProperty<TValue> ToSubjectProperty<TValue>(this IObservable<TValue> source)
        {
            var subjectProperty = new SubjectProperty<TValue>();
            subjectProperty.OnSubscribe = () => source.Subscribe(subjectProperty);
            return subjectProperty;
        }
    }
}