using AsuncionDesktop.Domain.Entities;
using AsuncionDesktop.Domain.Interfaces;
using AsuncionDesktop.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Application.UseCases
{
    public class SendActaToBlockchainUseCase
    {
        private readonly ICardanoApiService _apiService;

        public SendActaToBlockchainUseCase(ICardanoApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<string> ExecuteAsync(Acta acta)
        {
            return await _apiService.EnviarActaAsync(acta);
        }
    }
}
