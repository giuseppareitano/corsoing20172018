using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Unict
{
    class E12_RaceConditionQueue
    {
        public static void Run()
        {
            // create a shared collection
            Queue<int> sharedQueue = new Queue<int>();
            
            // create a lock for queue
            ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();

            // populate the collection with items to process
            for (int i = 0; i < 1000; i++)
            {
                sharedQueue.Enqueue(i);
            }

            // define a counter for the number of processed items
            int itemCount = 0;

            // create tasks to process the list
            Task[] tasks = new Task[10];

            for (int i = 0; i < tasks.Length; i++)
            {
                // create the new task
                tasks[i] = new Task(() => {

                    //=====================================
                    // Race condition
                    //=====================================
                    while (true)
                    {
                        rwlock.EnterReadLock();
                        int queueCount = sharedQueue.Count;
                        rwlock.ExitReadLock();

                        if (queueCount == 0) break;

                        // take an item from the queue
                        try
                        {
                            //=====================================
                            // Race condition
                            //=====================================

                            rwlock.EnterWriteLock();
                            if (sharedQueue.Count > 0)
                            {
                                int item = sharedQueue.Dequeue();
                                // increment the count of items processed
                                Interlocked.Increment(ref itemCount);
                            }
                            rwlock.ExitWriteLock();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Task faulted {0}",e.Message);
                            break;
                        }
                    }
                });
                // start the new task
                tasks[i].Start();
            }
            // wait for the tasks to complete
            Task.WaitAll(tasks);
            // report on the number of items processed
            Console.WriteLine("Items processed: {0}", itemCount);
        }


    }
}
