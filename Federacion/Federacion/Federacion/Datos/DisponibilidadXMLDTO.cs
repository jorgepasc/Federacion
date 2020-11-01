using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Federacion.Datos
{
    //[XmlType("Federacion.Datos.DisponibilidadXMLDTO")]
    public class DisponibilidadXMLDTO
    {
        private DateTime _FechaDisponibilidad;
        private bool _Viernes;
        private bool _Sabado1;
        private bool _Sabado2;
        private bool _Domingo1;
        private bool _Domingo2;
        private String _Comentarios;

        [XmlElementAttribute(Order = 1)]
        public DateTime FechaDisponibilidad { get; set; }
        [XmlElementAttribute(Order = 2)]
        public bool Viernes { get; set; }
        [XmlElementAttribute(Order = 3)]
        public bool Sabado1 { get; set; }
        [XmlElementAttribute(Order = 4)]
        public bool Sabado2 { get; set; }
        [XmlElementAttribute(Order = 5)]
        public bool Domingo1 { get; set; }
        [XmlElementAttribute(Order = 6)]
        public bool Domingo2 { get; set; }
        [XmlElementAttribute(Order = 7)]
        public String Comentarios { get; set; }

    }
}
