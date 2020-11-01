using Federacion.Datos;
using Federacion.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Federacion.Pages.Arbitro
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsultarPartidos : ContentPage
    {
        List<PartidoDTO> listadoPartidos;
        public ConsultarPartidos()
        {
            try
            {
                InitializeComponent();

                DateTime lunesAnterior, lunesSiguiente;
                lunesAnterior = DateTime.Today.GetDateAnteriorLunes();
                lunesSiguiente = DateTime.Today.GetDateSiguienteLunes();

                ConsultarPartidosPage.Title = "Partidos semana " + lunesAnterior.ToString("dd/MM/yy");

                listadoPartidos = DatabaseHelper.CargarPartidos(UsuarioActivoDTO.IdArbitro, lunesAnterior, lunesSiguiente, false);

                if (listadoPartidos != null && listadoPartidos.Count() > 0)
                {
                    listaPartidos.ItemsSource = listadoPartidos;
                }
                else
                {
                    lblInfo.Text = "No tienes ningún partido este fin de semana";
                    lblInfo.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    frame.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", ex.Message, "ACEPTAR");
                lblInfo.Text = "ERROR: " + ex.Message;
            }
        }

        private void ListaPartidos_Refreshing(object sender, EventArgs e)
        {
            try
            {
                listadoPartidos = DatabaseHelper.CargarPartidos(UsuarioActivoDTO.IdArbitro, DateTime.Today.GetDateAnteriorLunes(), DateTime.Today.GetDateSiguienteLunes(), false);
                if (listadoPartidos != null && listadoPartidos.Count() > 0)
                {
                    listaPartidos.ItemsSource = listadoPartidos;
                }
                else
                {
                    lblInfo.Text = "No tienes ningún partido este fin de semana";
                    lblInfo.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    frame.IsVisible = false;
                }
                listaPartidos.IsRefreshing = false;
                listaPartidos.EndRefresh();
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", ex.Message, "ACEPTAR");
            }
        }

        private async void Partido_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new PartidoDetalles((PartidoDTO)listaPartidos.SelectedItem)));
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("" + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}