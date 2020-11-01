using System;
using System.Collections.Generic;
using System.Text;

namespace Federacion.Datos
{
     public class FuncionDTO
    {
        private int _IdFuncion;
        private string _Descripcion;
        public int IdFuncion { get; set; }
        public string Descripcion { get; set; }

        public FuncionDTO()
        {
            IdFuncion = 0;
            Descripcion = String.Empty;
        }

        public FuncionDTO(int idFuncion, string descripcionFuncion)
        {
            IdFuncion = idFuncion;
            Descripcion = descripcionFuncion;
        }

    }
}