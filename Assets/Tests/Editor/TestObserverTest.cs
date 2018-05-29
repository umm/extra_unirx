using System;
using NUnit.Framework;
using UniRx;

namespace ExtraUniRx
{
    public class TestObserverTest
    {
        [Test]
        public void EventTest()
        {
            // OnNext, OnError
            {
                var subject = new Subject<int>();
                var observer = new TestObserver<int>();
                subject.Subscribe(observer);

                subject.OnNext(10);
                Assert.AreEqual(1, observer.OnNextCount);
                Assert.AreEqual(10, observer.OnNextLastValue);
                Assert.AreEqual(0, observer.OnCompletedCount);
                Assert.AreEqual(0, observer.OnErrorCount);

                subject.OnNext(20);
                Assert.AreEqual(2, observer.OnNextCount);
                Assert.AreEqual(20, observer.OnNextLastValue);
                Assert.AreEqual(0, observer.OnCompletedCount);
                Assert.AreEqual(0, observer.OnErrorCount);

                subject.OnCompleted();
                Assert.AreEqual(2, observer.OnNextCount);
                Assert.AreEqual(20, observer.OnNextLastValue);
                Assert.AreEqual(1, observer.OnCompletedCount);
                Assert.AreEqual(0, observer.OnErrorCount);
            }

            // OnNext, OnError
            {
                var subject = new Subject<int>();
                var observer = new TestObserver<int>();
                subject.Subscribe(observer);

                subject.OnNext(10);
                Assert.AreEqual(1, observer.OnNextCount);
                Assert.AreEqual(10, observer.OnNextLastValue);
                Assert.AreEqual(0, observer.OnCompletedCount);
                Assert.AreEqual(0, observer.OnErrorCount);
                
                subject.OnError(new Exception("error"));
                Assert.AreEqual(1, observer.OnNextCount);
                Assert.AreEqual(10, observer.OnNextLastValue);
                Assert.AreEqual(0, observer.OnCompletedCount);
                Assert.AreEqual(1, observer.OnErrorCount);
                Assert.AreEqual("error", observer.OnErrorLastValue.Message);
            }
        }
    }
}