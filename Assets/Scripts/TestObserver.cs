using System;
using System.Collections.Generic;
using UniRx;

namespace ExtraUniRx
{
    public class TestObserver<TValue> : IObserver<TValue>
    {
        public int OnCompletedCount { get; private set; }

        public int OnNextCount { get; private set; }

        public int OnErrorCount { get; private set; }

        public List<TValue> OnNextValues { get; private set; }

        public List<Exception> OnErrorValues { get; private set; }

        public TValue OnNextLastValue
        {
            get
            {
                return this.OnNextValues.Count > 0 ? this.OnNextValues[this.OnNextValues.Count - 1] : default(TValue);
            }
        }

        public Exception OnErrorLastValue
        {
            get
            {
                return this.OnErrorValues.Count > 0
                    ? this.OnErrorValues[this.OnErrorValues.Count - 1]
                    : default(Exception);
            }
        }

        public TestObserver()
        {
            this.OnNextValues = new List<TValue>();
            this.OnErrorValues = new List<Exception>();
        }

        public void OnCompleted()
        {
            this.OnCompletedCount++;
        }

        public void OnError(Exception error)
        {
            this.OnErrorCount++;
            this.OnErrorValues.Add(error);
        }

        public void OnNext(TValue value)
        {
            this.OnNextCount++;
            this.OnNextValues.Add(value);
        }
    }
}