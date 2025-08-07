using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsuncionDesktop.Domain.Entities;
using AsuncionDesktop.Application.UseCases;
using AsuncionDesktop.Domain.Interfaces;
using AsuncionDesktop.Infrastructure.Services;
using AsuncionDesktop.Presentation.Components;


using System.Drawing.Imaging;
using Infrastructure.Services;
using Newtonsoft.Json;
using System.Net.Http;


namespace AsuncionDesktop.Presentation.Forms
{
    public partial class ScanForm : Form
    {
        #region Campos y Constructor

        private readonly ScanDocumentUseCase _scanDocumentUseCase;
        private readonly ProcessImagesUseCase _processImagesUseCase;
        private readonly IDirectoryService _directoryService;
        private ScannerService _scannerService;

        private string imagenPath;
        private string actaPath;
        private string cortePath;
        private string jsonPath;
        private List<Acta> actas = new List<Acta>();

        private ZoomPictureBox picActa1;

   
     

        public ScanForm(
            ScanDocumentUseCase scanDocumentUseCase,
            ProcessImagesUseCase processImagesUseCase,
            IDirectoryService directoryService)
        {
            InitializeComponent();
            _scanDocumentUseCase = scanDocumentUseCase;
            _processImagesUseCase = processImagesUseCase;
            _directoryService = directoryService;

            this.Shown += ScanForm_Shown;
            InitializeDirectories();
            InitializeLoader();
            SetIcons();

            lstActas.SmallImageList = imageList1;
            bgwPrincipal.WorkerReportsProgress = true;
            bgwPrincipal.ProgressChanged += bgwPrincipal_ProgressChanged;
            bgwPrincipal.RunWorkerCompleted += bgwPrincipal_RunWorkerCompleted;

            //picActa1 = new ZoomPictureBox
            //{
            //    Dock = DockStyle.Fill,
            //    BackColor = Color.White
            //};

            //this.Controls.Add(picActa1);
            //picActa1.BorderStyle = BorderStyle.FixedSingle;
            //picActa1.Top = 31;
            //picActa1.Left = 819;
            //picActa.Width = 300;
            //picActa.Height = 300;

            _processImagesUseCase.OnCandidatoSegmentado += ProcessImageUseCase_OnCandidatoSegmentado;
        }

        #endregion

        #region Inicialización y Configuración

        private void InitializeDirectories()
        {
            string basePath = _directoryService.GetBasePath();
            if (string.IsNullOrEmpty(basePath))
                throw new InvalidOperationException("BasePath no está configurado.");

            actaPath = Path.Combine(basePath, "actas");
            imagenPath = Path.Combine(basePath, "imagenes");
            cortePath = Path.Combine(basePath, "cortes");
            jsonPath = Path.Combine(basePath, "json");

            EnsureDirectoryExists(actaPath);
            EnsureDirectoryExists(imagenPath);
            EnsureDirectoryExists(cortePath);
            EnsureDirectoryExists(jsonPath);
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine($"Directorio creado: {path}");
            }
        }

        private void InitializeLoader()
        {
            pbProgreso.Minimum = 0;
            pbProgreso.Step = 1;
            pbProgreso.Visible = false;
        }

        private void SetIcons()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string iconPath1 = Path.Combine(basePath, @"..\..\Recursos\create.png");
            string iconPath2 = Path.Combine(basePath, @"..\..\Recursos\complete.png");
            imageList1.Images.Add(Image.FromFile(iconPath1));
            imageList1.Images.Add(Image.FromFile(iconPath2));
        }

        #endregion

        #region Eventos del Formulario

        private void ScanForm_Shown(object sender, EventArgs e)
        {
            LoadImages();
            UpdateCounters();
        }

        private void ScanForm_Load(object sender, EventArgs e)
        {
            this.FormClosing += ScanForm_FormClosing;

            _scannerService = new ScannerService(imagenPath);
            _scannerService.ImagenGuardada += ruta => MessageBox.Show($"Imagen guardada: {ruta}");
           
            _scannerService.ImagenEscaneada += (ruta, imagen) =>
            {
                BeginInvoke(new Action(() =>
                {
                    picActa.Image?.Dispose();
                    picActa.Image = imagen;
                    picActa.SizeMode = PictureBoxSizeMode.Zoom;

                    var item = new ListViewItem(Path.GetFileNameWithoutExtension(ruta));
                    item.Tag = ruta;
                    lstImages.Items.Add(item);
                }));
            };

            _scannerService.Error += mensaje => MessageBox.Show($"Error: {mensaje}");
            SetupListView();
        }

        private void ScanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _scannerService?.Cerrar(); // Libera correctamente el escáner
        }

       
        #endregion

            #region Escaneo y Procesamiento de Imágenes

        private  void cmdScan_Click(object sender, EventArgs e)
        {
            _scannerService.EscanearImagen(this);
        }
        private void bgwPrincipal_DoWork_1(object sender, DoWorkEventArgs e)
        {           
            //var arguments = (Tuple<string, ListView>)e.Argument;
            //string directoryPath = arguments.Item1;
            //ListView targetListView = arguments.Item2;

            //        LoadImageNamesToListView(directoryPath, targetListView, sender as BackgroundWorker);
            //        e.Result = arguments;

          }

        private async void cmdPrepare_Click(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Leer todas las imágenes en el directorio de trabajo
                List<string> imagePaths = await _processImagesUseCase.GetImagesAsync(imagenPath);

                if (imagePaths.Count == 0)
                {
                    MessageBox.Show("No se encontraron imágenes en el directorio.");
                    return;
                }

                // Configurar la barra de progreso
                pbProgreso.Visible = true;
                pbProgreso.Maximum = imagePaths.Count;
                pbProgreso.Value = 0;

                // 2️⃣ Procesar cada imagen
               foreach (string imagePath in imagePaths)
                {
                    try
                    {
                        ShowImage(imagePath);
        
                        await Task.Delay(10);
        
                        // Procesar la imagen
                        await _processImagesUseCase.ProcessActaAsync(imagePath, actas, cortePath);
        
                        // Encontrar el acta procesada (la última o la correspondiente a esta imagen)
                        Acta actaProcesada = actas.LastOrDefault(); // O buscar la acta específica
        
                        if (actaProcesada != null)
                        {
                            // Encontrar la página procesada
                            Pagina paginaProcesada = actaProcesada.paginas.LastOrDefault(); // O buscar la página específica
            
                            if (paginaProcesada != null)
                            {
                                // Crear la carpeta destino si no existe
                                Directory.CreateDirectory(actaPath);
                
                                // Construir el nuevo nombre de archivo
                                string nuevoNombre = $"{actaProcesada.Codigo}_{paginaProcesada.Numero}{Path.GetExtension(imagePath)}";
                                string rutaDestino = Path.Combine(actaPath, nuevoNombre);
                
                                // Mover y renombrar la imagen
                                if (File.Exists(rutaDestino))
                                {
                                    File.Delete(rutaDestino); // Eliminar si ya existe
                                }
                                if (picActa.Image != null)
                                {
                                    picActa.Image.Dispose();
                                    picActa.Image = null;
                                    GC.Collect();  // Forzar recolección de basura para liberar el archivo
                                    GC.WaitForPendingFinalizers();
                                }

                                File.Move(imagePath, rutaDestino);
                
                                // Actualizar la ruta en la página procesada
                                paginaProcesada.Path = rutaDestino;

                                string nombreSinExtension = Path.GetFileNameWithoutExtension(imagePath);
                                Invoke(new Action(() =>
                                {
                                    foreach (ListViewItem item in lstImages.Items)
                                    {
                                        if (item.Text == nombreSinExtension)
                                        {
                                            lstImages.Items.Remove(item);
                                            break;
                                        }
                                    }

                                    // 🔹 Agregar la nueva entrada a `lstPages`
                                    lstPages.Items.Add(new ListViewItem(Path.GetFileNameWithoutExtension(nuevoNombre)));
                                }));
                                // 🔹 Actualizar `lstActas` en la UI
                                Invoke(new Action(() =>
                                {
                                    UpdateOrAddActaToListView(actaProcesada, paginaProcesada);
                                }));

                            }
                        }
        
                        // Actualizar la barra de progreso en la UI
                        Invoke(new Action(() => pbProgreso.Value++));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error procesando la imagen {Path.GetFileName(imagePath)}: {ex.Message}");
                    }
}

                pbProgreso.Visible = false;
                MessageBox.Show("Procesamiento completado.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante el procesamiento: {ex.Message}");
            }
        }

        private async void cmdProcess_Click(object sender, EventArgs e)
        {
            try
            {
                if (actas == null || actas.Count == 0)
                {
                    MessageBox.Show("No hay actas procesadas.");
                    return;
                }

                // 1. Obtener acta
                Acta acta = actas.Last();
                acta.Paginas = acta.paginas.Count();
                    var votosSufragantes = acta.paginas?
                    .SelectMany(p => p.candidatos ?? new List<Candidato>()) // aplana todos los candidatos
                    .FirstOrDefault(c => c.Id == 111)?.VotosIa ?? 0;
                acta.Sufragantes = votosSufragantes;
                    var votosBlancos = acta.paginas?
                    .SelectMany(p => p.candidatos ?? new List<Candidato>()) // aplana todos los candidatos
                    .FirstOrDefault(c => c.Id == 222)?.VotosIa ?? 0;
                acta.Blancos = votosBlancos;
                    var votosNulos = acta.paginas?
                    .SelectMany(p => p.candidatos ?? new List<Candidato>()) // aplana todos los candidatos
                    .FirstOrDefault(c => c.Id == 333)?.VotosIa ?? 0;
                acta.Nulos = votosNulos;
                // 2. Subir imágenes a IPFS
                var ipfsService = new IpfsService();
                var uploadUseCase = new UploadActaToIpfsUseCase(ipfsService);
                bool exitoso = await uploadUseCase.ExecuteAsync(acta);
                if (!exitoso)
                {
                    MessageBox.Show("❌ Falló la carga de imágenes a IPFS.");
                    return;
                }

                // 3. Enviar a la blockchain
                var apiService = new CardanoApiService();
                var useCase = new SendActaToBlockchainUseCase(apiService);
                string txIcr = "";
                try
                {
                    string respuesta = await useCase.ExecuteAsync(acta);
                    txIcr = respuesta;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Error al ejecutar transacción:\n{ex.Message}");
                    return;
                }

                if (txIcr.StartsWith("Error:"))
                {
                    MessageBox.Show("❌ No se pudo enviar el acta a la blockchain.\n" + txIcr);
                    return;
                }

                // 4. Mapear DTO para backend tradicional
                var dtoActa = MapToActaUpdateDto(acta);
                dtoActa.txicr = txIcr; // incluir hash de blockchain en el DTO

                // 5. Enviar al backend tradicional
                var token = Session.Token;
                var digitalizacionService = new DigitalizacionService(token);
                var resultado = await digitalizacionService.EnviarActaDigitalizadaAsync(dtoActa);

                MessageBox.Show(resultado);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error durante el proceso:\n{ex.Message}");
            }
        }



        private void UpdateOrAddActaToListView(Acta acta, Pagina pagina)
        {
            lstActas.Invoke(new Action(() =>
            {
                // Buscar si el acta ya está en el ListView
                ListViewItem item = lstActas.Items.Cast<ListViewItem>()
                    .FirstOrDefault(it => it.SubItems[1].Text == acta.Codigo.ToString());

                if (item == null)
                {
                    // 🔹 Si el acta no existe, agregarla como nueva
                    item = new ListViewItem { ImageIndex = 0 };
                    item.SubItems.Add(acta.Codigo.ToString());
                    item.SubItems.Add(acta.Seguridad.ToString());

                    for (int i = 1; i <= GetMaxPageConfig(); i++)
                    {
                        item.SubItems.Add("0");  // Inicializar con "0"
                    }

                    lstActas.Items.Add(item);
                }

                // 🔹 Actualizar la página correspondiente
                int subItemIndex = pagina.Numero + 2;
                while (item.SubItems.Count <= subItemIndex)
                {
                    item.SubItems.Add("0");
                }

                item.SubItems[subItemIndex].Text = "X";

                // 🔹 Si todas las páginas están completas, cambiar la imagen del acta
                if (IsActaComplete(acta))
                {
                    item.ImageIndex = 1; // Cambia la imagen cuando se completa
                }

                lstActas.Refresh();
            }));
        }



        #endregion

        #region Métodos Auxiliares

        private void LoadImages()
        {
            pbProgreso.Visible = true;
            pbProgreso.Value = 0;
            lstImages.View = View.Details;
            lstImages.Columns.Add("Nombre del Archivo", 800);

            lstPages.View = View.Details;
            lstPages.Columns.Add("Nombre del Archivo", 800);

            LoadImageNamesToListView(imagenPath, lstImages);
            LoadImageNamesToListView(actaPath, lstPages);

            StartFirstTask();
        }

        private void StartFirstTask()
        {
            bgwPrincipal.RunWorkerAsync(new Tuple<string, ListView>(imagenPath, lstImages));
        }

        private void SetupListView()
        {
            int maxPages = GetMaxPageConfig();
            foreach (ColumnHeader column in lstActas.Columns)
            {
                column.Width = 60;
            }


            // Limpiar columnas existentes
            lstActas.Columns.Clear();

            // Añadir columnas fijas
            lstActas.Columns.Add("Est", 20);
            lstActas.Columns.Add("Código", 50);
            lstActas.Columns.Add("Seguridad", 50);

            // Añadir columnas dinámicas para cada página
            for (int i = 1; i <= maxPages; i++)
            {
                lstActas.Columns.Add("Pág " + i.ToString(), 50);
            }

            // Añadir columnas de total y estado
            lstActas.Columns.Add("Total", 50);
            lstActas.Columns.Add("Estado", 100);
            lstActas.Scrollable = true;
            lstActas.View = View.Details;
            lstActas.Width = 600;

        }

        private void UpdateCounters()
        {
            int totalProcessedImages = Directory.GetFiles(actaPath, "*.tif").Length;
            int totalOriginalImages = Directory.GetFiles(imagenPath, "*.tif").Length;
            int totalActas = lstActas.Items.Count;

            lblTotalPaginas.Text = totalProcessedImages.ToString();
            lblTotalImagenes.Text = totalOriginalImages.ToString();
            lblTotalActas.Text = totalActas.ToString();
        }

        #endregion

        #region Eventos de Lista
       
        private void lstImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstImages.SelectedItems.Count > 0)
            {
                string filePath = Path.Combine(imagenPath, lstImages.SelectedItems[0].Text)  + ".tif";
                ShowImage(filePath);
            }
        }

        private void lstPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstPages.SelectedItems.Count > 0)
            {
                string filePath = Path.Combine(actaPath, lstPages.SelectedItems[0].Text) + ".tif"; ;
                ShowImage(filePath);
            }
        }

        private void ShowImage(string imagePath)
        {
            try
            {
                if (!File.Exists(imagePath))
                {
                    MessageBox.Show($"La imagen no existe: {imagePath}");
                    return;
                }

                // 🔹 Liberar imagen anterior antes de asignar la nueva
                if (picActa.Image != null)
                {
                    picActa.Image.Dispose();
                    picActa.Image = null;
                }

                using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);
                        ms.Position = 0;

                        Image image = Image.FromStream(ms);

                        // 🔹 Usar Invoke para asegurar que el PictureBox se actualiza en el hilo correcto
                        picActa.Invoke(new Action(() =>
                        {
                            picActa.Image = (Image)image.Clone(); // Clonar para evitar bloqueo
                            picActa.SizeMode = PictureBoxSizeMode.Zoom;
                            picActa.Refresh();
                        }));

                        image.Dispose(); // Liberar memoria
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar la imagen: {ex.Message}");
            }
        }



        #endregion

        #region BackgroundWorker

        private void bgwPrincipal_DoWork(object sender, DoWorkEventArgs e)
        {
            //var arguments = (Tuple<string, ListView>)e.Argument;
            //string directoryPath = arguments.Item1;
            //ListView targetListView = arguments.Item2;

            //LoadImageNamesToListView(directoryPath, targetListView, sender as BackgroundWorker);
            //e.Result = arguments;
        }

        private void bgwPrincipal_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbProgreso.Value = e.ProgressPercentage;
        }

        private void bgwPrincipal_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error: " + e.Error.Message);
            }
            else if (!e.Cancelled)
            {
                pbProgreso.Visible = false;
            }
        }

        private void LoadImageNamesToListView(string directoryPath, ListView listView)
        {
            if (string.IsNullOrEmpty(directoryPath) || listView == null)
                return;

            listView.Items.Clear(); // Limpiar la lista antes de agregar nuevos elementos

            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
            FileInfo[] files = dirInfo.GetFiles("*.tif");

            foreach (FileInfo file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Name);
                if (listView.InvokeRequired)
                {
                    listView.Invoke(new Action(() => listView.Items.Add(new ListViewItem(fileName))));
                }
                else
                {
                    listView.Items.Add(new ListViewItem(fileName));
                }
            }
        }

        private void ProcessImageUseCase_OnCandidatoSegmentado(Pagina pagina, Candidato candidato)
        {
            Invoke(new Action(() =>
            {
                try
                {
                    if (File.Exists(candidato.Path))
                    {
                        // Usar un enfoque diferente para cargar la imagen
                        using (var bitmap = new Bitmap(candidato.Path))
                        {
                            picSegmento.Image?.Dispose();
                            picSegmento.Image = new Bitmap(bitmap); // Crear una copia para evitar bloqueos
                            picSegmento.SizeMode = PictureBoxSizeMode.Zoom; // o StretchImage
                            // Forzar actualización visual
                            picSegmento.Refresh();

                            // Información de depuración
                            Text = $"Imagen cargada: {Path.GetFileName(candidato.Path)}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al mostrar imagen: {ex.Message}");
                }
            }));
        }

        private bool IsActaComplete(Acta acta)
        {
            int maxPages = acta.Paginas;
            return acta.paginas.Count == maxPages && acta.paginas.All(p => p.Numero >= 1 && p.Numero <= maxPages);
        }
        public static int GetMaxPageConfig()
        {
            return int.Parse(ConfigurationManager.AppSettings["MaxPage"]);
        }

        private ActaDTO MapToActaUpdateDto(Acta acta)
        {
            var pagina = acta.paginas.FirstOrDefault();

            return new ActaDTO
            {
                id = acta.Codigo,
                blancos = acta.Blancos,
                nulos = acta.Nulos,
                sufragantes = acta.Sufragantes,
                votosicr = acta.Sufragantes,
                txicr=acta.TxIcr, 
                imagenacta = new ImagenActaDTO
                {
                    imagen = pagina != null && File.Exists(pagina.Path)
                    ? File.ReadAllBytes(pagina.Path)
                    : null, // o la URL/IPFS si ya subiste
                    hash = pagina?.Hash ?? "",
                    nombre = pagina?.Nombre??"",
                    pagina = pagina?.Numero.ToString() ?? "",
                    pathipfs = pagina?.Path ?? ""
                },
                imagensegmento = acta.paginas
                    .SelectMany(p => p.candidatos)
                    .Where(c => c.Id < 100)
                    .Select(c => new ImagenSegmentoDTO
                    {
                        candidato_id = c.Id,
                        imagen = c != null && File.Exists(c.Path)
                        ? File.ReadAllBytes(c.Path)
                        : null,
                        hash = c.Hash ?? "",
                        nombre = Path.GetFileName(c.Path ?? ""),
                        pathipfs = c.Url?? ""
                    })
                    .ToList(),
                votos = acta.paginas
                    .SelectMany(p => p.candidatos)
                    .Where(c => c.Id < 100)
                    .Select(c => new VotoReconocidoDTO
                    {
                        candidato_id = c.Id,
                        votosicr = c.VotosIa
                    })
                    .ToList()
            };
        }

        


        #endregion
    }
}
