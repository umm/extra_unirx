using System;
using ExtraUniRx;
using NUnit.Framework;
using UniRx;

namespace EditTest.ExtraUniRx
{
    public class TenseSubjectTest
    {
        [Test]
        public void SubjectTest()
        {
            var tenseSubject = new TenseSubject();
            var tenseObserver = new TestObserver<Tense>();
            var testObserver = new TestObserver<Unit>();

            tenseSubject.Subscribe(tenseObserver);
            tenseSubject.WhenDo().Subscribe(testObserver);
            tenseSubject.WhenWill().Subscribe(testObserver);
            tenseSubject.WhenDid().Subscribe(testObserver);

            tenseSubject.Do();

            Assert.AreEqual(Tense.Do, tenseObserver.OnNextLastValue);
            Assert.AreEqual(1, testObserver.OnNextCount);

            tenseSubject.Will();

            Assert.AreEqual(Tense.Will, tenseObserver.OnNextLastValue);
            Assert.AreEqual(2, testObserver.OnNextCount);

            tenseSubject.Did();

            Assert.AreEqual(Tense.Did, tenseObserver.OnNextLastValue);
            Assert.AreEqual(3, testObserver.OnNextCount);
        }

        [Test]
        public void ValuedSubjectTest()
        {
            var tenseSubject = new TenseSubject<int>();
            var tenseObserver = new TestObserver<Tuple<int, Tense>>();
            var testObserver = new TestObserver<int>();

            tenseSubject.Subscribe(tenseObserver);
            tenseSubject.WhenDo().Subscribe(testObserver);
            tenseSubject.WhenWill().Subscribe(testObserver);
            tenseSubject.WhenDid().Subscribe(testObserver);

            tenseSubject.Do(4);

            Assert.AreEqual(4, tenseObserver.OnNextLastValue.Item1);
            Assert.AreEqual(Tense.Do, tenseObserver.OnNextLastValue.Item2);
            Assert.AreEqual(1, testObserver.OnNextCount);
            Assert.AreEqual(4, testObserver.OnNextLastValue);

            tenseSubject.Will(5);

            Assert.AreEqual(5, tenseObserver.OnNextLastValue.Item1);
            Assert.AreEqual(Tense.Will, tenseObserver.OnNextLastValue.Item2);
            Assert.AreEqual(2, testObserver.OnNextCount);
            Assert.AreEqual(5, testObserver.OnNextLastValue);

            tenseSubject.Did(6);

            Assert.AreEqual(6, tenseObserver.OnNextLastValue.Item1);
            Assert.AreEqual(Tense.Did, tenseObserver.OnNextLastValue.Item2);
            Assert.AreEqual(3, testObserver.OnNextCount);
            Assert.AreEqual(6, testObserver.OnNextLastValue);
        }

        [Test]
        public void ExtensionTest()
        {
            var testObserver = new TestObserver<Unit>();
            var tenseObserver = new TestObserver<Tense>();
            var tenseSubject = new Subject<Tense>();
            var anonymousObservable = tenseSubject.AsObservable();
            var anonymousObserver = Observer.Create<Tense>(tenseObserver.OnNext, tenseObserver.OnError, tenseObserver.OnCompleted);

            anonymousObservable.WhenDo().Subscribe(testObserver);
            anonymousObservable.WhenWill().Subscribe(testObserver);
            anonymousObservable.WhenDid().Subscribe(testObserver);
            anonymousObserver.Do();
            anonymousObserver.Will();
            anonymousObserver.Did();
            tenseSubject.OnNext(Tense.Do);
            tenseSubject.OnNext(Tense.Will);
            tenseSubject.OnNext(Tense.Did);
            Assert.AreEqual(3, testObserver.OnNextCount);
            Assert.AreEqual(3, tenseObserver.OnNextCount);
        }
    }
}