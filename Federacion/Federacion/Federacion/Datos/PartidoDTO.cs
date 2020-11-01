using System;
using System.Collections.Generic;
using System.Text;

namespace Federacion.Datos
{
    /// <summary>
    /// Tiene los campos de la tabla Partido y datos extra que necesitamos de otras tablas
    /// </summary>
    public class PartidoDTO
    {
        //Datos tabla
        private int _IdPartido;
        private DateTime _FechaPartido;
        private String _EquipoLocal;
        private String _EquipoVisitante;
        private String _Equipos;
        private int _IdCategoria;
        private String _Ubicacion;
        private String _Observaciones;

        public int IdPartido { get; set; }
        public DateTime FechaPartido { get; set; }
        public String EquipoLocal { get; set; }
        public String EquipoVisitante { get; set; }
        public String Equipos { get; set; }
        public int IdCategoria { get; set; }
        public String Ubicacion { get; set; }
        public String Observaciones { get; set; }

        //Datos extra (GetDatosPartido)
        private String _DescripcionCategoria;
        private String _AbreviaturaCategoria;
        private int _IdPartidoArbitrado;
        private int _IdFuncion;
        private String _DescripcionFuncion;
        private decimal _Importe;
        private int _IdArbitro;
        private String _Alias;
        private String _NumTelefono;
        private String _ColorFondo;
        private String _ColorLetra;
        private String _Resultado;
        private String _ImageSource;
        private bool _IsVisibleDesasignar;
        private bool _IsVisibleTextoFuncion;
        private bool _IsVisiblePickerFuncion;
        private FuncionDTO _Funcion;
        private List<FuncionDTO> _listaFunciones;
        private List<ArbitroDTO> _listaArbitros;
        public String DescripcionCategoria { get; set; }
        public String AbreviaturaCategoria { get; set; }
        public int IdPartidoArbitrado { get; set; }
        public int IdFuncion { get; set; }
        public String DescripcionFuncion { get; set; }     
        public decimal Importe { get; set; }
        public int IdArbitro { get; set; }
        public String Alias { get; set; }
        public String NumTelefono { get; set; }
        public String Color { get; set; }
        public String ColorLetra { get; set; }
        public String Resultado { get; set; }
        public String ImageSource { get; set; }
        public bool IsVisibleTextoFuncion { get; set; }
        public bool IsVisiblePickerFuncion { get; set; }
        public bool IsVisibleDesasignar { get; set; }
        public FuncionDTO Funcion { get; set; }
        public List<FuncionDTO> listaFunciones { get; set; }
        public List<ArbitroDTO> listaArbitros { get; set; }
    }
}
