using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using AuxiliarKinect.FuncoesBasicas;

namespace ImageByEvent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public KinectSensor Kinect {private set; get; }

        public MainWindow()
        {
            InitializeComponent();
            InicializarKinect();
        }

        public void Drag_Completed(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs evt)
        {
            Kinect.ElevationAngle = Convert.ToInt32(slider.Value);
            eixoValor.Content = Kinect.ElevationAngle;

        }

        private void InicializarKinect()
        {
            Kinect = InicializadorKinect.InicializarPrimeiroSensor(0);
            Kinect.Start();
            Kinect.ColorStream.Enable(ColorImageFormat.InfraredResolution640x480Fps30);
            Kinect.ColorFrameReady += Kinect_ColorFrameReady;
        }

        private void Kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
          ColorImageFrame quadro = e.OpenColorImageFrame();
          imageKinect.Source = ObterImagemRGB(quadro);
        }

        private ImageSource ObterImagemRGB(ColorImageFrame quadro)
        {
            if (quadro == null) return null;
            using (quadro)
            {
                byte[] bytesImagem = new byte[quadro.PixelDataLength];
                quadro.CopyPixelDataTo(bytesImagem);
                return BitmapSource.Create(quadro.Width, quadro.Height,
                96, 96, PixelFormats.Gray16, null, bytesImagem,
                quadro.Width * quadro.BytesPerPixel);
            }  
        }

    }
}
