using System.Diagnostics;

namespace Ejercicio6
{
    internal class Program
    {
        static readonly object l = new object();
        static int fila = -1;
        static int columna = -1;
        static bool encontrado = false;

        static async Task Main(string[] args)
        {
            Random ran = new Random();
            int[,] arrayGrande = new int[10, 10000000];

            for (int i = 0; i < arrayGrande.GetLength(0); i++)
            {
                for (int j = 0; j < arrayGrande.GetLength(1); j++)
                {
                    arrayGrande[i, j] = ran.Next(1, arrayGrande.Length / 2 + 1);
                }
            }

            Stopwatch stopw = new Stopwatch();
            List<Task> tareas = new List<Task>();


            // TASKS
            stopw.Start();

            for (int i = 0; i < arrayGrande.GetLength(0); i++)
            {
                int num = i;
                tareas.Add(Task.Run(() => Busqueda(arrayGrande, num)));
            }

            await Task.WhenAll(tareas);
            stopw.Stop();

            msj(arrayGrande);
            Console.WriteLine($"Tiempo no lineal: {stopw.Elapsed.TotalMilliseconds} ms");


            // LINEAL

            encontrado = false;
            fila = -1;
            columna = -1;

            stopw.Restart();
            BusquedaLineal(arrayGrande);
            stopw.Stop();

            msj(arrayGrande);
            Console.WriteLine($"Tiempo lineal: {stopw.Elapsed.TotalMilliseconds} ms");
        }

        static void msj(int[,] array)
        {
            if (encontrado)
            {
                Console.WriteLine($"Posición: [{fila}, {columna}]");
                Console.WriteLine($"Comprobación: {array[fila, columna]}");
            }
            else
            {
                Console.WriteLine("No se encontró el 42");
            }

         
        }
        static void Busqueda(int[,] array, int x)
        {
            for (int j = 0; j < array.GetLength(1) && !encontrado; j++)
            {
                if (array[x, j] == 42)
                {
                    lock (l)
                    {
                        if (!encontrado)
                        {
                            fila = x;
                            columna = j;
                            encontrado = true;
                            Console.WriteLine($"Task {x} encontró el 42");
                        }
                    }

                }
            }
        }

        static void BusquedaLineal(int[,] array)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j] == 42)
                    {
                        fila = i;
                        columna = j;
                        encontrado = true;
                        return;
                    }
                }
            }
        }
    }
}
