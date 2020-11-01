using Federacion.Datos;
using Federacion.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Federacion.Pages.Admin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DesignarArbitro : ContentPage
	{
        ObservableCollection<ArbitroDTO> listaArbitros = new ObservableCollection<ArbitroDTO>();
		public DesignarArbitro ()
		{
			InitializeComponent ();            
            try
            {
                CargarArbitros();
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", "Error al cargar los árbitros", "ACEPTAR");
                Log.Log.LogMessageToFile("Error al cargar los árbitros" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                lblInfo.Text = "ERROR " + ex.Message;
            }
            finally
            {
                lista.IsRefreshing = false;
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (listaArbitros != null && listaArbitros.Any())
                {
                    SearchBar buscador = (SearchBar)sender;
                    string filtro = buscador.Text.ToUpper();
                    lista.ItemsSource = listaArbitros.Where(f => f.Alias.ToUpper().Contains(filtro));
                }
            }
            catch(Exception ex)
            {
                DisplayAlert("ERROR", ex.Message, "ACEPTAR");
            }            
        }

        private async void Arbitro_Tapped(object sender, EventArgs e)
        {
            ArbitroDTO arbitroSelected = (ArbitroDTO)lista.SelectedItem;
            //Navegar a siguiente pagina pasando el arbi
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new DesignarPartidos(arbitroSelected)));
        }

        private void Lista_Refreshing(object sender, EventArgs e)
        {
            CargarArbitros();
        }

        private void CargarArbitros()
        {
            try
            {
                //Carga lista de árbitros 
                listaArbitros = new ObservableCollection<ArbitroDTO>(DatabaseHelper.GetListaArbitrosDesignar());
                if (listaArbitros != null && listaArbitros.Any())
                {
                    lista.ItemsSource = listaArbitros;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", "Error al obtener los árbitros" + ex.Message, "ACEPTAR");
                Log.Log.LogMessageToFile("Error al cargar los árbitros" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                lblInfo.Text = "ERROR " + ex.Message;
            }
            finally
            {
                lista.IsRefreshing = false;
            }
        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            CargarArbitros();
        }
    }
}