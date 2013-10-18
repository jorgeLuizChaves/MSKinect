using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;

namespace ImageByEvent
{
    public class EsqueletoUsuarioAuxiliar
    {
        public KinectSensor Kinect{private set; get;}
        public EsqueletoUsuarioAuxiliar(KinectSensor kinect)
        {
            this.Kinect = kinect;
        }
    }
}
