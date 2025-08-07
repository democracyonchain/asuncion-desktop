using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public class Acta
    {
        public int Codigo { get; set; }
        public int Seguridad { get; set; }
        public int Provincia { get; set; }
        public int Canton{ get; set; }
        public int Parroquia { get; set; }
        public int Zona { get; set;}
        public int Junta { get; set;}
        public string Sexo { get; set; }
        public int Dignidad { get; set; }
        public int Pagina { get; set; }
        [JsonProperty("numero_paginas")]
        public int Paginas { get; set; }
        public string Path { get; set; }
        public List<Pagina> paginas { get; set; }
        public int Estado { get; set; }
        public int Sufragantes { get; set; }
        public int Blancos { get; set; }
        public int Nulos { get; set; }
        public string TxIcr { get; set; }
        public Acta()
        {
            paginas = new List<Pagina>();
        }
        public void ActualizaEstado(int nuevoEstado)
        {
            this.Estado = nuevoEstado;
        }
        public void ActualizaPath(string nuevoPath)
        {
            this.Path = nuevoPath;
        }
    }
}
