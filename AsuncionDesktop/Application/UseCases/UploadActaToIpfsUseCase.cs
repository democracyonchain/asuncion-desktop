using AsuncionDesktop.Domain.Entities;
using AsuncionDesktop.Infrastructure.Services;
using System.Linq;
using System.Threading.Tasks;

namespace AsuncionDesktop.Application.UseCases
{
    public class UploadActaToIpfsUseCase
    {
        private readonly IpfsService _ipfsService;

        public UploadActaToIpfsUseCase(IpfsService ipfsService)
        {
            _ipfsService = ipfsService;
        }

        public async Task<bool> ExecuteAsync(Acta acta)
        {
            if (acta.paginas == null || !acta.paginas.Any())
                return false;

            // Subir todas las imágenes de las páginas
            foreach (var pagina in acta.paginas)
            {
                if (string.IsNullOrEmpty(pagina.Path))
                    return false;

                var resultPagina = await _ipfsService.UploadFileAsync(pagina.Path);
                if (string.IsNullOrEmpty(resultPagina.Hash))
                    return false;
                pagina.Url = $"ipfs://{resultPagina.Hash}";
            }


            foreach (var candidato in acta.paginas.SelectMany(p => p.candidatos))
            {
                if (string.IsNullOrEmpty(candidato.Path)) return false;

                var resultCorte = await _ipfsService.UploadFileAsync(candidato.Path);
                if (string.IsNullOrEmpty(resultCorte.Hash)) return false;
                candidato.Url = $"ipfs://{resultCorte.Hash}";                
            }

            return true;
        }
    }
}
