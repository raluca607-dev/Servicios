namespace Ejercicio2
{
    internal class Program
    {
        static bool terminado = false;
        static readonly object l = new object();
        static int ganador = 0;
        static Caballo[] caballos = new Caballo[5];
        static Thread[] hilos = new Thread[5];
        static void Main(string[] args)
        {
            
            int apuesta = 0;
            int seguir = 0;
            do
            {
                Console.Clear();
                terminado = false;
                ganador = 0;
                do
                {
                    Console.WriteLine("Por qué caballo quieres apostar? (1-5) ");
                    if (!int.TryParse(Console.ReadLine(), out apuesta))
                    {
                        Console.WriteLine("Número incorrecto");
                    }
                } while (apuesta < 1 || apuesta > 5);

                for (int i = 0; i < 5; i++)
                {
                    caballos[i] = new Caballo(i);
                    hilos[i] = new Thread(correr);

                }


                for (int i = 0; i < 5; i++)
                {
                    hilos[i].Start(caballos[i]);
                }
                for (int i = 0; i < 5; i++)
                {

                    hilos[i].Join();
                }

                if (ganador == apuesta)
                {
                    Console.WriteLine("¡Has ganado!");
                }
                else
                {
                    Console.WriteLine("Lo siento mucho, has perdido");
                }

                Console.WriteLine();

                Console.WriteLine("Quieres volver a apostar?\n1. Si\n2. No");
                while (!int.TryParse(Console.ReadLine(), out seguir) || (seguir != 1 && seguir != 2))
                {
                    Console.WriteLine("Opción incorrecta.");
                }



            } while (seguir != 2);

        }

        static void correr(object horse)
        {
            Random r = new Random();
            Caballo caballo = (Caballo)horse;


            while (!terminado)
            {
                lock (l)
                {
                    if (!terminado)
                    {
                        caballo.Posicion += r.Next(1, 5);
                        
                        for (int i = 0; i < 5; i++)
                        {

                            imprimir(caballos[i].Numero + 1, caballos[i].Posicion);
                        }

                        if (caballo.Posicion >= 50)
                        {
                            terminado = true;
                            ganador = caballo.Numero + 1;
                        }
                        Thread.Sleep(r.Next(1, 50));
                    }
                }
            }


        }
        public static void imprimir(int num, int pos)
        {
            Console.SetCursorPosition(63, num);
            Console.Write("|");

            Console.SetCursorPosition(0, num);

            Console.Write($"Caballo {num}:");
            for (int i = 0; i < pos; i++)
            {
                Console.Write("-");
            }
            Console.Write("> /-\\º");


            Console.WriteLine();
        }

    }
}
