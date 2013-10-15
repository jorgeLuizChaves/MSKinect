using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace AuxiliarKinect.FuncoesBasicas
{
    public class InicializadorKinect
    {
        public static KinectSensor InicializarPrimeiroSensor(int anguloInicial)
        {
            KinectSensor kinectSensor = KinectSensor.KinectSensors.First(sensor => sensor.Status == KinectStatus.Connected);
            kinectSensor.Start();
            kinectSensor.ElevationAngle = anguloInicial;
            return kinectSensor;
        }
    }
}