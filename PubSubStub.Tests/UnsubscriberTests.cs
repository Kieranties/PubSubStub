using NUnit.Framework;
using System.Collections.ObjectModel;

namespace PubSubStub.Tests
{
    [TestFixture]
    public class UnsubscriberTests
    {

        #region Constructor
        [Test]
        public void Constructor_WithNullValues_ReturnsUnsubscriberInstance()
        {
            var nullInstance = new Unsubscriber<object>(null, new Collection<object>());
            var nullCollection = new Unsubscriber<object>(new object(), null);
            var nullInstanceAndCollection = new Unsubscriber<object>(null, null);

            Assert.IsNotNull(nullInstance);
            Assert.IsNotNull(nullCollection);
            Assert.IsNotNull(nullInstanceAndCollection);
        }

        [Test]
        public void Constructor_WithValidValues_ReturnsUnsubscriberInstance()
        {
            var obj = new object();
            var collection = new Collection<object> {obj};
            
            var unsubscriber = new Unsubscriber<object>(obj, collection);
            
            Assert.IsNotNull(unsubscriber);
        }

        [Test]
        public void Constructor_WithCollectionMissingInstance_ReturnsUnsubscriberInstance()
        {
            var unsubscriber = new Unsubscriber<object>(new object(), new Collection<object>());

            Assert.IsNotNull(unsubscriber);
        }
        #endregion

        #region Dispose
        [Test]
        public void Dispose_WithNullConstructorParameters_DoesNotThrow()
        {
            var nullInstance = new Unsubscriber<object>(null, new Collection<object>());
            var nullCollection = new Unsubscriber<object>(new object(), null);
            var nullInstanceAndCollection = new Unsubscriber<object>(null, null);

            Assert.DoesNotThrow(nullInstance.Dispose);
            Assert.DoesNotThrow(nullCollection.Dispose);
            Assert.DoesNotThrow(nullInstanceAndCollection.Dispose);
        }

        [Test]
        public void Dispose_WithCollectionMissingInstance_DoesNotThrow()
        {
            var unsubscriber = new Unsubscriber<object>(new object(), new Collection<object>());

            Assert.DoesNotThrow(unsubscriber.Dispose);
        }

        [Test]
        public void Dispose_WithCollectionContainingInstance_RemovesInstanceFromCollection()
        {
            var obj = new object();
            var collection = new Collection<object> {obj};

            var unsubscriber = new Unsubscriber<object>(obj, collection);
            unsubscriber.Dispose();

            Assert.IsEmpty(collection);
        }
        #endregion
    }
}
