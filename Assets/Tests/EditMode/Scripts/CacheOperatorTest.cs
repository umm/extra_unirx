using NUnit.Framework;
using UniRx;

namespace ExtraUniRx
{
    public class CacheOperatorTest
    {
        [Test]
        public void CacheTest()
        {
            var subject = new Subject<Unit>();
            var observable = subject.Select(_ => 1).Scan((a, b) => a + b).Cache();

            var testObserver1 = new TestObserver<int>();
            var disposable1 = observable.Subscribe(testObserver1);

            Assert.AreEqual(0, testObserver1.OnNextCount);

            subject.OnNext(Unit.Default);

            Assert.AreEqual(1, testObserver1.OnNextCount);
            Assert.AreEqual(1, testObserver1.OnNextLastValue);

            subject.OnNext(Unit.Default);
            subject.OnNext(Unit.Default);

            Assert.AreEqual(3, testObserver1.OnNextCount);
            Assert.AreEqual(3, testObserver1.OnNextLastValue);

            disposable1.Dispose();

            var testObserver2 = new TestObserver<int>();
            var disposable2 = observable.Subscribe(testObserver2);

            Assert.AreEqual(1, testObserver2.OnNextCount);
            Assert.AreEqual(3, testObserver2.OnNextLastValue);

            subject.OnNext(Unit.Default);

            Assert.AreEqual(2, testObserver2.OnNextCount);
            Assert.AreEqual(4, testObserver2.OnNextLastValue);

            disposable2.Dispose();

            subject.OnNext(Unit.Default);
            subject.OnNext(Unit.Default);

            var testObserver3 = new TestObserver<int>();
            var disposable3 = observable.Subscribe(testObserver3);

            Assert.AreEqual(1, testObserver3.OnNextCount);
            Assert.AreEqual(6, testObserver3.OnNextLastValue);

            subject.OnNext(Unit.Default);

            Assert.AreEqual(2, testObserver3.OnNextCount);
            Assert.AreEqual(7, testObserver3.OnNextLastValue);

            disposable3.Dispose();

            subject.OnCompleted();

            var testObserver4 = new TestObserver<int>();
            var disposable4 = observable.Subscribe(testObserver4);

            Assert.AreEqual(1, testObserver4.OnNextCount);
            Assert.AreEqual(7, testObserver4.OnNextLastValue);

            subject.OnNext(Unit.Default);

            Assert.AreEqual(1, testObserver4.OnNextCount);
            Assert.AreEqual(7, testObserver4.OnNextLastValue);

            disposable4.Dispose();
        }

        [Test]
        public void CacheAllTest()
        {
            var subject = new Subject<Unit>();
            var observable = subject.Select(_ => 1).Scan((a, b) => a + b).CacheAll();

            var testObserver1 = new TestObserver<int>();
            var disposable1 = observable.Subscribe(testObserver1);

            Assert.AreEqual(0, testObserver1.OnNextCount);

            subject.OnNext(Unit.Default);

            Assert.AreEqual(1, testObserver1.OnNextCount);
            Assert.AreEqual(1, testObserver1.OnNextLastValue);

            subject.OnNext(Unit.Default);
            subject.OnNext(Unit.Default);

            Assert.AreEqual(3, testObserver1.OnNextCount);
            Assert.AreEqual(3, testObserver1.OnNextLastValue);

            disposable1.Dispose();

            var testObserver2 = new TestObserver<int>();
            var disposable2 = observable.Subscribe(testObserver2);

            Assert.AreEqual(3, testObserver2.OnNextCount);
            Assert.AreEqual(3, testObserver2.OnNextLastValue);

            subject.OnNext(Unit.Default);

            Assert.AreEqual(4, testObserver2.OnNextCount);
            Assert.AreEqual(4, testObserver2.OnNextLastValue);

            disposable2.Dispose();

            subject.OnNext(Unit.Default);
            subject.OnNext(Unit.Default);

            var testObserver3 = new TestObserver<int>();
            var disposable3 = observable.Subscribe(testObserver3);

            Assert.AreEqual(6, testObserver3.OnNextCount);
            Assert.AreEqual(6, testObserver3.OnNextLastValue);

            subject.OnNext(Unit.Default);

            Assert.AreEqual(7, testObserver3.OnNextCount);
            Assert.AreEqual(7, testObserver3.OnNextLastValue);

            disposable3.Dispose();

            subject.OnCompleted();

            var testObserver4 = new TestObserver<int>();
            var disposable4 = observable.Subscribe(testObserver4);

            Assert.AreEqual(7, testObserver4.OnNextCount);
            Assert.AreEqual(7, testObserver4.OnNextLastValue);

            subject.OnNext(Unit.Default);

            Assert.AreEqual(7, testObserver4.OnNextCount);
            Assert.AreEqual(7, testObserver4.OnNextLastValue);

            disposable4.Dispose();
        }
    }
}