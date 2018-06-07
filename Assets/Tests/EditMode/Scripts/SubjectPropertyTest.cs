﻿using NUnit.Framework;
using UniRx;

namespace ExtraUniRx
{
    public class SubjectPropertyTest
    {
        [Test]
        public void ObserverTest()
        {
            var property = new SubjectProperty<int>();
            Assert.AreEqual(0, property.Value);

            Observable.Return(1).Subscribe(property);
            Assert.AreEqual(1, property.Value);

            Observable.Return(2).Subscribe(property);
            Assert.AreEqual(2, property.Value);
        }

        [Test]
        public void ObservableTest()
        {
            var property = new SubjectProperty<int>();
            var observer = new TestObserver<int>();
            property.Subscribe(observer);
            
            Assert.AreEqual(0, observer.OnNextCount);
            property.Value = 10;
            
            Assert.AreEqual(1, observer.OnNextCount);
            Assert.AreEqual(10, observer.OnNextLastValue);
            
            property.Value = 20;
            Assert.AreEqual(2, observer.OnNextCount);
            Assert.AreEqual(20, observer.OnNextLastValue);

            var observer2 = new TestObserver<int>();
            property.Subscribe(observer2);
            Assert.AreEqual(0, observer2.OnNextCount);
            
            property.Value = 30;
            Assert.AreEqual(1, observer2.OnNextCount);
            Assert.AreEqual(30, observer2.OnNextLastValue);
            Assert.AreEqual(3, observer.OnNextCount);
            Assert.AreEqual(30, observer.OnNextLastValue);
        }
    }
}