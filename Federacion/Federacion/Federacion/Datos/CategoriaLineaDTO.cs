using System;
using System.Collections.Generic;
using System.Text;

namespace Federacion.Datos
{
    class CategoriaLineaDTO
    {
        private int _IdCategoriaLinea;
        private int _IdCategoria;
        private int _IdFuncion;
        private decimal _Importe;
        public int IdCategoriaLinea { get; set; }
        public int IdCategoria { get; set; }
        public int IdFuncion { get; set; }
        public decimal Importe { get; set; }
    }
}