using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string palabra = txtCadena.Text.Trim();
            string ruta = txtUrl.Text.Trim();
            if (!Directory.Exists(ruta))
            {
                MessageBox.Show("El directorio no existe");
            }
            else if (string.IsNullOrEmpty(txtCadena.Text))
            {
                MessageBox.Show("Introduce una palabra");
            }
            else
            {
                listResultado.Items.Clear();

                try
                {
                    DirectoryInfo directorio = new DirectoryInfo(ruta);

                    List<Task<string>> tareas = new List<Task<string>>();

                    foreach (FileInfo archivo in directorio.GetFiles("*.txt"))
                    {

                        Task<string> tarea = Task.Run(() =>

                            BuscaPalabra(archivo.FullName, palabra)
                        );
                        tareas.Add(tarea);

                    }

                    while (tareas.Count > 0)
                    {
                        Task<string> terminado = await Task.WhenAny(tareas);

                        tareas.Remove(terminado);

                        string resultado = await terminado;

                        listResultado.Items.Add(resultado);
                    }


                }
                catch (IOException ex)
                {
                    Debug.WriteLine(ex.Message);

                }
                catch (UnauthorizedAccessException ex)
                {
                    Debug.WriteLine(ex.Message);

                }



            }
        }

        private string BuscaPalabra(string ruta, string cadena)
        {
            string contenido = "";
            int cont = 0;
            try
            {
                contenido = File.ReadAllText(ruta);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            for (int i = 0; i < contenido.Length - cadena.Length; i++)
            {
                bool encontrado = true;
                for (int j = 0; j < cadena.Length; j++)
                {
                    if (contenido[i + j] != cadena[j])
                    {
                        encontrado = false;
                        break;
                    }
                }

                if (encontrado)
                {
                    cont++;
                }

            }


            return $"{Path.GetFileName(ruta)}-{cadena}:{cont}";
        }


        private async void btnPosicion_Click(object sender, EventArgs e)
        {
            string palabra = txtCadena.Text.Trim();
            string ruta = txtUrl.Text.Trim();
            if (!Directory.Exists(ruta))
            {
                MessageBox.Show("El directorio no existe");
            }
            else if (string.IsNullOrEmpty(palabra))
            {
                MessageBox.Show("Introduce una palabra");
            }
            else
            {
                listResultado.Items.Clear();

                try
                {
                    DirectoryInfo directorio = new DirectoryInfo(ruta);

                    List<Task<string>> tareas = new List<Task<string>>();

                    foreach (FileInfo archivo in directorio.GetFiles("*.txt"))
                    {

                        Task<string> tarea = Task.Run(() =>

                            BuscaPosicion(archivo.FullName, palabra)
                        );
                        tareas.Add(tarea);

                    }
                    string[] resultados = await Task.WhenAll(tareas);

                    foreach (string resultado in resultados)
                    {
                        listResultado.Items.Add("Posición: " + resultado);
                    }


                }
                catch (IOException ex)
                {
                    Debug.WriteLine(ex.Message);

                }
                catch (UnauthorizedAccessException ex)
                {
                    Debug.WriteLine(ex.Message);

                }


            }
        }

        private string BuscaPosicion(string ruta, string cadena)
        {
            string contenido = "";
            int posicion = 0;
            try
            {
                contenido = File.ReadAllText(ruta);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            for (int i = 0; i < contenido.Length - cadena.Length; i++)
            {
                bool encontrado = true;
                for (int j = 0; j < cadena.Length; j++)
                {
                    if (contenido[i + j] != cadena[j])
                    {
                        encontrado = false;
                        break;
                    }
                }

                if (encontrado)
                {
                    posicion = i;
                    break;
                }
            }
            return $"Archivo: {Path.GetFileName(ruta)} - " +
           $"Palabra: {cadena} - Posición: {posicion}";
        }


        private async void btnHttp_Click(object sender, EventArgs e)
        {
            string ruta = txtUrl.Text.Trim();
            listResultado.Items.Clear();

            try
            {
                using (HttpClient cliente = new HttpClient())
                {

                    HttpResponseMessage respuesta = await cliente.GetAsync(ruta);

                    listResultado.Items.Add("HTTP: " + respuesta.ToString());

                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }
}
