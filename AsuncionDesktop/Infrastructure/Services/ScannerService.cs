using AsuncionDesktop;
using NTwain;
using NTwain.Data;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Infrastructure.Services
{
    public class ScannerService
    {
        private TwainSession _twain;
        private DataSource _currentSource;
        private readonly string _outputFolder;
        private bool _isBusy = false;

        public event Action<string> ImagenGuardada;
        public event Action<string> Error;
        public event Action<string, Image> ImagenEscaneada;

        private int _scanCounter = 0; // cuenta imágenes dentro de un mismo escaneo
        private string _scanBaseTimestamp = ""; // marca única por sesión


        public ScannerService(string outputFolder)
        {
            _outputFolder = outputFolder;
            Directory.CreateDirectory(_outputFolder); // Asegura que la carpeta exista
        }

        public void EscanearImagen(Control ventanaHost)
        {
            if (_isBusy)
            {
                Error?.Invoke("Ya hay un escaneo en curso. Por favor, espera.");
                return;
            }

            _isBusy = true;

            try
            {
                // Cerrar fuente y sesión previas si están abiertas
                if (_currentSource != null && _currentSource.IsOpen)
                {
                    _currentSource.Close();
                    _currentSource = null;
                }

                if (_twain != null && _twain.State > 3) // 3 = Open
                {
                    try
                    {
                        _currentSource?.Close();
                    }
                    catch { /* Ignorar error si ya estaba cerrada */ }

                    try
                    {
                        _twain?.Close();
                    }
                    catch { /* Ignorar error si no se puede cerrar */ }

                }

                var appId = TWIdentity.CreateFromAssembly(DataGroups.Image, typeof(Program).Assembly);
                _twain = new TwainSession(appId);

                _twain.TransferError += (s, e) =>
                {
                    Error?.Invoke("Error durante el escaneo.");
                    _isBusy = false;
                };

                _twain.StateChanged += (s, e) =>
                {
                    Console.WriteLine("Nuevo estado TWAIN: " + _twain.State);
                };

                _twain.DataTransferred += (s, e) =>
                {
                    try
                    {
                        if (e.NativeData != IntPtr.Zero)
                        {
                            // Si es la primera imagen, generar base de timestamp
                            if (_scanCounter == 0)
                            {
                                _scanBaseTimestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                            }

                            using (var stream = e.GetNativeImageStream())
                            {
                                var img = Image.FromStream(stream);
                                _scanCounter++;

                                string fileName = $"scan_{_scanBaseTimestamp}_p{_scanCounter}.tif";
                                string filePath = Path.Combine(_outputFolder, fileName);

                                img.Save(filePath, ImageFormat.Tiff);
                                ImagenEscaneada?.Invoke(filePath, (Image)img.Clone());
                                img.Dispose();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Error?.Invoke("Error al procesar imagen escaneada: " + ex.Message);
                    }
                };

                _twain.Open();

                var firstSource = _twain.FirstOrDefault();
                if (firstSource == null)
                {
                    Error?.Invoke("No se encontró ningún escáner disponible.");
                    _isBusy = false;
                    return;
                }

                _currentSource = firstSource;

                try
                {
                    _currentSource.Open();

                    if (ventanaHost == null || ventanaHost.Handle == IntPtr.Zero)
                    {
                        Error?.Invoke("La ventana host aún no está lista.");
                        _isBusy = false;
                        return;
                    }

                    //_currentSource.Enable(SourceEnableMode.NoUI, false, ventanaHost.Handle);
                    _currentSource.Enable(SourceEnableMode.NoUI, false, IntPtr.Zero);

                    _scanCounter = 0;

                }
                catch (Exception ex)
                {
                    Error?.Invoke("Error al habilitar el escáner: " + ex.Message);
                    _isBusy = false;
                }
            }
            catch (Exception ex)
            {
                Error?.Invoke("Excepción inesperada durante el escaneo: " + ex.Message);
                _isBusy = false;
            }
        }

        // Método para liberar la sesión manualmente
        public void Cerrar()
        {
            try
            {
                _currentSource?.Close();
                _twain?.Close();
            }
            catch
            {
                // Ignoramos cualquier excepción al cerrar
            }
            finally
            {
                _isBusy = false;
            }
        }

        // Método opcional para forzar la liberación del bloqueo si algo falla
        public void ForzarFinEscaneo()
        {
            _isBusy = false;
        }
    }
}
