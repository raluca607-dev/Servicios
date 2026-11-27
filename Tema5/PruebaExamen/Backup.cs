using System;
using System.Collections.Generic;
using System.Text;

namespace PruebaExamen
{
    internal class Backup
    {

        private List<string> log=new List<string>();

        public List<string> Log
        {
            get { return log; }
        }

        public FileInfo[] buscaArchivos(string direorigen, string[] extensiones)
        {

            if (Directory.Exists(direorigen))
            {
                DirectoryInfo carpeta = new DirectoryInfo(direorigen);

                FileInfo[] archivos = carpeta.GetFiles();

                FileInfo[] filtrados = Array.FindAll(
                    archivos,
                    archivo => Array.Exists(
                        extensiones,
                        ext => ext == archivo.Extension.ToLower()
                        )
                    );

                return filtrados;

            }
            else
            {
                return null;
            }

        }

        public bool copiaArchivo(FileInfo acopiar, string diredestino)
        {
            int cont = 0;
          
                try
                {
                    using (StreamReader sr = new StreamReader(acopiar.FullName))
                    {
                        using (StreamWriter sw = new StreamWriter(Path.Combine(diredestino, acopiar.Name)))
                        {
                            string linea;
                            while ((linea = sr.ReadLine()) != null)
                            {
                                sw.WriteLine(linea);

                                cont++;

                                if (cont % 10 == 0)
                                {
                                    log.Add($"Archivo: {acopiar.Name,20}," +
                                        $"Bytes: {acopiar.Length,10}," +
                                        $"Nº de líneas: {cont,5}");
                                    Console.WriteLine($"Archivo: {acopiar.Name,20}," +
                                        $"Bytes: {sw.BaseStream.Position,10}," +
                                        $"Nº de líneas: {cont,5}");
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

        public int buscaYCopia(string dirOrigen, string dirDestino, string[] extensiones)
        {

            FileInfo[] archivos = buscaArchivos(dirOrigen, extensiones);
            int cont = 0;
            Array.ForEach(archivos, archivo =>
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

            Task<int> buscaycopia1 = Task.Run(()
                => buscaYCopia(dirOrigen, dirDestino, new string[] { ".txt", ".md", ".json" }));
            
            Task<int> buscaycopia2 = Task.Run(()
                => buscaYCopia(dirOrigen, dirDestino, new string[] { ".java", ".cs", ".py" }));

            int[] resultados = await Task.WhenAll(buscaycopia1, buscaycopia2);
          
            return resultados[0] + resultados[1];

        }




    }
}
