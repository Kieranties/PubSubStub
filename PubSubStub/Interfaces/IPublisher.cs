using System;

namespace PubSubStub.Interfaces
{
    /// <summary>
    /// Allows an object to represent itself as a publisher of type T information.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPublisher<T>: IObservable<T>, IDisposable, IPublisher
    {
        /// <summary>
        /// Occurs when the publisher has completed.
        /// </summary>
        event EventHandler Complete;

        /// <summary>
        /// Publishes the specified data to subscribers of this publisher.
        /// </summary>
        /// <param name="data">The data.</param>
        void Publish(T data);
    }

    /// <summary>
    /// Allows instances of IPublisher&lt;T&gt; to be cast to a simpler type
    /// </summary>
    public interface IPublisher { }
}
