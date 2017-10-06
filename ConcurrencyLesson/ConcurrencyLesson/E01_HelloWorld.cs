using System;
using System.Threading;
using System.Threading.Tasks;

namespace Unict
{
    class E01_HelloWorld
    {
        public static void Run()
        {
            Task t= Task.Factory.StartNew(() => //Con questa riga di codice riusciamo a creare il task e a runnarlo subito
            {
                Console.WriteLine("Hello World");
            });
            Console.WriteLine(" Status = {0}",t.Status);
            Thread.Sleep(1000); //Mettendo quest'istruzione abbiamo bloccato il thread principale
            //In questo modo siamo sicuri che il task HelloWorld parta prima della scritta del main program

        }
    }
}
