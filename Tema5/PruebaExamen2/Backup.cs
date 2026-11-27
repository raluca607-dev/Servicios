using System;
using System.Collections.Generic;
using System.Text;

namespace PruebaExamen2
{
    internal class Backup
    {
        private List<string> log = new List<string>();

        public List<string> Log
        {
            get { return log; }
        }

        public FileInfo[] buscaArchivos(string dirOrigen, string[] extensiones)
        {

            if (Directory.Exists(dirOrigen))
            {
                DirectoryInfo dir = new DirectoryInfo(dirOrigen);
                FileInfo[] archivos = dir.GetFiles();

                FileInfo[] resultado = Array.FindAll(archivos,
                    archivo => Array.Exists(extensiones,
                    ex => ex == archivo.Extension));

                return resultado;
            }
            else
            {
                return null;
            }

        }

        public bool copiaArchivo(FileInfo acopiar, string dirDestino)
        {
            int cont = 0;
            try
            {
                using (StreamReader sr = new StreamReader(acopiar.FullName))
                {
                    using (StreamWriter sw = new StreamWriter(dirDestino))
                    {
                        string linea;
                        while ((linea = sr.ReadLine()) != null)
                        {
                            sw.WriteLine(linea);
                            cont++;
                            if (cont % 10 == 0)
                            {
                               
                                log.Add($"{acopiar.Name,20}," +
                                    $"{sw.BaseStream.Position,10}," +
                                    $"{cont,5}");
                                Console.WriteLine($"{acopiar.Name,20}," +
                                    $"{sw.BaseStream.Position,10}," +
                                    $"{cont,5}");


                            }
                        }
                        return true;
                    }
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public int buscaYcopia(string dirOrigen, string dirDestino, string[] extensiones)
        {
            int cont = 0;

            FileInfo[] f = buscaArchivos(dirOrigen, extensiones);

            Array.ForEach(f,
                archivo =>
                {
                    if (copiaArchivo(archivo, dirDestino))
                    {
                        cont++;
                    }
                });
            return cont;
        }

        public async Task<int> InitBackup(string dirOrigen)
        {
            string dirDestino = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "backup");

            Task<int> hilo1 = Task.Run(() => buscaYcopia(dirOrigen, dirDestino, new string[] { ".txt", ".md", ".json" }));
            Task<int> hilo2 = Task.Run(() => buscaYcopia(dirOrigen, dirDestino, new string[] { ".java", ".cs", ".py" }));

            int[] resultados = await Task.WhenAll(hilo1, hilo2);

            return resultados[0] + resultados[1];

        }

    }
}
