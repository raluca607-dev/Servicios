using static System.Net.Mime.MediaTypeNames;

namespace Ejercicio5
{
    internal class Program
    {
        static readonly object l = new object();
        static List<int> lista = new List<int>();
        static bool pararP = false;
        static bool pararC = false;
        static int primos = 0;

        static void Main(string[] args)
        {

            Thread prod = new Thread(Producir);
            Thread cons = new Thread(Consumir);

            prod.Priority = ThreadPriority.BelowNormal;
            cons.Priority = ThreadPriority.AboveNormal;

            prod.Start();
            cons.Start();

            prod.Join();
            cons.Join();

            if (primos == 5)
            {
                pararP = false;
            }
        }

        static void Producir()
        {

            Random r = new Random();
            

            while (!pararP)
            {
                int num = r.Next(1000, 10001);
                lock (l)
                {
                    if (!pararP)
                    {
                        lista.Add(num);

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($"P:{num},");


                    }
                }
                Thread.Sleep(10);
            }

        }

        static void Consumir()
        {

            int num = 0;
            bool hayNum = false;
            while (!pararC)
            {
                lock (l)
                {
                    if (lista.Count > 0)
                    {
                        num = lista[0];
                        lista.RemoveAt(0);
                    }
                    if (num != 0)
                    {
                        if (!pararC)
                        {
                            if (EsPrimo(num))
                            {

                                primos++;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write($"C:PRIMO:{num},");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"C:NO_PRIMO:{num},");
                            }
                            if (primos == 5)
                            {
                                pararP = true;
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Productor ha finalizado");
                                Console.ForegroundColor = ConsoleColor.Red;

                            }
                            if (pararP && lista.Count == 0)
                            {
                                pararC = true;
                            }


                        }
                    }
                }
                Thread.Sleep(10);

            }

        }
        static bool EsPrimo(int numero)
        {
            if (numero < 2)
            {
                return false;

            }

            for (int i = 2; i < numero; i++)
            {
                if (numero % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
