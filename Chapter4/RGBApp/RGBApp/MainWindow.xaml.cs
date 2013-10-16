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

namespace RGBApp
{
    
    public partial class MainWindow : Window
    {
        public KinectSensor Kinect { private set; get; }
        public MainWindow()
        {
            InitializeComponent();
            InicializarKinect();
        }

        private void bater_foto(object sender, RoutedEventArgs e)
        {
            ColorImageFrame instantFrame = Kinect.ColorStream.OpenNextFrame(0);
            imagemKinect.Source = ObterImagemSensorRGB(instantFrame);
        }

        private void InicializarKinect()
        {
            int anguloInicial = 0;
            Kinect = InicializadorKinect.InicializarPrimeiroSensor(anguloInicial);
            Kinect.Start();
            Kinect.ColorStream.Enable();
        }

        private BitmapSource ObterImagemSensorRGB(ColorImageFrame quadro)
        {
            using (quadro)
            {
                byte[] bytesImagem = new byte[quadro.PixelDataLength];
                quadro.CopyPixelDataTo(bytesImagem);
                int dpiX = 96;
                int dpiY = 96;
                int strideImage = quadro.Width * quadro.BytesPerPixel;
                return BitmapSource.Create(quadro.Width, quadro.Height,
                dpiX, dpiY, PixelFormats.Bgr32, null, bytesImagem,
                strideImage);
            }
        }
    }
}
