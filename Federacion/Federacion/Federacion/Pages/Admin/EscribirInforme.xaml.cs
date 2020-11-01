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
	public partial class EscribirInforme : ContentPage
	{
        List<ArbitroDTO> listadoArbitros = new List<ArbitroDTO>();
        public EscribirInforme()
		{
            try
            {
                InitializeComponent();
                listadoArbitros = DatabaseHelper.CargarTodosArbitros();
                if (listadoArbitros != null && listadoArbitros.Any())
                {
                    pickerArbitro.ItemsSource = listadoArbitros;
                }
            }
            catch(Exception ex)
            {
                DisplayAlert("ERROR", "Error al cargar: " + ex.Message, "ACEPTAR");
                lblInfo.Text = "ERROR" + ex.Message;
            }
        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }        

        private void BtnEnviar_Clicked(object sender, EventArgs e)
        {            
            try
            {
                InformeDTO informe = new InformeDTO();
                ArbitroDTO arbitroSelected = (ArbitroDTO)pickerArbitro.SelectedItem;

                if(arbitroSelected == null)
                {
                    DisplayAlert("ERROR", "Escoge un árbitro al que realizarle el informe", "ACEPTAR");
                    return;
                }

                if (String.IsNullOrEmpty(txtInforme.Text))
                {
                    DisplayAlert("ERROR", "El informe no puede estar vacío", "ACEPTAR");
                    return;
                }

                informe.IdArbitro = arbitroSelected.IdArbitro;
                informe.FechaInforme = DateTime.Now;
                informe.TextoInforme = txtInforme.Text;
                informe.IsFavorable = IsFavorable.IsToggled;

                if(DatabaseHelper.CrearInforme(informe) != 1)
                {
                    DisplayAlert("ERROR", "Ha habido un problema al crear el informe", "ACEPTAR");
                }
                else
                {
                    DisplayAlert("OK", "El informe ha sido creado correctamente", "ACEPTAR");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", "Error al crear el informe " + ex.Message, "ACEPTAR");
                Log.Log.LogMessageToFile("" + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }
    }
}