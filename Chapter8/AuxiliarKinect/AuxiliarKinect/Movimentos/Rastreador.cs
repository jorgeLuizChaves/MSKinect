using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos
{
    public class Rastreador<T> : IRastreador where T : Movimento, new()
    {
        private T movimento;
        public EstadoRastreamento EstadoAtual { get; private set; }
        public event EventHandler MovimentoIdentificado;
        public event EventHandler MovimentoEmProgresso;

        public Rastreador()
        {
            EstadoAtual = EstadoRastreamento.NAO_IDENTIFICADO;
            movimento = Activator.CreateInstance<T>();
        }


        public void Rastrear(Skeleton esqueleto)
        {
            EstadoRastreamento novoEstado =  movimento.Rastrear(esqueleto);
            if(novoEstado.IsIdentificado() && !EstadoAtual.IsIdentificado() )
            {
                ChamarEvento(MovimentoIdentificado);
            }

            if (novoEstado.IsEmExecucao() && (EstadoAtual.IsEmExecucao() || EstadoAtual.IsNaoIdentificado()))
            {
                ChamarEvento(MovimentoEmProgresso);
            }
            EstadoAtual = novoEstado;
        }

        private void ChamarEvento(EventHandler evento)
        {
            if (evento != null)
                evento(movimento, new EventArgs());
        }
    }
}
