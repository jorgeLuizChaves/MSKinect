using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos
{
    public interface IRastreador
    {
        EstadoRastreamento EstadoAtual { get; }
        void Rastrear(Skeleton esqueleto);
    }
}
