using PubSubStub.Interfaces;
using System;
using System.Collections.Concurrent;

namespace PubSubStub
{
    /// <summary>
    /// Simple implementation of IPublisherFactory
    /// </summary>
    public class PublisherFactory: IPublisherFactory
    {
        /// <summary>
        /// Singleton instance access to factory
        /// </summary>
        public static readonly PublisherFactory Instance = new PublisherFactory();

        /// <summary>
        /// The instantiated publishers
        /// </summary>
        private static readonly ConcurrentDictionary<Type, IPublisher> _publishers = new ConcurrentDictionary<Type, IPublisher>();

        /// <summary>
        /// Prevents a default instance of the <see cref="PublisherFactory"/> class from being created.
        /// </summary>
        private PublisherFactory() { }

        /// <summary>
        /// Returns the active publisher instance for type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IPublisher<T> Resolve<T>()
        {
            IPublisher value;
            if (_publishers.TryGetValue(typeof (T), out value))
                return value as IPublisher<T>;

            return null;
        }

        /// <summary>
        /// Registers a new publisher instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="publisher"></param>
        /// <returns>
        /// True if publisher is successfully registered, otherwise false
        /// </returns>
        public bool Register<T>(IPublisher<T> publisher)
        {
            if (_publishers.TryAdd(typeof (T), publisher))
            {
                // allow the publisher to remove itself from the factory collection
                publisher.Complete += (sender, args) =>
                                        {
                                            IPublisher removed;
                                            _publishers.TryRemove(typeof(T), out removed);
                                        };
                return true;
            }
            return false;
        }
    }
}
