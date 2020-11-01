using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Federacion.Datos
{
    //[XmlType("Federacion.Datos.DisponibilidadDTO")]
    class DisponibilidadDTO
    {
        private long _IdDisponibilidad;
        public DisponibilidadXMLDTO _InfoDisponibilidad;
        private DateTime _FechaDisponibilidad;
        private long _IdArbitro;
        private string _DisponibilidadDetalles;

        public long IdDisponibilidad { get; set; }        
        public DisponibilidadXMLDTO InfoDisponibilidad { get; set; }
        public DateTime FechaDisponibilidad { get; set; }
        public long IdArbitro { get; set; }
        public string DisponibilidadDetalles { get; set; }

    }
}
