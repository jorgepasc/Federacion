using Federacion.Datos;
using Federacion.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Federacion.Pages.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisualizarNominas : ContentPage
    {
        List<NominaDTO> listadoNominas = new List<NominaDTO>();
        public VisualizarNominas(DateTime fecha)
        {
            try
            {                
                InitializeComponent();

                VisualizarNominasPage.Title = "Nóminas " + fecha.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));

                listadoNominas = DatabaseHelper.CargarNominasHistorico(fecha.Month);

                if (listadoNominas != null && listadoNominas.Count() > 0)
                {
                    listaNominas.ItemsSource = listadoNominas;
                    lblTotal.Text = "TOTAL: " + listadoNominas.Sum(f => f.Total) + " €";
                }
                else
                {
                    lblInfo.Text = "No hay nóminas para visualizar este mes";
                    lblInfo.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    frame.IsVisible = false;
                    frameTotal.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", "Error al cargar: " + ex.Message, "ACEPTAR");//No se ve por que no va el OnAppearing y el DisplayAlert no va en el new Page
                lblInfo.Text = "ERROR " + ex.Message; 
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (listadoNominas != null && listadoNominas.Any())
                {
                    SearchBar buscador = (SearchBar)sender;
                    string filtro = buscador.Text.ToUpper();
                    listaNominas.ItemsSource = listadoNominas.Where(f => f.Alias.ToUpper().Contains(filtro));
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