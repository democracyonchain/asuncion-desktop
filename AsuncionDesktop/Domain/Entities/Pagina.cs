using AsuncionDesktop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public class Pagina
    {
        public int ActaId { get; set; }
        public int Numero { get; set; }
        public string Nombre{ get; set; }
        public string Path{ get; set; }
        public List<Candidato> candidatos { get; set; }

        public int Estado { get; set; }
        public Point Referencia { get; set; }
        public Pagina()
        {
            candidatos = new List<Candidato>();
        }
        public void ActualizaNombre(string nuevoNombre)
        {
            this.Nombre = nuevoNombre;
        }
        public void ActualizaEstado(int nuevoEstado)
        {
            this.Estado = nuevoEstado;
        }
        public void ActualizaPath(string nuevoPath)
        {
            this.Path = nuevoPath;
        }
        public void ActualizaReferencia(Point nuevaReferencia)
        {
            this.Referencia = nuevaReferencia;
        }
    }
}



