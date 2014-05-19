GameOfLife
==========


I have extended the implementation mentioned at http://code.msdn.microsoft.com/Samples-for-Parallel-b4b76364 to use the strategy design pattern. This uses a concurrent queue to implement an object pool. Object pools improve application performance in situations where you require multiple instances of a class and the class is expensive to create or destroy. When a client program requests a new object, the object pool first attempts to provide one that has already been created and returned to the pool. If none is available, only then is a new object created. The concurrent bag inherits from the IProducerConsumerCollection which defines methods to manipulate thread-safe collections intended for producer/consumer usage. The current implementation uses .NET Framework 4 support for parallel programming using System.Threading.Tasks.Parallel class, which includes parallel versions of For and ForEach loops.
