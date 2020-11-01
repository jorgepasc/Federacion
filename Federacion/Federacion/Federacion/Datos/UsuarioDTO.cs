using System;
using System.Collections.Generic;
using System.Text;

namespace Federacion.Datos
{
    public class UsuarioDTO
    {
        //Datos BD
        private int _IdUsuario;
        private int _IdArbitro;
        private int _IdAdministrador;
        private String _Usuario;        
        private String _Password;
        private bool _IsAdmin;

        public int IdUsuario { get; set; }
        public int? IdArbitro { get; set; }
        public int? IdAdministrador { get; set; }
        public String Usuario { get; set; }        
        public String Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
