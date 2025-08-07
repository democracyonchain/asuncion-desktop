using AsuncionDesktop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Domain.Interfaces
{
    public interface ICardanoApiService
    {
        Task<string> EnviarActaAsync(Acta acta);
    }
}
