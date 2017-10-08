using System;
using System.Threading;
using System.Threading.Tasks;

namespace Unict
{
    class E03_TaskState
    {
        public static void Run()
        {
            // use an Action delegate and a named method
            Task task1 = new Task(new Action<object>(printMessage),
            "First task");
            // use a anonymous delegate
            Task task2 = new Task(delegate (object obj) {
                printMessage(obj);
            }, "Second Task");
            // use a lambda expression and a named method
            // note that parameters to a lambda don’t need
            // to be quoted if there is only one parameter
            Task task3 = new Task((obj) => printMessage(obj), "Third task");
            // use a lambda expression and an anonymous method
            Task task4 = new Task((obj) => {
                printMessage(obj);
            }, "Fourth task");
            task1.Start();
            task2.Start();
            task3.Start();
            task4.Start();

            Task.WaitAll(task1,task2,task3,task4);

            /*Console.WriteLine("Message: {0}", task4.Status);

            task4.Start(); 
            Mostra le eccezioni sollevate nel momento in cui andiamo a startare un task già eseguito
             */
        }
        static void printMessage(object message) 
            //Questo oggetto lo si passa nel momento in cui viene creato il task
            //Questo mi serve ad avere task con lo stesso workload ma parametrizzato
        {
            Console.WriteLine("Message: {0}", message);
        }


    }
}
