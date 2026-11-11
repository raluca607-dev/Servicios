namespace Ejercicio1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Thread suma = new Thread(incrementar);
            //Thread resta = new Thread(decrementar);
            Thread suma = new Thread(() =>
            {
                while (!terminado)
                {
                    lock (l)
                    {
                        if (!terminado)
                        {
                            cont++;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Hilo 1: "+cont);

                            if (cont >= 500)
                            {
                                terminado = true;
                            }
                        }
                    }
                }
            });

            Thread resta = new Thread(() =>
            {
                while (!terminado)
                {
                    lock (l)
                    {
                        if (!terminado)
                        {
                            cont--;

                            Console.ForegroundColor= ConsoleColor.Red;
                            Console.WriteLine("Hilo 2: "+cont);

                            if (cont<=-500)
                            {
                                terminado= true;
                            }
                        }
                    }
                }
            });

            suma.Start();
            resta.Start();

            suma.Join();
            resta.Join();


            Console.ResetColor();
            Console.WriteLine("El ganador es el Hilo "+ganador);



        }
        static readonly object l = new object();
        static int cont = 0;
        static bool terminado = false;
        static int ganador;
        //static void incrementar()
        //{

        //    while (!terminado)
        //    {
        //        lock (l)
        //        {
        //            if (!terminado)
        //            {
        //                cont++;
        //                Console.ForegroundColor = ConsoleColor.Green;
        //                Console.WriteLine("Hilo 1: " + cont);

        //                if (cont >= 500)
        //                {
        //                    terminado = true;
        //                    ganador = 1;
        //                }
        //            }
        //        }
        //    }
        //}
        //static void decrementar()
        //{
        //    while (!terminado)
        //    {
        //        lock (l)
        //        {
        //            if (!terminado)
        //            {
        //                cont--;
        //                Console.ForegroundColor = ConsoleColor.Red;
        //                Console.WriteLine("Hilo 2: " + cont);

        //                if (cont <= -500)
        //                {
        //                    terminado = true;
        //                    ganador = 2;

        //                }


        //            }
        //        }
        //    }
        //}
    }
}
