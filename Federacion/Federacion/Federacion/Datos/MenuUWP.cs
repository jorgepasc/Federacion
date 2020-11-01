using System;
using System.Collections.Generic;
using System.Text;

namespace Federacion.Datos
{
    class MenuUWP
    {
        public MenuUWP()
        {
            Nombre = String.Empty;
            Source = String.Empty;
        }

        public MenuUWP(string nombre, string source)
        {
            Nombre = nombre;
            Source = source;
        }

        private String _Nombre;
        public String Nombre { get; set; }
        private String _ImageSource;
        public String Source { get; set; }        
    }
}
