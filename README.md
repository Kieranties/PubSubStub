PubSubStub
==========

A tiny .NET publish/subscribe library with a lot of flexibility based on the [Observer Pattern]

Aims
----

An excuse to think about design patterns, threading, and concurrency. This is a simple project built around 
the [IObserver<T>] and [IObservable<T>] interfaces introduced in .NET 4.

Implementation
--------------

The library introduces two key interfaces: `IPublisher<T>` and `ISubscriber<T>`.  Simple default implementations of
each are provide which can be used directly or as base classes for custom implementation.

An additional `IPublisherFactory` interface and concrete class are provided for simple management of publisher instances.  Any IOC container will
likely out-perform this class.

The base implementation classes handle all unsubscription duties required when a subscriber or publisher has completed its tasks.

-----

[@Kieranties] / [License] / [Source]


[@Kieranties]: http://twitter.com/kieranties
[License]: http://kieranties.mit-license.org/
[Source]: http://github.com/kieranties/pubsubstub
[Observer Pattern]: http://msdn.microsoft.com/en-us/library/ee817669.aspx
[IObserver<T>]: http://msdn.microsoft.com/en-gb/library/dd783449.aspx
[IObservable<T>]: http://msdn.microsoft.com/en-gb/library/dd990377.aspx