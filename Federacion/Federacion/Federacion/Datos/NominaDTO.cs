using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Federacion.Datos
{
    class NominaDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int _IdNomina;
        private DateTime _FechaNomina;
        private decimal _Total;
        private int _IdArbitro;

        public int IdNomina { get; set; }
        public DateTime FechaNomina{ get; set; }
        public decimal Total { get; set; }
        public int IdArbitro { get; set; }

        //Datos extra
        private string _PeriodoNomina;
        public string PeriodoNomina { get; set; }
        private string _Alias;
        public string Alias { get; set; }
    }
}