using System;
using FakeItEasy;
using NUnit.Framework;
using PubSubStub.Interfaces;

namespace PubSubStub.Tests
{
    [TestFixture]
    public class PublisherFactoryTests
    {
        private PublisherFactory _factory;
        
        [SetUp]
        public void Setup()
        {
            _factory = new PublisherFactory();

        }

        #region Register
        [Test]
        public void Register_UnregisteredType_Success()
        {
            var publisher = A.Fake<IPublisher<object>>();
         
            var result = _factory.Register(publisher);
            
            Assert.IsTrue(result);
        }

        [Test]
        public void Register_SinglePublisherMultipleTimes_Fails()
        {
            var publisher = A.Fake<IPublisher<object>>();
         
            _factory.Register(publisher);
            var result = _factory.Register(publisher);

            Assert.IsFalse(result);
        }

        [Test]
        public void Register_SamePublisherTypeMultipleTimes_Fails()
        {
            var publisher = A.Fake<IPublisher<object>>();
            var publisher2 = A.Fake<IPublisher<object>>();

            _factory.Register(publisher);
            var result = _factory.Register(publisher2);

            Assert.IsFalse(result);
        }

        [Test]
        public void Register_DifferentPublisherTypes_Success()
        {
            var objPublisher = A.Fake<IPublisher<object>>();
            var stringPublisher = A.Fake<IPublisher<string>>();
            var intPublisher = A.Fake<IPublisher<int>>();

            var objResult = _factory.Register(objPublisher);
            var stringResult = _factory.Register(stringPublisher);
            var intResult = _factory.Register(intPublisher);

            Assert.IsTrue(objResult);
            Assert.IsTrue(stringResult);
            Assert.IsTrue(intResult);
        }

        [Test]
        public void Register_OnSuccessRegisterRemovalAction_RemovesOnComplete()
        {
            var publisher = A.Fake<IPublisher<object>>();
            var regResult = _factory.Register(publisher);
            
            // Act: raise the publishers complete event
            publisher.Complete += Raise.WithEmpty().Now;

            // publisher is registered
            Assert.IsTrue(regResult);
            // publisher no longer registered
            var resolved = _factory.Resolve<object>();
            Assert.IsNull(resolved);
        }

        #endregion

        #region Resolve
        [Test]
        public void Resolve_WhenNoRegisteredPublishers_ReturnsNull()
        {
            Assert.IsNull(_factory.Resolve<object>());
        }

        [Test]
        public void Resolve_WhenSingleRegisteredPublisher_ReturnsPublisher()
        {
            var publisher = A.Fake<IPublisher<object>>();
            
            _factory.Register(publisher);
            var resolvedPublisher = _factory.Resolve<object>();

            Assert.IsNotNull(resolvedPublisher);
            Assert.AreSame(resolvedPublisher, publisher);
        }

        [Test]
        public void Resolve_WhenMultipleRegisteredPublishers_ReturnsPublishers()
        {
            var objPublisher = A.Fake<IPublisher<object>>();
            var stringPublisher = A.Fake<IPublisher<string>>();
            var intPublisher = A.Fake<IPublisher<int>>();

            _factory.Register(objPublisher);
            _factory.Register(stringPublisher);
            _factory.Register(intPublisher);
            var resolvedObjectPublisher = _factory.Resolve<object>();
            var resolvedStringPublisher = _factory.Resolve<string>();
            var resolvedIntPublisher = _factory.Resolve<int>();

            Assert.IsNotNull(resolvedObjectPublisher);
            Assert.IsNotNull(resolvedStringPublisher);
            Assert.IsNotNull(resolvedIntPublisher);
            Assert.AreSame(resolvedObjectPublisher, objPublisher);
            Assert.AreSame(resolvedStringPublisher, stringPublisher);
            Assert.AreSame(resolvedIntPublisher, intPublisher);
        }
        #endregion

    }
}
