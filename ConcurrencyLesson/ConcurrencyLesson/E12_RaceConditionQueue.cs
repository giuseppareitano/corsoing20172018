﻿using System;
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
                    ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();
                    //=====================================
                    // Race condition
                    //=====================================

                    while (true)
                    {
                        // take an item from the queue
                        rwlock.EnterReadLock();
                        int queueCount = sharedQueue.Count;
                        rwlock.ExitReadLock();
                        if (queueCount == 0) break;
                        
                        try
                        {
                            //=====================================
                            // Race condition
                            //=====================================
                            rwlock.EnterWriteLock();
                            int item = sharedQueue.Dequeue();
                            rwlock.ExitWriteLock();
                            // increment the count of items processed
                            Interlocked.Increment(ref itemCount);
                            
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Task faulted");
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
