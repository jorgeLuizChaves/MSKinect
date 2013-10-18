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
            Kinect.DepthStream.Enable();
            Kinect.SkeletonStream.Enable();
            Kinect.DepthFrameReady +=Kinect_DepthFrameReady;
        }

        private void Kinect_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs depthImageEvent)
        {
            imageKinect.Source = ReconhecerHumanos(depthImageEvent.OpenDepthImageFrame());
        }

        private BitmapSource ReconhecerHumanos(DepthImageFrame quadro)
        {
            if (quadro == null) return null;
            using (quadro)
            {
                DepthImagePixel[] imagemProfundidade =
                new DepthImagePixel[quadro.PixelDataLength];
                quadro.CopyDepthImagePixelDataTo(imagemProfundidade);
                byte[] bytesImagem = new byte[imagemProfundidade.Length * 4];
                for (int indice = 0; indice < bytesImagem.Length; indice += 4)
                {
                    if (imagemProfundidade[indice / 4].PlayerIndex != 0)
                    {
                        bytesImagem[indice + 1] = 255;
                    }
                }
                return BitmapSource.Create(quadro.Width, quadro.Height,
                96, 96, PixelFormats.Bgr32, null, bytesImagem,
                quadro.Width * 4);
            } 
        }
    }
}
