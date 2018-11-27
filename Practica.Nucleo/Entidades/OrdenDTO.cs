using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica.Nucleo.Entidades
{
   public class OrdenDTO 
    {
        public int Id { get; set; }
        public string Folio { get; set; }
        public string Fecha { get; set; }
        public Cliente Cliente { get; set; }
        public Destinatario Destinatario { get; set; }
        public Usuario Usuario { get; set; }
        public Paquete Paquete { get; set; }
        public IList<Historial> Historiales { get; set; }
        public double Precio { get; set; }
        public string NumeroRastreo { get; set; }
        public string Estado { get; set; }
    }
}
