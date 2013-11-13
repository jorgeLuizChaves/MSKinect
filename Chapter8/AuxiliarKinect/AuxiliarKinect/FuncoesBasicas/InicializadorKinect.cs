using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;

namespace AuxiliarKinect.FuncoesBasicas
{
    public class InicializadorKinect
    {
        public KinectSensorChooser SeletorKinect { private set; get; }
        public Action<KinectSensor> MetodoInicializadorKinect { get; set; }

        public InicializadorKinect()
        {
            SeletorKinect = new KinectSensorChooser();
            SeletorKinect.KinectChanged +=SeletorKinect_KinectChanged;
            SeletorKinect.Start();
        }

        [Obsolete]
        public static KinectSensor InicializarPrimeiroSensor(int anguloInicial)
        {
            KinectSensor kinectSensor = KinectSensor.KinectSensors.First(sensor => sensor.Status == KinectStatus.Connected);
            kinectSensor.Start();
            kinectSensor.ElevationAngle = anguloInicial;
            return kinectSensor;
        }

        private void SeletorKinect_KinectChanged(object sender, KinectChangedEventArgs kinectArgs)
        {
            if (kinectArgs.OldSensor != null)
            {
                try
                {
                    if (kinectArgs.OldSensor.DepthStream.IsEnabled)
                        kinectArgs.OldSensor.DepthStream.Disable();
                    if (kinectArgs.OldSensor.SkeletonStream.IsEnabled)
                        kinectArgs.OldSensor.SkeletonStream.Disable();
                    if (kinectArgs.OldSensor.ColorStream.IsEnabled)
                        kinectArgs.OldSensor.ColorStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // Captura exceção caso o KinectSensor entre
                    // em um estado inválido durante a desabilitação
                    // de um fluxo.
                }
            }
            if (kinectArgs.NewSensor != null)
            {
                if (MetodoInicializadorKinect != null)
                    MetodoInicializadorKinect(SeletorKinect.Kinect);
            }
        }
    }
}