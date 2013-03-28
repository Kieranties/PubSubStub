
namespace PubSubStub.Interfaces
{
    /// <summary>
    /// Implements methods to return instances of running publishers
    /// </summary>
    public interface IPublisherFactory
    {
        /// <summary>
        /// Returns the active publisher instance for type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IPublisher<T> Resolve<T>();

        /// <summary>
        /// Registers a new publisher instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>True if publisher is successfully registered, otherwise false</returns>
        bool Register<T>(IPublisher<T> publisher);
    }
}
