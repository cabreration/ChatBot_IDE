using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot_IDE.Logica
{
    class ErrorC
    {
        public string tipo;
        public int fila;
        public int columna;
        public string lexema;
        public string descripcion;

        //constructor para un lexico y un sintactico
        public ErrorC(string tipo, int fila, int columna, string lexema, string descripcion) {
            this.tipo = tipo;
            this.fila = fila;
            this.columna = columna;
            this.lexema = lexema;
            this.descripcion = descripcion;
        }

        //constructor para un error semantico
        public ErrorC(string tipo, string descripcion) {
            this.tipo = tipo;
            this.descripcion = descripcion;
            this.fila = 0;
            this.columna = 0;
            this.lexema = "";
        }

        public ErrorC()
        {
            tipo = null;
            descripcion = null;
            fila = 0;
            columna = 0;
            lexema = "";
        }
    }
}
