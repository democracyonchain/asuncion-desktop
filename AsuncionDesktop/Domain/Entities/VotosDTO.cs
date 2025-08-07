using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public class VotosDTO
    {
        public int candidato_id { get; set; }           // ID del candidato
        public int votosdigitacion { get; set; }        // Cantidad de votos digitados o reconocidos
    }
}
