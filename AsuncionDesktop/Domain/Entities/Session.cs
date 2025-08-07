using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Entities
{
    public static class Session
    {
        public static string Token { get; set; }
        public static string Username { get; set; }
        public static string Provincia { get; set; }
    }
}
