using System;
using System.Collections.Generic;

namespace PubSubStub
{
    /// <summary>
    /// A generic, disposable class to allow instances of T to be removed from
    /// instances of K
    /// </summary>
    /// <typeparam name="T">The instance type of subscriber.</typeparam>
    /// <typeparam name="TCollection">The instance type collection.</typeparam>
    public class Unsubscriber<T, TCollection> : IDisposable  
        where TCollection: ICollection<T>
    {
        /// <summary>
        /// The subscriber collection
        /// </summary>
        protected readonly TCollection subscribers;

        /// <summary>
        /// The subscriber instance
        /// </summary>
        protected readonly T subscriber;

        /// <summary>
        /// Initializes a new instance of the <see cref="Unsubscriber{T, TCollection}"/> class.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="subscribers">The subscribers.</param>
        public Unsubscriber(T subscriber, TCollection subscribers)
        {
            this.subscribers = subscribers;
            this.subscriber = subscriber;            
        }

        /// <summary>
        /// Removes the subscriber from the subscriber collection
        /// </summary>
        public virtual void Dispose()
        {
            if (subscriber != null && null != subscribers
                && subscribers.Contains(subscriber))
            {
                subscribers.Remove(subscriber);
            }
        }
    }
}
