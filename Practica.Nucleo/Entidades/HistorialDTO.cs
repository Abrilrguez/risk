using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica.Nucleo.Entidades
{
    public class HistorialDTO
    {
        public int Id { get; set; }
        public string Fecha { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string Ciudad { get; set; }
        public string EstadoPaquete { get; set; }
        public Usuario Usuario { get; set; }
    }
}
