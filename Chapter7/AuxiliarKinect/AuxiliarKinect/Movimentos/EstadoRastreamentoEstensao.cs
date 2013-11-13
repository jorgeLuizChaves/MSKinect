using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliarKinect.Movimentos
{
    public static class EstadoRastreamentoEstensao
    {
        public static bool IsIdentificado(this EstadoRastreamento estado)
        {
            return estado == EstadoRastreamento.IDENTIFICADO;
        }

        public static bool IsNaoIdentificado(this EstadoRastreamento estado)
        {
            return estado == EstadoRastreamento.NAO_IDENTIFICADO;
        }

        public static bool IsEmExecucao(this EstadoRastreamento estado)
        {
            return estado == EstadoRastreamento.EM_EXECUCAO;
        }
    }
}