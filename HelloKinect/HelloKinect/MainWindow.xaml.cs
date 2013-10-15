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
        private const int MAX_SKELETON = 6;

        public MainWindow()
        {
            InitializeComponent();
            inicializarKinect();
        }

        private void inicializarKinect()
        {
            int anguloInicial = 0;
            KinectSensor kinect = InicializadorKinect.InicializarPrimeiroSensor(anguloInicial);
            kinect.SkeletonStream.Enable();
            kinect.SkeletonFrameReady += kinect_SkeletonEvent;
        }

        private void kinect_SkeletonEvent(object sender, SkeletonFrameReadyEventArgs e)
        {
           using(SkeletonFrame quadroAtual =  e.OpenSkeletonFrame())
           {
               if (quadroAtual != null)
               {
                   executarRegraMaoDireitaEmcimaDaCabeca(quadroAtual);
               }
           }
        }

        private void executarRegraMaoDireitaEmcimaDaCabeca(SkeletonFrame quadroAtual)
        {
            Skeleton[] esqueletos = new Skeleton[MAX_SKELETON];
            quadroAtual.CopySkeletonDataTo(esqueletos);
            Skeleton usuario = esqueletos.FirstOrDefault(esqueleto =>
                                                            esqueleto.TrackingState == SkeletonTrackingState.Tracked);

            if (HasUsuario(usuario))
            {
                Joint maoDireita = usuario.Joints[JointType.HandRight];
                Joint cabeca = usuario.Joints[JointType.Head];
                bool novoTesteMaoDireitaAcimaCabeca = IsMaoDireitaAcimaDaCabeca(maoDireita.Position.Y, cabeca.Position.Y);
                if (MaoDireitaAcimaCabeca != novoTesteMaoDireitaAcimaCabeca)
                {
                    MaoDireitaAcimaCabeca = novoTesteMaoDireitaAcimaCabeca;
                    if (MaoDireitaAcimaCabeca)
                        MessageBox.Show("A mão direita está acima da cabeça!");
                }
            }
        }

        private bool HasUsuario(Skeleton usuario)
        {
            return usuario != null;
        }

        public bool IsMaoDireitaAcimaDaCabeca(float maoDireitaPosicaoY, float cabecaPosicaoY)
        {
            return (maoDireitaPosicaoY > cabecaPosicaoY);
        }
    }
}
