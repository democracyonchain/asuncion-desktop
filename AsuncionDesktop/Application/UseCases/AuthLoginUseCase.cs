using AsuncionDesktop.Domain.Entities;
using AsuncionDesktop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsuncionDesktop.Application.UseCases
{
    public class AuthLoginUseCase
    {
        private readonly IAuthService _authService;
        public AuthLoginUseCase(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<AuthResponse> ExecuteAsync(string username, string password)
        {
            return _authService.LoginAsync(username, password);
        }
    }
}
