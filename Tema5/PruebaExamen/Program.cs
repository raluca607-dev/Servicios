namespace PruebaExamen
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Backup bu = new Backup();

            int num = await bu.InitBackup(Path.Combine(Environment.GetEnvironmentVariable("HOMEPATH"), "examen"));

            Console.WriteLine($"Total de archivos copiados: {num}");

            Console.WriteLine(Array.FindIndex(bu.Log.ToArray(), numer => numer.Contains("100")));

            Thread hiloTortillas = new Thread(Tortilla);
            Thread hiloPatata = new Thread(() => Ingredientes("Patata"));
            Thread hiloCebillas = new Thread(() => Ingredientes("Cebolla"));

            hiloCebillas.Start();
            hiloPatata.Start();
            hiloTortillas.Start();

            hiloCebillas.Join();
            hiloPatata.Join();
            hiloTortillas.Join();

            Console.WriteLine($"Han sobrado {NCebollas} cebollas y {NPatatas} patatas");
        }

        static int NCebollas = 0;
        static int NPatatas = 0;
        static int NTortillas = 0;
        static readonly object l = new object();
        static bool terminadas = false;

        static void Tortilla()
        {
            while (!terminadas)
            {
                lock (l)
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
                        terminadas = true;
                    }
                }

            }
        }


        static void Ingredientes(string nombre)
        {
            Random random = new Random();

            while (!terminadas)
            {
                int num = random.Next(0, 2);
                lock (l)
                {
                    if (NTortillas < 10)
                    {

                        if (nombre.ToLower().Trim() == "patata")
                        {
                            NPatatas++;
                        }
                        else if (nombre.ToLower().Trim() == "cebolla")
                        {
                            NCebollas++;
                        }

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Cebollas: {NCebollas,3}");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"Patatas:  {NPatatas,3}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                if (num == 1)
                {

                    Thread.Sleep(50);
                }


            }



        }


    }
}
