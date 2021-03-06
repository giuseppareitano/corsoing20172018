﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Unict
{
    class E05_TaskResult
    {
        public static void Run()
        {
            // create the task
            Task<int> task1 = new Task<int>(() =>
            {
                int sum = 0;
                for (int i = 0; i < 100; i++)
                {
                    sum += i;
                }
                return sum;
            });
            // start the task
            task1.Start();

           /* task1.Wait(); //Il main thread si blocca finchè il thread non completa
            //Modo alternativo
            */
            // write out the result
            Console.WriteLine("Result 1: {0}", task1.Result); 
            //Quando chiamiamo result il main thread si blocca in attesa che il thread produca il risultato
            
            
            // create the task using state
            Task<int> task2 = new Task<int>(obj =>
            {
                int sum = 0;
                int max = (int)obj;
                for (int i = 0; i < max; i++)
                {
                    sum += i;
                }
                return sum;
            }, 100);
            // start the task
            task2.Start();
            // write out the result
            Console.WriteLine("Result 2: {0}", task2.Result);


        }
    }
}
