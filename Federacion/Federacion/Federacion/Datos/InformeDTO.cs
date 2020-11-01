using System;
using System.Collections.Generic;
using System.Text;

namespace Federacion.Datos
{
    public class InformeDTO
    {
        //Datos BD
        private int _IdInforme;
        private int _IdArbitro;
        private DateTime _FechaInforme;
        private string _TextoInforme;
        private bool _IsFavorable;

        public int IdInforme { get; set; }
        public int IdArbitro { get; set; }
        public DateTime FechaInforme { get; set; }
        public string TextoInforme { get; set; }
        public bool IsFavorable { get; set; }

        //Datos extra
        private string _Color;
        public string Color { get; set; }
    }
}
