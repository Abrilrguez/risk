using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica.Nucleo.Entidades
{
    public class Paquete : Persistent
    {
        public  override int Id { get; set; }
        public string Peso { get; set; }
        public string Tamanio { get; set; }
        public string Contenido { get; set; }
        public string Descripcion { get; set; }
    }
}
