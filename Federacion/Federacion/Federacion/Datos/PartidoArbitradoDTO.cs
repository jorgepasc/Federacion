using System;
using System.Collections.Generic;
using System.Text;

namespace Federacion.Datos
{
    class PartidoArbitradoDTO
    {
        private long _IdPartidoArbitrado;
        private long _IdPartido;
        private long _IdArbitro;
        private String _Funcion;
        private decimal _Importe;

        public long IdPartidoArbitrado { get; set; }
        public long IdPartido { get; set; }
        public long IdArbitro { get; set; }
        public String Funcion { get; set; }
        public decimal Importe { get; set; }
    }
}
