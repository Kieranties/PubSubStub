using PubSubStub.Collections.Generic;
using PubSubStub.Interfaces;
using System;

namespace PubSubStub
{
    /// <summary>
    /// A generic publisher implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Publisher<T>: IPublisher<T>
    {
        /// <summary>
        /// The collection of subscribers this publisher pushes messsages to
        /// </summary>
        protected readonly ConcurrentCollection<IObserver<T>> subscribers = new ConcurrentCollection<IObserver<T>>();

        /// <summary>
        /// Occurs when the publisher has completed.
        /// </summary>
        public event EventHandler Complete;

        /// <summary>
        /// Called when the publisher has completed it's tasks.
        /// </summary>
        protected virtual void OnComplete()
        {
            var handler = Complete;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Subscribes the specified subscriber to publish events.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns>An IDisposable implementation to allow subscribers to unsubscribe from events</returns>
        public virtual IDisposable Subscribe(IObserver<T> subscriber)
        {
            if(subscriber == null)
                throw new ArgumentNullException("subscriber", "Cannot subscribe a null subscriber");

            if (subscribers.Contains(subscriber))
                return null;

            subscribers.Add(subscriber);
            return new Unsubscriber<IObserver<T>>(subscriber, subscribers);
        }

        /// <summary>
        /// Publishes the specified data to the subscriber collection.
        /// </summary>
        /// <param name="data">The data.</param>
        public virtual void Publish(T data)
        {
            foreach (var subscriber in subscribers)
                subscriber.OnNext(data);            
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// Triggers IObservable.OnCompleted for all subscribers
        /// </summary>
        public virtual void Dispose()
        {
            foreach (var subscriber in subscribers)
                subscriber.OnCompleted();

            OnComplete();
        }
    }
}
