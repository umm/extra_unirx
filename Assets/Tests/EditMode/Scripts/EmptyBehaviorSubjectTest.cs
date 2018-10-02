using System;
using ExtraUniRx;
using NUnit.Framework;

namespace EditTest.ExtraUniRx
{
    public class EmptyBehaviorSubjectTest
    {
        [Test]
        public void SubscribeTest()
        {
            var subject = new EmptyBehaviorSubject<int>();
            var observer1 = new TestObserver<int>();
            var observer2 = new TestObserver<int>();

            var disposable = subject.Subscribe(observer1);
            Assert.AreEqual(0, observer1.OnNextCount);
            Assert.AreEqual(0, observer2.OnNextCount);

            subject.OnNext(1);
            Assert.AreEqual(1, observer1.OnNextCount);
            Assert.AreEqual(1, observer1.OnNextLastValue);
            Assert.AreEqual(0, observer2.OnNextCount);

            subject.OnNext(2);
            Assert.AreEqual(2, observer1.OnNextCount);
            Assert.AreEqual(2, observer1.OnNextLastValue);
            Assert.AreEqual(0, observer2.OnNextCount);

            subject.Subscribe(observer2);
            Assert.AreEqual(2, observer1.OnNextCount);
            Assert.AreEqual(2, observer1.OnNextLastValue);
            Assert.AreEqual(1, observer2.OnNextCount);
            Assert.AreEqual(2, observer2.OnNextLastValue);

            disposable.Dispose();

            subject.OnNext(1);
            Assert.AreEqual(2, observer1.OnNextCount);
            Assert.AreEqual(1, observer2.OnNextCount);

            subject.OnError(new NotImplementedException());
            Assert.AreEqual(2, observer1.OnNextCount);
            Assert.AreEqual(1, observer2.OnNextCount);
            Assert.AreEqual(0, observer1.OnErrorCount);
            Assert.AreEqual(0, observer2.OnErrorCount);

            subject.OnCompleted();
            Assert.AreEqual(2, observer1.OnNextCount);
            Assert.AreEqual(1, observer2.OnNextCount);
            Assert.AreEqual(0, observer1.OnErrorCount);
            Assert.AreEqual(0, observer2.OnErrorCount);
            Assert.AreEqual(0, observer1.OnCompletedCount);
            Assert.AreEqual(0, observer2.OnCompletedCount);
        }
    }
}