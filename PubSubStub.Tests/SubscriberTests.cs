using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax;
using NUnit.Framework;
using PubSubStub.Interfaces;

namespace PubSubStub.Tests
{
    [TestFixture]
    class SubscriberTests
    {
        private IPublisher<object> _publisher;
        private Subscriber<object> _subscriber;

        [SetUp]
        public void Setup()
        {
            _publisher = A.Fake<IPublisher<object>>();
            _subscriber = new Subscriber<object>();
        }

        #region Subscribe
        [Test]
        public void Subscribe_ToNullPublisher_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _subscriber.Subscribe(null));
        }

        [Test]
        public void Subscribe_ToPublisher_GetsDisposer()
        {
            _publisher.Configure()
                      .CallsTo(x => x.Subscribe(A<IObserver<object>>.Ignored))
                      .Returns(A.Fake<IDisposable>());

            _subscriber.Subscribe(_publisher);

            A.CallTo(() => _publisher.Subscribe(_subscriber)).MustHaveHappened();
        }

        [Test]
        public void Subscribe_ToPublisherWithExistingSubscription_CallsExistingDisposer()
        {
            var disposer = A.Fake<IDisposable>();
            _publisher.Configure()
                      .CallsTo(x => x.Subscribe(A<IObserver<object>>.Ignored))
                      .Returns(disposer);

            _subscriber.Subscribe(_publisher);
            _subscriber.Subscribe(_publisher);

            A.CallTo(() => disposer.Dispose()).MustHaveHappened(Repeated.Exactly.Once);
        }
        #endregion

        #region Unsubscribe
        [Test]
        public void Unsubscribe_WithNoSubscription_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _subscriber.Unsubscribe());
        }

        [Test]
        public void Unsubscribe_WithSubscription_CallsDisposer()
        {
            var disposer = A.Fake<IDisposable>();
            _publisher.Configure()
                      .CallsTo(x => x.Subscribe(_subscriber))
                      .Returns(disposer);

            _subscriber.Subscribe(_publisher);
            _subscriber.Unsubscribe();

            A.CallTo(() => disposer.Dispose()).MustHaveHappened(Repeated.Exactly.Once);

        }
        #endregion

        #region OnCompleted
        [Test]
        public void OnCompleted_WithNullSubscription_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _subscriber.OnCompleted());
        }

        [Test]
        public void OnCompleted_WithSubscription_CallsDisposer()
        {
            var disposer = A.Fake<IDisposable>();
            _publisher.Configure()
                      .CallsTo(x => x.Subscribe(_subscriber))
                      .Returns(disposer);

            _subscriber.Subscribe(_publisher);
            _subscriber.Unsubscribe();

            A.CallTo(() => disposer.Dispose()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void OnCompleted_WithSingleRegisteredCompleteHandler_FiresHandler()
        {
            var completeHandler = A.Fake<Action>();
            _subscriber.BindOnCompleted(completeHandler);

            _subscriber.OnCompleted();

            A.CallTo(() => completeHandler.Invoke()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void OnCompleted_WithMultipleRegisteredCompleteHandler_FiresHandlers()
        {
            var handler1 = A.Fake<Action>();
            var handler2 = A.Fake<Action>();

            _subscriber.BindOnCompleted(handler1);
            _subscriber.BindOnCompleted(handler2);
            _subscriber.OnCompleted();

            A.CallTo(() => handler1.Invoke()).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => handler2.Invoke()).MustHaveHappened(Repeated.Exactly.Once);
        }
        #endregion

        #region OnNext
        [Test]
        public void OnNext_WithNullSubscriber_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _subscriber.OnNext(new object()));
        }

        [Test]
        public void OnNext_WithNullData_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _subscriber.OnNext(null));
        }

        [Test]
        public void OnNext_WithSingleRegisteredNextHandler_FiresHandler()
        {
            var nextHandler = A.Fake<Action<object>>();
            var nextObject = new object();

            _subscriber.BindOnNext(nextHandler);
            _subscriber.OnNext(nextObject);

            A.CallTo(() => nextHandler.Invoke(nextObject)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void OnNext_WithMultipleRegisteredNextHandler_FiresHandlers()
        {
            var handler1 = A.Fake<Action<object>>();
            var handler2 = A.Fake<Action<object>>();
            var nextObject = new object();

            _subscriber.BindOnNext(handler1);
            _subscriber.BindOnNext(handler2);
            _subscriber.OnNext(nextObject);

            A.CallTo(() => handler1.Invoke(nextObject)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => handler2.Invoke(nextObject)).MustHaveHappened(Repeated.Exactly.Once);
        }
        #endregion

        #region OnError
        [Test]
        public void OnError_WithNullSubscriber_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _subscriber.OnError(new Exception()));
        }

        [Test]
        public void OnError_WithNullData_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _subscriber.OnError(null));
        }

        [Test]
        public void OnError_WithSingleRegisteredErrorHandler_FiresHandler()
        {
            var errorHandler = A.Fake<Action<Exception>>();
            var errorObject = new Exception();

            _subscriber.BindOnError(errorHandler);
            _subscriber.OnError(errorObject);

            A.CallTo(() => errorHandler.Invoke(errorObject)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void OnError_WithMultipleRegisteredErrorHandler_FiresHandlers()
        {
            var handler1 = A.Fake<Action<Exception>>();
            var handler2 = A.Fake<Action<Exception>>();
            var nextObject = new Exception();

            _subscriber.BindOnError(handler1);
            _subscriber.BindOnError(handler2);
            _subscriber.OnError(nextObject);

            A.CallTo(() => handler1.Invoke(nextObject)).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => handler2.Invoke(nextObject)).MustHaveHappened(Repeated.Exactly.Once);
        }
        #endregion

        #region Dispose
        [Test]
        public void Dispose_WithNullSubscriber_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _subscriber.Dispose());
        }

        [Test]
        public void Dispose_WithSubscriber_CallsDispoer()
        {
            var disposer = A.Fake<IDisposable>();
            _publisher.Configure()
                             .CallsTo(x => x.Subscribe(_subscriber))
                             .Returns(disposer);

            _subscriber.Subscribe(_publisher);
            _subscriber.Dispose();

            A.CallTo(() => disposer.Dispose()).MustHaveHappened(Repeated.Exactly.Once);
        }
        #endregion
    }
}
