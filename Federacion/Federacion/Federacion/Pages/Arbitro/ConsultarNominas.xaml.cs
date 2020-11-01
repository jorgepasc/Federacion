using Federacion.Datos;
using Federacion.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Federacion.Pages.Arbitro
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsultarNominas : TabbedPage
    {
        List<PartidoDTO> listadoPartidos = new List<PartidoDTO>();
        List<NominaDTO> listadoNominas = new List<NominaDTO>();
        public ConsultarNominas()
        {
            InitializeComponent();
            ConsultarNominasPage.Title = "Nóminas " + DateTime.Now.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));

            CargarNominaMensual();
            CargarNominasHistorico();
        }

        /// <summary>
        /// Carga todos los partidos del mes con su importe, y los suma
        /// </summary>
        private void CargarNominaMensual()
        {
            try
            {                
                DateTime primeroDeMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DateTime ultimoDeMes = primeroDeMes.AddMonths(1).AddDays(-1);
                //Le pasamos true para que se salte la carga de arbitros(detalles)
                listadoPartidos = DatabaseHelper.CargarPartidos(UsuarioActivoDTO.IdArbitro, primeroDeMes, ultimoDeMes, true);
                decimal total = 0;

                if (listadoPartidos != null && listadoPartidos.Count() > 0)
                {
                    listaPartidos.ItemsSource = listadoPartidos;
                    total = listadoPartidos.Sum(f => f.Importe);
                }
                else
                {
                    lblMensual.IsVisible = true;
                }

                lblImporte.Text = String.Format("TOTAL: {0}€", total.ToString());
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", "Error al cargar las nóminas del mes: " + ex.Message, "ACEPTAR");
                lblMensual.Text = "ERROR " + ex.Message;
                lblMensual.IsVisible = true;
            }
        }

        /// <summary>
        /// Accede a la tabla nóminas y muestra todas las del árbitro de este año
        /// </summary>
        private void CargarNominasHistorico()
        {
            try
            {
                listadoNominas = DatabaseHelper.CargarNominasHistoricoByIdArbitro(UsuarioActivoDTO.IdArbitro);
                decimal total = 0;

                if (listadoNominas != null && listadoNominas.Any())
                {
                    listaNominas.ItemsSource = listadoNominas;
                    total = listadoNominas.Sum(f => f.Total);
                }
                else
                {
                    lblHistorico.IsVisible = true;
                }
                lblImporteHistorico.Text = String.Format("TOTAL: {0}€", total.ToString());
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", "Error al cargar las nóminas de la temporada: " + ex.Message, "ACEPTAR");
            }

        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}