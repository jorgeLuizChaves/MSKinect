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
using System.Windows.Controls.Primitives;

namespace EixoMotorizado
{
    public partial class MainWindow : Window
    {
        public KinectSensor Kinect { private set; get; }
        public MainWindow()
        {
            InitializeComponent();
            InicializarKinect();
        }

        public void slider_DragCompleted(object sender,DragCompletedEventArgs e)
        {
            AtualizarValores();
        }

        private void InicializarKinect()
        {
            int anguloInicial = 0;
           Kinect = InicializadorKinect.InicializarPrimeiroSensor(anguloInicial);
        }

        private void AtualizarValores()
        {
            Kinect.ElevationAngle = Convert.ToInt32(slider.Value);
            label.Content = Kinect.ElevationAngle;
        }


    }
}
