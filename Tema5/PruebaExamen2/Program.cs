namespace PruebaExamen2
{
    internal class Program
    {
        static int NCebollas = 0;
        static int NPatatas = 0;
        static int NTortillas = 0;
        static bool terminada = false;
        static readonly object l = new object();

        static async Task Main(string[] args)
        {
            Backup backup = new Backup();

            string dire = Path.Combine(Environment.GetEnvironmentVariable("HOMEPATH"), "examen");

            int res = await backup.InitBackup(dire);

            Console.WriteLine(res);

            Console.WriteLine(Array.FindIndex(backup.Log.ToArray(), numeros => numeros.Contains("100")));


            Thread hilo1 = new Thread(Tortillas);
            Thread hilo2 = new Thread(() => Ingrediente("patata"));
            Thread hilo3 = new Thread(() => Ingrediente("cebolla"));

            hilo1.Start();
            hilo2.Start();
            hilo3.Start();

            hilo1.Join();
            hilo2.Join();
            hilo3.Join();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Han sobrado {NCebollas} cebolla/s y {NPatatas} patata/s");

        }


        static void Tortillas()
        {
            while (!terminada)
            {
                lock (l)
                {
                    if (!terminada)
                    {
                        if (NCebollas >= 5 && NPatatas >= 5)
                        {
                            NCebollas -= 5;
                            NPatatas -= 5;
                            NTortillas++;

                            Console.ForegroundColor = ConsoleColor.Green;

                            Console.WriteLine($"Tortillas: {NTortillas,3} Cebollas: {NCebollas,3} Patatas: {NPatatas,3} ");

                        }

                        if (NTortillas >= 10)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            terminada = true;
                        }
                    }
                }
            }
        }

        static void Ingrediente(string ingred)
        {
            Random r = new Random();
            while (!terminada)
            {

                lock (l)
                {
                    if (!terminada)
                    {
                        if (ingred.ToLower().Trim() == "patata")
                        {
                            NPatatas++;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"Patata:  {NPatatas}");
                        }
                        else if (ingred.ToLower().Trim() == "cebolla")
                        {
                            NCebollas++;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Cebolla:  {NCebollas}");

                        }

                    }
                }

                if (r.Next(0, 2) == 1)
                {
                    Thread.Sleep(50);
                }

            }

        }
    }
}
