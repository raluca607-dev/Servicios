using System;
using System.Collections.Generic;
using System.Text;

namespace Ejercicio2
{
    internal class Caballo
    {
        public int Numero { get; set; }
        public int Posicion { get; set; }

        public Caballo(int numero)
        {
            Numero = numero;
            Posicion= 0;
        }
    }
}
