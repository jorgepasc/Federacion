using System;
using System.Collections.Generic;
using System.Text;

namespace Federacion.Datos
{
    public class ArbitroDTO
    {
        //Datos BD
        private int _IdArbitro;
        private String _DNI;
        private String _Alias;
        private String _Nombre;
        private String _Apellidos;
        private String _NumTelefono;

        public int IdArbitro { get; set; }
        public String DNI { get; set; }
        public String Alias { get; set; }
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String NumTelefono { get; set; }

        //Datos extra
        private String _Funcion;
        public String Funcion { get; set; }
        private int _NumPartidos;
        public int NumPartidos { get; set; }
        private String _NombreCompleto;
        public String NombreCompleto { get; set; }

    }
}
