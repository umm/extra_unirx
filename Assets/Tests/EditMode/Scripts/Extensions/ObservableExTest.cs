using ExtraUniRx;
using NUnit.Framework;

namespace EditTest.ExtraUniRx
{
    public class ObservableExTest
    {
        [Test]
        public void ReturnsTest()
        {
            {
                var observer = new TestObserver<int>();
                ObservableEx.Returns(new int[] {1, 2, 3}).Subscribe(observer);
                Assert.AreEqual(3, observer.OnNextCount);
                Assert.AreEqual(new[] {1, 2, 3}, observer.OnNextValues);
                Assert.AreEqual(1, observer.OnCompletedCount);
                Assert.AreEqual(0, observer.OnErrorCount);
            }

            {
                var observer = new TestObserver<int>();
                ObservableEx.Returns(new int[0]).Subscribe(observer);
                Assert.AreEqual(0, observer.OnNextCount);
                Assert.AreEqual(1, observer.OnCompletedCount);
                Assert.AreEqual(0, observer.OnErrorCount);
            }
        }
    }
}