# async-breakfast
A dable in the .net async/await patern

I put this together for a presentation. Here are my notes from the slides:
## Async/Await Pattern
* Responsiveness or Scaling
* Most useful for slow external resources
* Tasks are not Threads
* Cancel instead of abort
* Http request/response remains synchronous 

### notes
There are two primary benefits I see to asynchrony: scalability and offloading (e.g. responsiveness, parallelism).
Async and await in ASP.NET are all about I/O. Most useful for slow external resources. Like reading and writing files, database records, and REST APIs. However, they’re not good for CPU-bound tasks.

In terms of responsiveness:
for GUI apps this prevents your UI from being stuck while things happen in the background. Think winforms or mobile app. DO NOT CONFUSE THIS WITH A WEB UI. 

In terms of scalability:
ASP.net: imagine 2 threads in your app pool with 3 incoming requests. Async/await will return a thread to the pool once it has handed off the external call so that the 3 request can start processing.  In this example we are achieving “scalability”. Asynchronous code does not replace the thread pool. It uses the thread pool more efficiently.

Tasks do not spool up threads! Tasks are pieces of code that need to be run using the available threads. There is a task scheduler in the background that passes tasks off to the available threads. 

As such tasks are not aborted like threads are, instead they are cancelled by passing in a cancellation token. In your long running code you can then check the status of the cancellation token and throw exception

## Pitfalls to avoid
* Async-over-sync
* Async void
* Mixing sync and async

### notes
Async-over-sync: Async over sync refers to wrapping your synchronous code in a Task.Run or something similar. You are offloading work on one thread  to another thread. If you are doing it for scalability reasons, then rather re-implement your code in an async specific manner. If your doing it for responsiveness reasons then maybe it’s ok as you are swapping out a valuable resource for a less valuable one (think UI thread vs thread pool thread). It’s not always bad but it depends on the reason you are doing it for. Conisder who will be using your code and how  much will they know about it.

Async void: these methods don’t return a Task. Use sparingly and only for truly fire and forget scenarios as it is hard to tell when they have completed. Exceptions don’t show up in the calling code, but in the synchronization context/global unhandled exception handler only.  

Mixing sync and async: If you choose to do async, it is better if the entire stack of that one async call becomes async.  In .net framework (4.5), calling blocking code from non-blocking code can result in deadlocks. The sync.context allows only one chunk of code at a time. When async completes the sync.context tries to run the rest of the code in the chunk. If you call sync method on async method, the sync.context already has a thread running when it tries to step back in to the original async code. Deadlock. You won’t see this behaviour in .net core as there is no sync.context, but it would render your async work pointless. Also if you run out of threads in your thread pool you will see a similar result to a deadlock

