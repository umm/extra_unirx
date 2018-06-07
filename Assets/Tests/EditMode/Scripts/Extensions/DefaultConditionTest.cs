using NUnit.Framework;
using UniRx;

namespace ExtraUniRx.Extensions
{
    public class DefaultConditionTest
    {
        private class Stub
        {
            public bool Value { get; set; }
        }

        [Test]
        public void BasicTest()
        {
            var intSubject = new Subject<int>();
            var classSubject = new Subject<Stub>();
            var stopper = new Subject<Unit>();
            intSubject.IsDefault().Buffer(stopper).Subscribe(x => Assert.AreEqual(3, x.Count));
            intSubject.IsNotDefault().Buffer(stopper).Subscribe(x => Assert.AreEqual(2, x.Count));
            classSubject.IsDefault().Buffer(stopper).Subscribe(x => Assert.AreEqual(2, x.Count));
            classSubject.IsNotDefault().Buffer(stopper).Subscribe(x => Assert.AreEqual(1, x.Count));

            intSubject.OnNext(0);
            intSubject.OnNext(1);
            intSubject.OnNext(0);
            intSubject.OnNext(1);
            intSubject.OnNext(0);
            classSubject.OnNext(null);
            classSubject.OnNext(new Stub());
            classSubject.OnNext(null);

            stopper.OnNext(Unit.Default);
        }

        [Test]
        public void SelectorTest()
        {
            var intSubject = new Subject<int>();
            var classSubject = new Subject<Stub>();
            var stopper = new Subject<Unit>();
            // IsDefault は「元が default」か「Selector を通した結果が Default」ならば値を流す
            intSubject.IsDefault(x => x * 10).Buffer(stopper).Subscribe(x => Assert.AreEqual(3, x.Count));
            intSubject.IsDefault(x => x - 1).Buffer(stopper).Subscribe(x => Assert.AreEqual(5, x.Count));
            intSubject.IsNotDefault(x => x * 10).Buffer(stopper).Subscribe(x => Assert.AreEqual(2, x.Count));
            classSubject.IsDefault(x => x.Value).Buffer(stopper).Subscribe(x => Assert.AreEqual(2, x.Count));
            classSubject.IsNotDefault(x => x.Value).Buffer(stopper).Subscribe(x => Assert.AreEqual(1, x.Count));

            intSubject.OnNext(0);
            intSubject.OnNext(1);
            intSubject.OnNext(0);
            intSubject.OnNext(1);
            intSubject.OnNext(0);
            classSubject.OnNext(null);
            classSubject.OnNext(new Stub());
            classSubject.OnNext(new Stub {Value = true});

            stopper.OnNext(Unit.Default);
        }
    }
}