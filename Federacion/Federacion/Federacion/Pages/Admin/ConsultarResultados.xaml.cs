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

namespace Federacion.Pages.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsultarResultados : ContentPage
    {
        List<PartidoDTO> listadoPartidos;
        public ConsultarResultados()
        {
            InitializeComponent();

            try
            {
                DateTime lunesAnterior, lunesSiguiente;
                lunesAnterior = DateTime.Today.GetDateAnteriorLunes();
                lunesSiguiente = DateTime.Today.GetDateSiguienteLunes();

                ConsultarResultadosPage.Title = "Resultados semana " + lunesAnterior.ToString("dd/MM/yy");

                listadoPartidos = DatabaseHelper.CargarResultados(lunesAnterior, lunesSiguiente);

                if (listadoPartidos != null && listadoPartidos.Count() > 0)
                {
                    listaPartidos.ItemsSource = listadoPartidos;
                }
                else
                {
                    lblInfo.Text = "No se ha jugado ningún partido este fin de semana";
                    lblInfo.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    frame.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", "Error al cargar: " + ex.Message, "ACEPTAR");
                lblInfo.Text = "ERROR " + ex.Message;
            }            
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if(listadoPartidos != null && listadoPartidos.Any())
                {
                    SearchBar buscador = (SearchBar)sender;
                    string filtro = buscador.Text.ToUpper();
                    listaPartidos.ItemsSource = listadoPartidos.Where(f => f.AbreviaturaCategoria.ToUpper().Contains(filtro));
                }                
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", ex.Message, "ACEPTAR");
            }            
        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}