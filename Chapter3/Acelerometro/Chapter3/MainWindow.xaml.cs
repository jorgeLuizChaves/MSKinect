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
using System.Windows.Threading;

namespace Chapter3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public KinectSensor Kinect {private set; get;}

        public MainWindow()
        {
            InitializeComponent();
            InicializarKinect();
            InicializarTimer();
        }

        public void InicializarKinect()
        {
            int anguloInicial = 0;
            Kinect = InicializadorKinect.InicializarPrimeiroSensor(anguloInicial);
        }

        private void AtualizarValoresAcelerometro()
        {
            Vector4 posicoesAcelerometro = Kinect.AccelerometerGetCurrentReading();
            labelX.Content = Math.Round(posicoesAcelerometro.X, 3);
            labelY.Content = Math.Round(posicoesAcelerometro.Y, 3);
            labelZ.Content = Math.Round(posicoesAcelerometro.Z, 3);
        }

        private void InicializarTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            AtualizarValoresAcelerometro();
        }

    }
}
