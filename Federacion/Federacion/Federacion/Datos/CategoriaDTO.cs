using System;
using System.Collections.Generic;
using System.Text;

namespace Federacion.Datos
{
    class CategoriaDTO
    {
        private int _IdCategoria;
        private string _Descripcion;
        private string _Abreviatura;

        public int IdCategoria { get; set; }
        public string Descripcion { get; set; }
        public string Abreviatura { get; set; }
    }
}