using System;
using System.Collections.Generic;
using System.Text;

namespace Federacion.Datos
{
    public class AdministradorDTO
    {
        //Datos BD
        private int _IdAdministrador;
        private String _DNI;        
        private String _Nombre;
        private String _Apellidos;
        private String _NumTelefono;
        private String _Alias;

        public int IdAdministrador { get; set; }
        public String DNI { get; set; }        
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String NumTelefono { get; set; }
        public String Alias { get; set; }
    }
}
