using PubSubStub.Interfaces;
using System;

namespace PubSubStub
{
    /// <summary>
    /// A generic subscriber implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Subscriber<T>: ISubscriber<T>, IDisposable
    {
        /// <summary>
        /// The object used to dispose/unsubscribe from a publisher
        /// </summary>
        protected IDisposable disposer;

        /// <summary>
        /// The methods to process when an error occurs
        /// </summary>
        protected Action<Exception> errorHandler;

        /// <summary>
        /// The methods to process when data is received from the publisher
        /// </summary>
        protected Action<T> nextHandler;

        /// <summary>
        /// The method to process when the publishers has completed it's tasks
        /// </summary>
        protected Action completeHandler;

        /// <summary>
        /// Subscribes to the specified publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        public virtual void Subscribe(IPublisher<T> publisher)
        {
            if (disposer != null)
                disposer.Dispose();

            disposer = publisher.Subscribe(this);
        }

        /// <summary>
        /// Unsubscribes this instance from its registered publisher.
        /// </summary>
        public virtual void Unsubscribe()
        {
            if (disposer != null)
                disposer.Dispose();
        }

        /// <summary>
        /// Called when the registered publisher has completed.
        /// </summary>
        public virtual void OnCompleted()
        {
            if (completeHandler != null)
                completeHandler.Invoke();

            Unsubscribe();
        }

        /// <summary>
        /// Called when the registered publisher has data to provide to this subscriber.
        /// </summary>
        /// <param name="data">The data.</param>
        public virtual void OnNext(T data)
        {
            if (nextHandler != null)
            {
                nextHandler.Invoke(data);
            }
        }

        /// <summary>
        /// Called when the registered publisher reports an error.
        /// </summary>
        /// <param name="error">The error.</param>
        public virtual void OnError(Exception error)
        {
            if (errorHandler != null)
            {
                errorHandler.Invoke(error);
            }
        }

        /// <summary>
        /// Binds an action to be executed when this subscriber receives data from the registered publisher.
        /// </summary>
        /// <param name="handler">The next handler.</param>
        public virtual void BindOnNext(Action<T> handler)
        {
            nextHandler += handler;
        }

        /// <summary>
        /// Binds an action to be executed when this subscriber receives an error from the registered publisher.
        /// </summary>
        /// <param name="handler">The error handler.</param>
        public virtual void BindOnError(Action<Exception> handler)
        {
            errorHandler += handler;
        }

        /// <summary>
        /// Binds an action to be executed when this subscriber receives notice of the registered publisher being completed.
        /// </summary>
        /// <param name="handler">The complete handler.</param>
        public virtual void BindOnCompleted(Action handler)
        {
            completeHandler += handler;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// Ensures this subscriber unsubscribes from a publisher.
        /// </summary>
        public virtual void Dispose()
        {
            Unsubscribe();
        }
    }
}
