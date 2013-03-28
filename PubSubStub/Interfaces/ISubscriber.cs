using System;

namespace PubSubStub.Interfaces
{
    /// <summary>
    /// Allows an object to represent it self as a subscriber of T information.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISubscriber<T>: IObserver<T>
    {
        /// <summary>
        /// Subscribes to the specified publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        void Subscribe(IPublisher<T> publisher);
    }
}
