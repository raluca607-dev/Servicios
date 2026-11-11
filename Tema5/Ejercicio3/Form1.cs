using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private async Task<string> DownloadFileAsync(string nombreArchivo, int delay)
        {
            await Task.Delay(delay);

            return $"Archivo {nombreArchivo} descargado en {delay} ms";
            Console.WriteLine("yipi");
        }

        private async void btnAceptar_Click(object sender, EventArgs e)
        {
            string nombre = txtFileName.Text;

            Random r = new Random();
            int tiempo = r.Next(100, 10001);

            string resultado = await DownloadFileAsync(nombre, tiempo);
            txtResult.Text=(resultado + "\r\n");
        }

    }
}
