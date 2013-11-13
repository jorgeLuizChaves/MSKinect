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
using Microsoft.Kinect.Toolkit;
using AuxiliarKinect.Movimentos;
using AuxiliarKinect.Movimentos.Poses;
using ImageByEvent.Auxiliar;
using Microsoft.Kinect.Toolkit.Interaction;

namespace ImageByEvent
{
    public partial class MainWindow : Window
    {
        public KinectSensor Kinect {private set; get; }
        public List<IRastreador> Rastreadores { private set; get; }
        InteractionStream fluxoInteracao;
        ConfiguracaoDesenho configuracaoMaoDireita;
        ConfiguracaoDesenho configuracaoMaoEsquerda;

        public MainWindow()
        {
            InitializeComponent();
            InicializarIniciador();
            InicializarRastreadores();
            InicializarFluxoInteracao();
            InicializarConfiguracoesDesenho();
            
            //Testando o kinect com interactions - TODO REFACTOR PARA ACENO.
            KinectRegion.KinectSensor = Kinect;
        }

        public void Drag_Completed(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs evt)
        {
            Kinect.ElevationAngle = Convert.ToInt32(slider.Value);
            eixoValor.Content = Kinect.ElevationAngle;

        }

        private void InicializarIniciador()
        {
            InicializadorKinect inicializador = new InicializadorKinect();
            inicializador.MetodoInicializadorKinect += InicializarKinect;
            kinectChooserUI.KinectSensorChooser = inicializador.SeletorKinect;
            InicializarKinect(inicializador.SeletorKinect.Kinect);
        }

        private void InicializarKinect(KinectSensor kinect)
        {
            Kinect = kinect;
            Kinect.Start();
            Kinect.DepthStream.Enable();
            Kinect.ColorStream.Enable();
            Kinect.SkeletonStream.Enable();
            Kinect.AllFramesReady +=Kinect_AllFramesReady;
        }

        private void Kinect_AllFramesReady(object sender, AllFramesReadyEventArgs allFrameEvent)
        {
            byte[] imagem = ObterImagemSensorRGB(allFrameEvent.OpenColorImageFrame());

            FuncoesProfundidade(allFrameEvent.OpenDepthImageFrame(), imagem, 2000);
            if (imagem != null)
            {
                 canvasKinect.Background = new ImageBrush(BitmapSource.Create(Kinect.ColorStream.FrameWidth,
                               Kinect.ColorStream.FrameHeight,
                               96, 96, PixelFormats.Bgr32, null,
                               imagem, Kinect.ColorStream.FrameBytesPerPixel * Kinect.ColorStream.FrameWidth));
            }

            canvasKinect.Children.Clear();
            FuncoesEsqueletoUsuario(allFrameEvent.OpenSkeletonFrame());
        }

        private void FuncoesEsqueletoUsuario(SkeletonFrame quadro)
        {
            if (quadro == null) return;
            using (quadro)
            {
                Skeleton primeiroUsuario = quadro.ObterEsqueletoPrimeiroUsuario();

                if (btnDesenhar.IsChecked)
                {
                    DesenharEmTela(quadro, primeiroUsuario);
                }
                else
                {

                    foreach(IRastreador rastreador in Rastreadores)
                    {
                        rastreador.Rastrear(primeiroUsuario);
                    }

                    if (IsDesenhaEsqueleto())
                    {
                        quadro.DesenharEsqueletoUsuario(Kinect, canvasKinect);
                    }
                }
            }
        }

        private void DesenharEmTela(SkeletonFrame quadro, Skeleton primeiroUsuario)
        {
            Skeleton[] esqueletos = new Skeleton[6];
            quadro.CopySkeletonDataTo(esqueletos);

            fluxoInteracao.ProcessSkeleton(esqueletos, Kinect.AccelerometerGetCurrentReading(), quadro.Timestamp);
            EsqueletoUsuarioAuxiliar esqueletoAuxiliar = new EsqueletoUsuarioAuxiliar(Kinect);

            if (configuracaoMaoDireita.DesenhoAtivo)
                esqueletoAuxiliar.InteracaoDesenhar(primeiroUsuario.Joints[JointType.HandRight], canvasDesenho, configuracaoMaoDireita);
            if (configuracaoMaoEsquerda.DesenhoAtivo)
                esqueletoAuxiliar.InteracaoDesenhar(primeiroUsuario.Joints[JointType.HandLeft], canvasDesenho, configuracaoMaoEsquerda);
        }

        private bool IsDesenhaEsqueleto()
        {
            return (chkEsqueleto.IsChecked);
        }

        private static bool hasSkeletonTracked(IEnumerable<Skeleton> esqueletosRastreados)
        {
            return esqueletosRastreados.Count() > 0;
        }

        private byte[] ObterImagemSensorRGB(ColorImageFrame quadro)
        {
            if (quadro == null)
                return null;

            using (quadro)
            {
                byte[] bytesImagem = new byte[quadro.PixelDataLength];
                quadro.CopyPixelDataTo(bytesImagem);
                return bytesImagem;
            }
        }

        private void FuncoesProfundidade(DepthImageFrame quadro, byte[] bytesImagem, int maxDistancia)
        {
            if (quadro == null || bytesImagem == null)
                return;
            
                using (quadro)
                {
                    DepthImagePixel[] imagemProfundidade = new DepthImagePixel[quadro.PixelDataLength];
                    quadro.CopyDepthImagePixelDataTo(imagemProfundidade);

                    if (btnDesenhar.IsChecked)
                    {
                        fluxoInteracao.ProcessDepth(imagemProfundidade, quadro.Timestamp);
                    }
                    else if (chkEscalaCinza.IsChecked)
                    {
                        ReconhecerProfundidade(bytesImagem, maxDistancia, imagemProfundidade);
                    }
                }
        }

        private void ReconhecerProfundidade(byte[] bytesImagem, int maxDistancia, DepthImagePixel[] imagemProfundidade)
        {
            DepthImagePoint[] pontosImagemProfundidade = new DepthImagePoint[640 * 480];

            Kinect.CoordinateMapper.MapColorFrameToDepthFrame(Kinect.ColorStream.Format,
                                          Kinect.DepthStream.Format, imagemProfundidade,
                                          pontosImagemProfundidade);

            for (int count = 0; count < pontosImagemProfundidade.Length; count++)
            {
                var point = pontosImagemProfundidade[count];
                if (point.Depth < maxDistancia && KinectSensor.IsKnownPoint(point))
                {
                    var pixelDataIndex = count * 4;
                    byte maiorValorCor =
                    Math.Max(bytesImagem[pixelDataIndex],
                    Math.Max(bytesImagem[pixelDataIndex + 1],
                    bytesImagem[pixelDataIndex + 2]));
                    bytesImagem[pixelDataIndex] = maiorValorCor;
                    bytesImagem[pixelDataIndex + 1] = maiorValorCor;
                    bytesImagem[pixelDataIndex + 2] = maiorValorCor;
                }
            }
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

        private void InicializarRastreadores()
        {
            Rastreadores = new List<IRastreador>();

            Rastreador<PoseT> rastreadorPoseT = new Rastreador<PoseT>();
            rastreadorPoseT.MovimentoIdentificado += PoseTIdentificada;

            Rastreador<PosePausa> rastreadorPausa = new Rastreador<PosePausa>();
            rastreadorPausa.MovimentoIdentificado += PosePausaIdentificada;
            rastreadorPausa.MovimentoEmProgresso += ProgressoBarraPose;

            Rastreadores.Add(rastreadorPoseT);
            Rastreadores.Add(rastreadorPausa);
        }

        private void PoseTIdentificada(object sender, EventArgs e)
        {
            chkEsqueleto.IsChecked = !chkEsqueleto.IsChecked;
        }

        private void PosePausaIdentificada(object sender, EventArgs e)
        {
            chkEscalaCinza.IsChecked = !chkEscalaCinza.IsChecked;
        }

        private void AcenoIdentificado(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ProgressoBarraPose(object sender, EventArgs e)
        {
            PosePausa pose = sender as PosePausa;

            Rectangle retangulo = new Rectangle();
            retangulo.Width = canvasKinect.ActualWidth;
            retangulo.Height = 20;
            retangulo.Fill = Brushes.Black;

            Rectangle poseRetangulo = new Rectangle();
            poseRetangulo.Width = canvasKinect.ActualWidth * pose.PercentualProgresso / 100;
            poseRetangulo.Height = 20;
            poseRetangulo.Fill = Brushes.BlueViolet;

            canvasKinect.Children.Add(retangulo);
            canvasKinect.Children.Add(poseRetangulo);
        }

        private void AcenoIndentificado(object sender, EventArgs e)
        {
            if (KinectRegion.KinectSensor == null)
                KinectRegion.KinectSensor = Kinect;
        }

        private void btnVoltarClick(object sender, RoutedEventArgs e)
        {
            if (KinectRegion.KinectSensor != null)
                KinectRegion.KinectSensor = null;
        }

        private void btnFecharClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnDesenharClick(object sender, RoutedEventArgs e)
        {
            if (!btnDesenhar.IsChecked)
                canvasDesenho.Children.Clear();
        }

        private void InicializarConfiguracoesDesenho()
        {
            configuracaoMaoDireita = new ConfiguracaoDesenho();
            configuracaoMaoDireita.Cor = Brushes.Red;
            configuracaoMaoDireita.Forma = FormaDesenho.Elipse;
            configuracaoMaoDireita.Tamanho = 20;

            configuracaoMaoEsquerda = new ConfiguracaoDesenho();
            configuracaoMaoEsquerda.Cor = Brushes.Blue;
            configuracaoMaoEsquerda.Forma = FormaDesenho.Retangulo;
            configuracaoMaoEsquerda.Tamanho = 20;
        }

        private void InicializarFluxoInteracao()
        {
            fluxoInteracao =
            new InteractionStream(Kinect, canvasDesenho);
            fluxoInteracao.InteractionFrameReady += fluxoInteracao_InteractionFrameReady;
        }

        private void fluxoInteracao_InteractionFrameReady(object sender, InteractionFrameReadyEventArgs e)
        {
            using (InteractionFrame quadro = e.OpenInteractionFrame())
            {
                if (quadro == null) return;
                UserInfo[] informacoesUsuarios = new UserInfo[6];
                quadro.CopyInteractionDataTo(informacoesUsuarios);
                IEnumerable<UserInfo> usuariosRastreados = informacoesUsuarios.Where(info => info.SkeletonTrackingId != 0);
                if (usuariosRastreados.Count() > 0)
                {
                    UserInfo usuarioPrincipal = usuariosRastreados.First();
                    if (usuarioPrincipal.HandPointers[0].HandEventType == InteractionHandEventType.Grip)
                        configuracaoMaoDireita.DesenhoAtivo = true;
                    else if (usuarioPrincipal.HandPointers[0].HandEventType == InteractionHandEventType.GripRelease)
                        configuracaoMaoEsquerda.DesenhoAtivo = false;

                    if (usuarioPrincipal.HandPointers[1].HandEventType == InteractionHandEventType.Grip)
                        configuracaoMaoDireita.DesenhoAtivo = true;
                    else if (usuarioPrincipal.HandPointers[1].HandEventType == InteractionHandEventType.GripRelease)
                        configuracaoMaoEsquerda.DesenhoAtivo = false;
                }
            }
        }
    }
}