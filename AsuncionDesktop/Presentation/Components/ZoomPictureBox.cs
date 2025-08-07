using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsuncionDesktop.Presentation.Components
{   
   public class ZoomPictureBox : PictureBox
    {
        private float _zoomFactor = 1.0f;
        private const float ZoomIncrement = 0.1f;
        private Point _imagePosition = Point.Empty;

        public ZoomPictureBox()
        {
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.MouseWheel += ZoomPictureBox_MouseWheel;
            this.MouseDown += ZoomPictureBox_MouseDown;
            this.MouseMove += ZoomPictureBox_MouseMove;
        }

        private void ZoomPictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Image == null) return;

            if (e.Delta > 0)
                _zoomFactor += ZoomIncrement;
            else if (e.Delta < 0 && _zoomFactor > ZoomIncrement)
                _zoomFactor -= ZoomIncrement;

            Invalidate();
        }

        private Point _lastMousePosition;
        private bool _dragging = false;

        private void ZoomPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _dragging = true;
                _lastMousePosition = e.Location;
            }
        }

        private void ZoomPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                _imagePosition.X += e.X - _lastMousePosition.X;
                _imagePosition.Y += e.Y - _lastMousePosition.Y;
                _lastMousePosition = e.Location;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (Image != null)
            {
                pe.Graphics.Clear(this.BackColor);
                int newWidth = (int)(Image.Width * _zoomFactor);
                int newHeight = (int)(Image.Height * _zoomFactor);

                pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                pe.Graphics.DrawImage(Image, _imagePosition.X, _imagePosition.Y, newWidth, newHeight);
            }
        }
    }
}
