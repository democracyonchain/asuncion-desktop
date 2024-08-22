using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing.Common;
using ZXing;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ListView = System.Windows.Forms.ListView;
using System.Drawing.Imaging;
using ZXing.QrCode;
using AsuncionDesktop.Domain.Entities;


namespace AsuncionDesktop.Presentation.Forms
{
    public partial class ScanForm : Form
    {
        #region Initialization & Constructor
        private string basePath=string.Empty;
        string actaPath = string.Empty;
        string imagenPath = string.Empty;
        string cortePath = string.Empty;
        string jsonPath = string.Empty;
        public List<Acta> actas = new List<Acta>();
        Point referencia;
        int referenciaX = 0;
        int referenciaY = 0;
        public ScanForm()
        {
            InitializeComponent();
            bgwPrincipal.WorkerReportsProgress = true;
            bgwPrincipal.ProgressChanged += bgwPrincipal_ProgressChanged;
            bgwPrincipal.RunWorkerCompleted += bgwPrincipal_RunWorkerCompleted;
            InitializeDirectories();
            InitializeLoader();
            LoadImages();
            SetIcons();
            lstActas.SmallImageList = imageList1;


        }
        private void ScanForm_Shown(object sender, EventArgs e)
        {
            if (!lstImages.IsHandleCreated)
            {
                lstImages.CreateControl();  
            }

            bgwPrincipal.RunWorkerAsync();
        }
        public void InitializeDirectories()
        {
            // Obtener la ruta base desde la configuración de la aplicación
            string basePath = ConfigurationManager.AppSettings["BasePath"];
            referenciaX = int.Parse(ConfigurationManager.AppSettings["ReferenciaX"]);
            referenciaY = int.Parse(ConfigurationManager.AppSettings["ReferenciaY"]);
            if (string.IsNullOrEmpty(basePath))
            {
                throw new InvalidOperationException("La configuración de BasePath no está especificada en el archivo de configuración.");
            }

            // Construir rutas completas para cada directorio necesario
             actaPath = Path.Combine(basePath, "actas");
             imagenPath = Path.Combine(basePath, "imagenes");
             cortePath = Path.Combine(basePath, "cortes");
             jsonPath = Path.Combine(basePath, "json");

            // Verificar y crear los directorios si no existen
            EnsureDirectoryExists(actaPath);
            EnsureDirectoryExists(imagenPath);
            EnsureDirectoryExists(cortePath);
            EnsureDirectoryExists(jsonPath);
        }
        private void EnsureDirectoryExists(string path)
        {
            // Verificar si el directorio existe
            if (!Directory.Exists(path))
            {
                // Si no existe, crear el directorio
                Directory.CreateDirectory(path);
                Console.WriteLine($"Directorio creado: {path}");  
            }
        }
        public static int GetMaxPageConfig()
        {
            return int.Parse(ConfigurationManager.AppSettings["MaxPage"]);
        }
        public void InitializeLoader()
        {
            pbProgreso.Minimum = 0;
            pbProgreso.Step = 1;
            pbProgreso.Visible = false;
        }
        public void LoadImages()
        {
            pbProgreso.Visible = true;
            pbProgreso.Value = 0;
            lstImages.View = View.Details; // Asegúrate de que esto está configurado
            lstImages.Columns.Add("Nombre del Archivo",800); // Asegúrate de que las columnas están definidas
            lstPages.View = View.Details; // Asegúrate de que esto está configurado
            lstActas.Columns.Add("Nombre del Archivo",800);
            bgwPrincipal.RunWorkerAsync(imagenPath);
        }
        public void SetIcons()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string iconPath1 = Path.Combine(basePath, @"..\..\Recursos\create.png");
            string iconPath2 = Path.Combine(basePath, @"..\..\Recursos\complete.png");

            imageList1.Images.Add(Image.FromFile(iconPath1));  // Ícono para estado inicial
            imageList1.Images.Add(Image.FromFile(iconPath2));
        }
        #endregion
        #region Form Events
        private void ScanForm_Load(object sender, EventArgs e)
        {
            SetupListView();

        }

        #endregion
        #region User Control Events
        private void bgwPrincipal_DoWork_1(object sender, DoWorkEventArgs e)
        {
            string directoryPath = e.Argument as string;
            LoadImageNamesToListView(directoryPath, lstImages, sender as BackgroundWorker);
        }

        private void bgwPrincipal_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbProgreso.Value = e.ProgressPercentage;
        }
       

        private void bgwPrincipal_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbProgreso.Visible = false;  // Ocultar la barra de progreso al finalizar
        }
        private void lstImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstImages.SelectedItems.Count > 0)
            {
                // Obtener el nombre del archivo del ítem seleccionado
                string selectedFileName = lstImages.SelectedItems[0].Text;

                // Combinar el path de la imagen con el nombre del archivo
                string filePath = Path.Combine(imagenPath, selectedFileName);

                // Mostrar la imagen
                ShowImagen(filePath);
            }
        }
        private async void cmdPrepare_Click(object sender, EventArgs e)
        {

              try
            {
                await ProcessImagesAsync(imagenPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error durante el procesamiento: " + ex.Message);
            }

        }
        private async void cmdProcess_Click(object sender, EventArgs e)
        {
            pbProgreso.Visible=true;
            pbProgreso.Maximum = lstImages.Items.Count;
            int contador=0;
            foreach (ListViewItem item in lstImages.Items)
            {

                string imagePath = item.Text;  
                await ProcessImage(imagePath);                
                contador++; 
                pbProgreso.Value = contador;
            }
            MessageBox.Show("fin");
        }
        #endregion
        #region UI Setup Methods
        private void SetupListView()
        {
            int maxPages = GetMaxPageConfig();
            foreach (ColumnHeader column in lstActas.Columns)
            {
                column.Width = 60;  // Establece un ancho que contribuya al desbordamiento horizontal
            }


            // Limpiar columnas existentes
            lstActas.Columns.Clear();

            // Añadir columnas fijas
            lstActas.Columns.Add("Est",20); // Ajusta el tamaño según sea necesario
            lstActas.Columns.Add("Código", 50); // Ajusta el tamaño según sea necesario
            lstActas.Columns.Add("Seguridad", 50); // Ajusta el tamaño según sea necesario

            // Añadir columnas dinámicas para cada página
            for (int i = 1; i <= maxPages; i++)
            {
                lstActas.Columns.Add("Pág " + i.ToString(), 50); // Ajusta el tamaño según sea necesario
            }

            // Añadir columnas de total y estado
            lstActas.Columns.Add("Total", 50); // Ajusta el tamaño según sea necesario
            lstActas.Columns.Add("Estado", 100); // Ajusta el tamaño según sea necesario
            lstActas.Scrollable = true;
            lstActas.View = View.Details;
            lstActas.Width = 600;  // Establece un ancho menor para forzar el scroll si es necesario

        }
        #endregion
        #region Business Logic
        private void LoadImageNamesToListView(string directoryPath, ListView listView, BackgroundWorker worker)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
            FileInfo[] files = dirInfo.GetFiles().Where(f => f.Extension.ToLower().EndsWith("tif")).ToArray();

            // Lógica para cargar nombres de archivo, similar a la que proporcioné antes
            int totalFiles = files.Length;
            int processedFiles = 0;

            foreach (FileInfo file in files)
            {
                ListViewItem item = new ListViewItem(file.Name);
                if (listView.IsHandleCreated)
                {
                    listView.Invoke(new MethodInvoker(() => listView.Items.Add(item)));
                }
                else
                {
                    listView.Items.Add(item); // Si el handle aún no está creado, añadir el ítem directamente.
                }

                processedFiles++;
                worker.ReportProgress((processedFiles * 100) / totalFiles);
            }

            MessageBox.Show("ok");
        }
        private void ShowImagen(string imagePath)
        {
            
            Bitmap bitmap = new Bitmap(imagePath);
            picActa.Image = bitmap;           
            picActa.SizeMode = PictureBoxSizeMode.Zoom;
        }
        
        private async Task ProcessImagesAsync(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath, "*.tif");
            int totalFiles = files.Length;
            int processedFiles = 0;
            pbProgreso.Visible = true;
            foreach (var file in files)
            {
                string barcode = await ReadBarcodeAsync(file);
                if (!string.IsNullOrEmpty(barcode))
                {
                    this.Invoke(new Action(() =>
                    {
                        int pagina = 0;
                        Acta acta = ProcessActaData(barcode,file);                     
                        pbProgreso.Value = (int)((double)++processedFiles / totalFiles * 100);
                    }));
                }
            }
            this.Invoke(new Action(() =>
            {
                MessageBox.Show("Todas las imágenes han sido procesadas. Total: " + actas.Count);
                pbProgreso.Visible = false;
            }));
        }


        private async Task<string> ReadBarcodeAsync(string imagePath)
        {
            return await Task.Run(() =>
            {
                using (var bitmap = (Bitmap)Image.FromFile(imagePath))
                {
                    var reader = new BarcodeReader
                    {
                        AutoRotate = true,
                        Options = new DecodingOptions
                        {
                            PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE },
                            TryHarder = true
                        }
                    };
                    
                    var result = reader.Decode(bitmap);
                    if (result != null)
                    {
                        referencia.X = (int)result.ResultPoints[2].X ;
                        referencia.Y = (int)result.ResultPoints[2].Y ;
                    }
                    else
                    {
                        referencia.X = 0;
                        referencia.Y = 0;
                    }
                    return result?.Text;
                }
            });
        }
        
        private async Task CropAndSaveImageAsync(string imagePath)
        {
            await Task.Run(() =>
            {
                using (var bitmap = new Bitmap(imagePath))
                {
                    // Aquí deberías adaptar tu lógica actual de corte de imágenes
                    // El código para cortar imágenes va aquí
                }
            });
        }
        private void UpdateListView(string barcode)
        {
            
            var item = lstActas.FindItemWithText(barcode);
            if (item != null)
            {
                item.SubItems[1].Text = "Procesada";  
            }
        }

        public string GenerateQrContentFromFilename(string filename)
        {
            // Suponiendo que el nombre del archivo viene sin la extensión .tif, si no es así, quitarla
            filename = Path.GetFileNameWithoutExtension(filename);

            // Dividir el nombre del archivo en sus componentes
            string[] parts = filename.Split('_');

            // Extraer las partes según el formato descrito
            int codigoProvincia = int.Parse(parts[0]);
            int codigoCanton = int.Parse(parts[1]);
            int codigoCircunscripcion = int.Parse(parts[2]);
            int codigoParroquia = int.Parse(parts[3]);
            int codigoZona = int.Parse(parts[4]);
            string juntaYSexo = parts[5];
            int codigo = int.Parse(parts[6]);
            string pagina = parts[7];

            // Extraer número de página y total de páginas desde la parte 'P1' o 'P2', etc.
            int numeroPagina = int.Parse(pagina.Substring(1, 1));
            int totalPaginas = 2;  // Asumido basado en tu descripción

            // Formatear la cadena para el QR
            string cabecera = $"{codigo},{12345},{codigoProvincia},{codigoCanton},{codigoParroquia},{codigoZona},{juntaYSexo}";
            string candidatos = GenerateCandidatesStringForPage(numeroPagina); // Método auxiliar para generar la cadena de candidatos
            string paginasInfo = $"{numeroPagina},{totalPaginas}";

            return $"{cabecera}|{candidatos}|{paginasInfo}";
        }
        private string GenerateCandidatesStringForPage(int pageNumber)
        {
            int startCandidate = (pageNumber - 1) * 6 + 1;
            string candidatesString = "";

            for (int i = 0; i < 6; i++)
            {
                if (i > 0) candidatesString += ";";
                int candidateId = startCandidate + i;
                candidatesString += $"{candidateId},{candidateId},{candidateId}";
            }

            return candidatesString;
        }


        private void cropImage(string imagePath)
        {
            string name = Path.GetFileName(imagePath);
            Point guia = new Point();
            int y1;
           // string outputDirectory = txtOrigen.Text + "/SEGMENTO/";
            Bitmap bitmap = new Bitmap(imagePath);
            var reader = new BarcodeReader
            {
                AutoRotate = true,
                TryInverted = true,
                Options = new DecodingOptions
                {
                    PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE },
                    TryHarder = true,
                    ReturnCodabarStartEnd = true,
                    PureBarcode = false
                }
            };


            // Attempt to decode multiple barcodes
            var result = reader.Decode(bitmap);

            if (result != null)
            {
                guia.X = (int)result.ResultPoints[2].X + 600;
                guia.Y = (int)result.ResultPoints[2].Y - 90;
                int alto = 240;
                int ancho = 1490;
                this.Invoke(new Action(() =>
                {
                   //pbSegment.Maximum = 10;
                    //pbSegment.Value = 0;
                }));
                for (int Y = 0; Y < 10; Y++)
                {
                    //aqui se deberia establecer el valor de pbSegment
                    y1 = guia.Y + (Y * alto);
                    Rectangle segmentRect = new Rectangle(guia.X, y1, ancho, alto);

                    using (Bitmap segmentImage = new Bitmap(ancho, alto))
                    {
                        using (Graphics g = Graphics.FromImage(segmentImage))
                        {
                            g.DrawImage(bitmap, new Rectangle(0, 0, ancho, alto), segmentRect, GraphicsUnit.Pixel);
                        }
                        string segmentPath = System.IO.Path.Combine("outputDirectory", $"{name}_s_{Y}.jpeg");
                        segmentImage.Save(segmentPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        //string extractedText = ExtractTextFromImage(segmentPath);
                        //string extractedNumbers = ExtractNumbers(extractedText);
                       // Console.WriteLine($"Segment {Y} Text: {extractedNumbers}");
                    }
                    this.Invoke(new Action(() =>
                    {
                        //pbSegment.Value++;
                    }));

                }
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    //lblCountNoBarcode.Text = (int.Parse(lblCountNoBarcode.Text) + 1).ToString();
                }));

            }

        }
        private async Task ProcessImage(string imagePath)
        {
            try
            {
                string fullPath = Path.Combine(imagenPath, imagePath);
                // Paso 1: Encontrar la posición del QR existente
                var qrPosition = FindQrCodePosition(fullPath);
                if (qrPosition.HasValue)
                {
                    // Paso 2: Borrar el QR existente
                    RemoveQrCode(fullPath, qrPosition.Value);
                }

                // Paso 3: Generar nuevo contenido QR basado en el nombre del archivo
                string newQrContent = GenerateQrContentFromFilename(Path.GetFileName(fullPath));

                // Paso 4: Añadir nuevo QR a la imagen
                AddQrCodeToImage(fullPath, newQrContent, new Point(qrPosition?.X ?? 0, qrPosition?.Y ?? 0)); // Asumiendo que queremos colocarlo en el mismo lugar
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar la imagen: {ex.Message}");
            }
        }
        private Rectangle? FindQrCodePosition(string imagePath)
        {
            try {


                Bitmap bitmap = new Bitmap(imagePath);
                var reader = new BarcodeReader
                {
                    AutoRotate = true,
                    TryInverted = true,
                    Options = new DecodingOptions
                    {
                        PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE },
                        TryHarder = true,
                        ReturnCodabarStartEnd = true,
                        PureBarcode = false
                    }
                };
                var result = reader.Decode(bitmap);

                if (result != null)
                {
                    return new Rectangle((int)result.ResultPoints[0].X-27, (int)result.ResultPoints[0].Y-220,
                                             250,
                                             250);
                    
                };


                return null;
            }
            catch (Exception e){
                MessageBox.Show(e.ToString());
                return null;
            }
                
            
        }

        private void RemoveQrCode(string imagePath, Rectangle qrRect)
        {
            string tempPath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + "_temp" + Path.GetExtension(imagePath));

            // Crea un Bitmap temporal directamente desde el archivo para evitar bloqueo
            using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                using (var originalBitmap = new Bitmap(fs))
                {
                    using (var cloneBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height, PixelFormat.Format24bppRgb))
                    {
                        using (var cloneGraphics = Graphics.FromImage(cloneBitmap))
                        {
                            cloneGraphics.DrawImage(originalBitmap, new Rectangle(0, 0, cloneBitmap.Width, cloneBitmap.Height));
                            cloneGraphics.FillRectangle(Brushes.White, qrRect); // Asumiendo fondo blanco
                        }

                        // Guarda en un archivo temporal primero
                        cloneBitmap.Save(tempPath, ImageFormat.Tiff);
                    }
                }
            }

            // Asegúrate de que todos los recursos están liberados y el archivo no está bloqueado
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Reemplazar el archivo original con el archivo temporal
            System.IO.File.Delete(imagePath);
            System.IO.File.Move(tempPath, imagePath);
        }






        private void AddQrCodeToImage(string imagePath, string qrContent, Point position)
        {
            var tempPath = Path.Combine(Path.GetDirectoryName(imagePath), Path.GetFileNameWithoutExtension(imagePath) + "_temp" + Path.GetExtension(imagePath));

            // Usar FileStream para abrir la imagen y evitar que Image.FromFile bloquee el archivo.
            using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                using (var sourceImage = (Bitmap)Image.FromStream(fs))
                {
                    using (var cloneBitmap = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format24bppRgb))
                    {
                        using (var graphics = Graphics.FromImage(cloneBitmap))
                        {
                            graphics.DrawImage(sourceImage, new Rectangle(0, 0, cloneBitmap.Width, cloneBitmap.Height));
                            var qrCodeImage = GenerateQrCode(qrContent, 250, 250); // Generar el QR
                            graphics.DrawImage(qrCodeImage, new Rectangle(position.X, position.Y, qrCodeImage.Width, qrCodeImage.Height));
                        }

                        // Guardar en un archivo temporal
                        cloneBitmap.Save(tempPath, ImageFormat.Tiff);
                    }
                }
            }

            // Asegurar que los recursos estén completamente liberados antes de manipular el archivo
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Eliminar el archivo original y mover el temporal a su lugar
            System.IO.File.Delete(imagePath);
            System.IO.File.Move(tempPath, imagePath);
        }



        private Bitmap GenerateQrCode(string content, int width, int height)
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = 0
                }
            };
            var pixelData = writer.Write(content);

            var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            try
            {
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            return bitmap;
        }
        private Acta ProcessActaData(string data, string imagePath)
        {
            string[] parts = data.Split('|');
            string[] headerParts = parts[0].Split(',');
            string[] candidatosParts = parts[1].Split(';');
            string[] pageInfo = parts[2].Split(',');

            int codigo = int.Parse(headerParts[0]);
            int maxPages =int.Parse(pageInfo[1]);
            ListViewItem item;
            // Buscar o crear nueva Acta
            Acta acta = actas.FirstOrDefault(a => a.Codigo == codigo);
            if (acta == null)
            {
                acta = new Acta
                {
                    Codigo = codigo,
                    Seguridad = int.Parse(headerParts[1]),
                    Provincia = int.Parse(headerParts[2]),
                    Canton = int.Parse(headerParts[3]),
                    Parroquia = int.Parse(headerParts[4]),
                    Zona = int.Parse(headerParts[5]),
                    Junta = int.Parse(headerParts[6].Substring(0, headerParts[6].Length - 1)),
                    Sexo = headerParts[6].Last().ToString(),
                    Pagina = int.Parse(pageInfo[0]),
                    Paginas = int.Parse(pageInfo[1]),
                    Estado = 0,
                    paginas = new List<Pagina>()
                };
                actas.Add(acta);

                AddNewItemToListView(acta);
            }
            

            // Asegurarse de que las páginas se manejen correctamente
            int numeroPagina = int.Parse(pageInfo[0]);
            Pagina pagina = acta.paginas.FirstOrDefault(p => p.Numero == numeroPagina);           
            if (pagina == null)
            {
                pagina = new Pagina
                {
                    ActaId = acta.Codigo,
                    Numero = numeroPagina,
                    Estado = 0,
                    Path = imagePath,
                    Referencia =referencia,
                    candidatos = new List<Candidato>()                    
                };
                acta.paginas.Add(pagina);
                UpdateListViewForNewPage(acta, pagina);
            }

            // Procesar candidatos
            foreach (string candidato in candidatosParts)
            {
                string[] candidatoInfo = candidato.Split(',');
                Candidato newCandidato = new Candidato
                {
                    Id = int.Parse(candidatoInfo[0]),
                    Orden = int.Parse(candidatoInfo[1]),
                    PatidoId = int.Parse(candidatoInfo[2]),
                    VotosIa = 0,
                    Estado = 0
                };
                pagina.candidatos.Add(newCandidato);
            }
            CortarCandidatos(pagina);
            return acta;
        }
        private void CortarCandidatos(Pagina pagina)
        {
            string imagePath = pagina.Path;  
            Point referencia = pagina.Referencia;  
            try
            {
                using (Bitmap bitmap = new Bitmap(imagePath))
                {
                    int startX = referencia.X + 610;  
                    int startY = referencia.Y - 93;  

                    for (int i = 0; i < pagina.candidatos.Count; i++)
                    {
                        int y = startY + (i * 240); 
                        Rectangle cropArea = new Rectangle(startX, y, 1490, 240);

                        using (Bitmap candidateBitmap = bitmap.Clone(cropArea, bitmap.PixelFormat))
                        {
                            string folderPath = Path.Combine(actaPath, $"{pagina.ActaId}");
                            Directory.CreateDirectory(folderPath);

                            string outputFilename = Path.Combine(actaPath, $"{pagina.ActaId}/{pagina.ActaId}_{pagina.candidatos[i].Id}.tif");
                            candidateBitmap.Save(outputFilename, System.Drawing.Imaging.ImageFormat.Tiff);

                            Candidato candidatoToUpdate = pagina.candidatos.FirstOrDefault(c => c.Id == pagina.candidatos[i].Id);
                            if (candidatoToUpdate != null)
                            {
                                candidatoToUpdate.Path = outputFilename;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar imagen: " + ex.Message);
            }
        }


        private void AddNewItemToListView(Acta acta)
        {

            this.Invoke(new Action(() =>
            {
                ListViewItem item = new ListViewItem
                {
                    ImageIndex = 0
                };

                item.SubItems.Add(acta.Codigo.ToString());
                item.SubItems.Add(acta.Seguridad.ToString());
                for (int i = 1; i <= GetMaxPageConfig(); i++)
                {
                    item.SubItems.Add("0");  // Inicializar con "0"
                }

                lstActas.Items.Add(item);
            }));
        }


        private void UpdateListViewForNewPage(Acta acta, Pagina pagina)
        {
            lstActas.Invoke(new Action(() =>
            {
                ListViewItem item = lstActas.Items.Cast<ListViewItem>().FirstOrDefault(it => it.SubItems[1].Text == acta.Codigo.ToString());
                if (item != null)
                {
                    int subItemIndex = pagina.Numero + 2; 
                    while (item.SubItems.Count <= subItemIndex)
                    {
                        item.SubItems.Add("0");
                    }

                    item.SubItems[subItemIndex].Text = "X";
                    if (IsActaComplete(acta))
                    {
                        item.ImageIndex = 1; 
                    }
                    lstActas.Refresh();
                }
            }));
        }

        private bool IsActaComplete(Acta acta)
        {
            int maxPages = acta.Paginas;
            return acta.paginas.Count == maxPages && acta.paginas.All(p => p.Numero >= 1 && p.Numero <= maxPages);
        }


        #endregion


    }
}
