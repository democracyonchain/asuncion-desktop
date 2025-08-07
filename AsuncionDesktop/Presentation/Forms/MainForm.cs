using System;
using System.Windows.Forms;
using AsuncionDesktop.Application.UseCases;
using AsuncionDesktop.Domain.Interfaces;
using AsuncionDesktop.Infrastructure.Services;


namespace AsuncionDesktop.Presentation.Forms
{
    public partial class MainForm : Form
    {
        private readonly ScanDocumentUseCase _scanDocumentUseCase;
        private readonly ProcessImagesUseCase _processImagesUseCase;
        private readonly IDirectoryService _directoryService;

        public MainForm()
        {
            InitializeComponent();

            // Inicializa los servicios necesarios para ScanForm
            _directoryService = new DirectoryService();
            var imageService = new ImageService();
            var qrCodeService = new QRCodeService();
            var fileService = new FileService();

            _scanDocumentUseCase = new ScanDocumentUseCase(imageService, qrCodeService);
            _processImagesUseCase = new ProcessImagesUseCase(imageService, qrCodeService, _directoryService, fileService);
        }

        private void unoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // ✅ Pasar correctamente las dependencias a ScanForm
            ScanForm scanForm = new ScanForm(_scanDocumentUseCase, _processImagesUseCase, _directoryService);
            scanForm.MdiParent = this;
            scanForm.Show();
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
