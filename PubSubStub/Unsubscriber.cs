using System;
using System.Collections.Generic;

namespace PubSubStub
{
    /// <summary>
    /// A generic, disposable class to allow instances of T to be removed from
    /// instances of TCollection
    /// </summary>
    /// <typeparam name="T">The instance type of subscriber.</typeparam>
    public class Unsubscriber<T> : IDisposable  
    {
        /// <summary>
        /// The subscriber collection
        /// </summary>
        protected readonly ICollection<T> subscribers;

        /// <summary>
        /// The subscriber instance
        /// </summary>
        protected readonly T subscriber;

        /// <summary>
        /// Initializes a new instance of the <see cref="Unsubscriber{T}"/> class.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="subscribers">The subscribers.</param>
        public Unsubscriber(T subscriber, ICollection<T> subscribers)
        {
            this.subscribers = subscribers;
            this.subscriber = subscriber;            
        }

        /// <summary>
        /// Removes the subscriber from the subscriber collection
        /// </summary>
        public virtual void Dispose()
        {
            if (subscriber != null && subscribers != null
                && subscribers.Contains(subscriber))
            {
                subscribers.Remove(subscriber);
            }
        }
    }
}
