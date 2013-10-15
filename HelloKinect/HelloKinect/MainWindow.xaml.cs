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

namespace HelloKinect
{
    public partial class MainWindow : Window
    {

        bool MaoDireitaAcimaCabeca;

        public MainWindow()
        {
            InitializeComponent();
            inicializarKinect();
        }

        private void inicializarKinect()
        {
            int anguloInicial = 0;
            KinectSensor kinect=  InicializadorKinect.InicializarPrimeiroSensor(anguloInicial);
            kinect.SkeletonStream.Enable();
        }
    }
}
