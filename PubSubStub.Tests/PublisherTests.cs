using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax;
using NUnit.Framework;

namespace PubSubStub.Tests
{
    [TestFixture]
    class PublisherTests
    {
        private Publisher<object> _publisher;

        [SetUp]
        public void Setup()
        {
            _publisher = new Publisher<object>();
        }

        #region Subscribe
        [Test]
        public void Subscribe_NullSubscriber_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _publisher.Subscribe(null));
        }

        [Test]
        public void Subscribe_NewSubscriber_ReturnsIDisposable()
        {
            var subscriber = new Subscriber<object>();
         
            var disposer = _publisher.Subscribe(subscriber);
            
            Assert.IsNotNull(disposer);
        }

        [Test]
        public void Subscribe_SameSubscriber_ReturnsNull()
        {
            var subscriber = new Subscriber<object>();

            _publisher.Subscribe(subscriber);
            var nullDisposer = _publisher.Subscribe(subscriber);

            Assert.IsNull(nullDisposer);
        }
        #endregion

        #region Publish
        [Test]
        public void Publish_WithNoSubscribers_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _publisher.Publish(new object()));
        }

        [Test]
        public void Publish_WithSingleSubscriber_SubscriberReceivesOnNext()
        {
            var subscriber = A.Fake<IObserver<object>>();
            var obj = new object();

            _publisher.Subscribe(subscriber);
            _publisher.Publish(obj);

            A.CallTo(() => subscriber.OnNext(obj)).MustHaveHappened();
        }

        [Test]
        public void Publish_WithMultipleSubscribers_SubscribersReceiveOnNext()
        {
            var subscriber1 = A.Fake<IObserver<object>>();
            var subscriber2 = A.Fake<IObserver<object>>();
            var subscriber3 = A.Fake<IObserver<object>>();
            var obj = new object();

            _publisher.Subscribe(subscriber1);
            _publisher.Subscribe(subscriber2);
            _publisher.Subscribe(subscriber3);
            _publisher.Publish(obj);

            A.CallTo(() => subscriber1.OnNext(obj)).MustHaveHappened();
            A.CallTo(() => subscriber2.OnNext(obj)).MustHaveHappened();
            A.CallTo(() => subscriber3.OnNext(obj)).MustHaveHappened();
        }
        #endregion

        #region Dispose
        [Test]
        public void Dispose_WithNoSubscribers_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _publisher.Dispose());
        }

        [Test]
        public void Dispose_WithSingleSubscriber_SubscriberReceivesOnCompleted()
        {
            var subscriber = A.Fake<IObserver<object>>();

            _publisher.Subscribe(subscriber);
            _publisher.Dispose();

            A.CallTo(() => subscriber.OnCompleted()).MustHaveHappened();
        }

        [Test]
        public void Dispose_WithMultipleSubscribers_SubscribersReceiveOnCompleted()
        {
            var subscriber1 = A.Fake<IObserver<object>>();
            var subscriber2 = A.Fake<IObserver<object>>();
            var subscriber3 = A.Fake<IObserver<object>>();

            _publisher.Subscribe(subscriber1);
            _publisher.Subscribe(subscriber2);
            _publisher.Subscribe(subscriber3);
            _publisher.Dispose();

            A.CallTo(() => subscriber1.OnCompleted()).MustHaveHappened();
            A.CallTo(() => subscriber2.OnCompleted()).MustHaveHappened();
            A.CallTo(() => subscriber3.OnCompleted()).MustHaveHappened();
        }

        [Test]
        public void Dispose_WhenCalled_RaisesCompleteEvent()
        {
            var handlerFired = false;
            _publisher.Complete += (sender, args) => handlerFired = true;
            _publisher.Dispose();

            Assert.IsTrue(handlerFired);
            
        }
        #endregion
    }
}
