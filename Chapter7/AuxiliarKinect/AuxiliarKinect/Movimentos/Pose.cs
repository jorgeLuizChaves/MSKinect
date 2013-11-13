using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos
{
    public abstract class Pose : Movimento
    {
        protected int QuadroIdentificacao { get; set; }

        public int PercentualProgresso
        {
            get
            {
                return ContadorQuadros * 100 / QuadroIdentificacao;
            }
        }

        public override EstadoRastreamento Rastrear(Skeleton esqueletoUsuario)
        {
            EstadoRastreamento novoEstado;
            if (esqueletoUsuario != null && PosicaoValida(esqueletoUsuario))
            {
                if (QuadroIdentificacao == ContadorQuadros)
                    novoEstado = EstadoRastreamento.IDENTIFICADO;
                else
                {
                    novoEstado = EstadoRastreamento.EM_EXECUCAO;
                    ContadorQuadros += 1;
                }
            }
            else
            {
                novoEstado = EstadoRastreamento.NAO_IDENTIFICADO;
                ContadorQuadros = 0;
            }
            return novoEstado;
        }

    }
}
