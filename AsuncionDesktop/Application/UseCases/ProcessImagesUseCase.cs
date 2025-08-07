using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsuncionDesktop.Domain.Entities;
using AsuncionDesktop.Domain.Interfaces;
using AsuncionDesktop.Infrastructure.Services;
using AsuncionDesktop.Presentation.Components;
using Newtonsoft.Json;

namespace AsuncionDesktop.Application.UseCases
{
    public class ProcessImagesUseCase
    {
        private readonly IImageService _imageService;
        private readonly IQRCodeService _qrCodeService;
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;

        public event Action<Acta> OnActaProcesada; // Evento para actualizar la UI
        public event Action<Pagina> OnPaginaProcesada;

          // Nuevo evento para candidatos segmentados
        public delegate void CandidatoSegmentadoEventHandler(Pagina pagina, Candidato candidato);
        public event CandidatoSegmentadoEventHandler OnCandidatoSegmentado;

        public ProcessImagesUseCase(
            IImageService imageService,
            IQRCodeService qrCodeService,
            IDirectoryService directoryService,
            IFileService fileService)
        {
            _imageService = imageService;
            _qrCodeService = qrCodeService;
            _directoryService = directoryService;
            _fileService = fileService;
        }
        public async Task<List<string>> GetImagesAsync(string directoryPath)
        {
            return await Task.Run(() =>
            {
                if (!Directory.Exists(directoryPath))
                {
                    return new List<string>();
                }

                return Directory.GetFiles(directoryPath, "*.tif").ToList();
            });
        }


        public async Task ProcessActaAsync(string imagePath, List<Acta> actas, string cortePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                throw new FileNotFoundException("El archivo de imagen no existe.", imagePath);

            try
            {
                // 1️⃣ Leer la imagen y extraer el código QR junto con la referencia
                var image = _imageService.LoadImage(imagePath);
                var (qrData, referencia) = _qrCodeService.DecodeQRCodeWithReference(image);

                if (string.IsNullOrEmpty(qrData))
                    throw new Exception("No se pudo leer el código QR.");

                // 2️⃣ Obtener la información del acta y la página SIN agregarlas aún
                var (nuevaActa, nuevaPagina) = ProcessActaData(qrData, imagePath);
                nuevaPagina.Referencia = referencia; // Asignar la referencia extraída del QR
                nuevaPagina.Path = (imagePath);
                nuevaPagina.Nombre= Path.GetFileName(imagePath);
                // 3️⃣ Buscar si la acta ya existe en la lista
                Acta actaExistente = actas.FirstOrDefault(a => a.Codigo == nuevaActa.Codigo);
                if (actaExistente == null)
                {
                    // ✅ Si la acta no existe, agregarla con la primera página incluida
                    nuevaActa.paginas.Add(nuevaPagina);
                    actas.Add(nuevaActa);
                }
                else
                {
                    // 4️⃣ Verificar si la página ya existe en el acta
                    bool paginaExiste = actaExistente.paginas.Any(p => p.Numero == nuevaPagina.Numero);

                    if (!paginaExiste)
                    {
                        // ✅ Si la página es nueva, agregarla
                        actaExistente.paginas.Add(nuevaPagina);
                    }
                    else
                    {
                        // 🔹 Si la página ya existe, no hacer nada (optimización)
                        return;
                    }
                }

                // 5️⃣ Notificar que la página ha sido procesada
                OnPaginaProcesada?.Invoke(nuevaPagina);

                // 6️⃣ Procesar la imagen y cortar los candidatos
                await SegmentarImagenAsync(imagePath, nuevaPagina, cortePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error procesando la página {Path.GetFileName(imagePath)}: {ex.Message}");
            }
        }


     

private (Acta acta, Pagina pagina) ProcessActaData(string data, string imagePath)
    {
        // Separar cabecera y JSON de candidatos
        int bracketIndex = data.IndexOf('[');
        string headerRaw = data.Substring(0, bracketIndex).Trim('"');
        string candidatosJson = data.Substring(bracketIndex);

        string[] headerParts = headerRaw.Split(',');

        int codigo = int.Parse(headerParts[1]);
        int numeroPagina = int.Parse(headerParts[9]);

        Acta acta = new Acta
        {
            Codigo = codigo,
            Seguridad = int.Parse(headerParts[1]),
            Provincia = int.Parse(headerParts[2]),
            Canton = int.Parse(headerParts[3]),
            Parroquia = int.Parse(headerParts[4]),
            Zona = int.Parse(headerParts[5]),
            Junta = int.Parse(headerParts[6]),
            Sexo = headerParts[7],
            Dignidad = int.Parse(headerParts[8]),
            Estado = 0,
            paginas = new List<Pagina>()
        };

        Pagina pagina = new Pagina
        {
            ActaId = codigo,
            Numero = numeroPagina,
            Estado = 0,
            Hash=  HashService.GenerateFileHash(imagePath),
            Path = imagePath,
            candidatos = new List<Candidato>()
        };

        // Usamos Newtonsoft.Json para deserializar el array de strings
        string[] candidatosArray = JsonConvert.DeserializeObject<string[]>(candidatosJson);

        if (pagina.Numero==1)
            {
                Candidato totalVotos = new Candidato
                {
                    Id = 111,
                    Orden = 1,
                    PatidoId = 111,
                    VotosIa = 0,
                    Estado = 0
                };
                pagina.candidatos.Add(totalVotos);
                Candidato votosBlancos = new Candidato
                {
                    Id = 222,
                    Orden = 2,
                    PatidoId = 222,
                    VotosIa = 0,
                    Estado = 0
                };
                pagina.candidatos.Add(votosBlancos);
                Candidato votosNulos = new Candidato
                {
                    Id = 333,
                    Orden = 3,
                    PatidoId = 333,
                    VotosIa = 0,
                    Estado = 0
                };
                pagina.candidatos.Add(votosNulos);
                Candidato votosTitulo = new Candidato
                {
                    Id = 444,
                    Orden = 4,
                    PatidoId = 444,
                    VotosIa = 0,
                    Estado = 0
                };
                pagina.candidatos.Add(votosTitulo);
            }


        foreach (string candidato in candidatosArray)
        {
            string[] info = candidato.Split(',');
            Candidato newCandidato = new Candidato
            {
                Id = int.Parse(info[0]),
                Orden = int.Parse(info[1]),
                PatidoId = int.Parse(info[2]),
                VotosIa = 0,
                Estado = 0
            };
            pagina.candidatos.Add(newCandidato);
        }

        return (acta, pagina);
    }


    private async Task SegmentarImagenAsync(string imagePath, Pagina pagina, string cortePath)
        {
          
            try
            {
                using (Bitmap bitmap = new Bitmap(imagePath))
                {
                    Point referencia = pagina.Referencia; // ✅ Usamos la referencia de la página
                    int startX = referencia.X + 390;
                    int startY = referencia.Y + 526;
                    //int startX = referencia.X - 410;
                    //int startY = referencia.Y + 412;
                   
                   
                    GoogleOcrService ocrService = new GoogleOcrService(@"Recursos\credenciales.json");


                    for (int i = 0; i < pagina.candidatos.Count; i++)
                    {
                        int y = startY + (i * 213);
                        Rectangle cropArea = new Rectangle(startX, y, 1490, 213);
                        //Rectangle cropArea = new Rectangle(startX, y, 2244, 236);

                        using (Bitmap candidateBitmap = bitmap.Clone(cropArea, bitmap.PixelFormat))
                        {
                            //string folderPath = Path.Combine("cortes", $"{pagina.ActaId}");
                            string folderPath = Path.Combine(cortePath, $"{pagina.ActaId}");
                            Directory.CreateDirectory(folderPath);

                            string outputFilename = Path.Combine(folderPath, $"{pagina.ActaId}_{pagina.candidatos[i].Id}.tif");
                            candidateBitmap.Save(outputFilename, System.Drawing.Imaging.ImageFormat.Tiff);



                            string votosDetectados = ocrService.ProcesarSegmento(outputFilename);
                            OcrTextProcessorService textProcessor = new OcrTextProcessorService();
                            int votosProcesados=textProcessor.ConvertirTextoANumero(votosDetectados);

                            Console.WriteLine($"Candidato {pagina.candidatos[i].Id}: {votosProcesados} votos");


                            // 🔹 Calcular hash de la imagen segmentada
                            string hash = HashService.GenerateFileHash(outputFilename);

                            // Guardar en la entidad Candidato
                            pagina.candidatos[i].VotosIa = votosProcesados;
                            pagina.candidatos[i].Path = outputFilename;
                            pagina.candidatos[i].Hash = hash;

                            // Notificar que se ha segmentado un candidato
                            OnCandidatoSegmentado?.Invoke(pagina, pagina.candidatos[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al segmentar la imagen de la página {pagina.Numero}: {ex.Message}");
            }
        }



      }
}
