using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public class AuthResponse
    {
        public string Provincia { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
    }

}
