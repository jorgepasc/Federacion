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
    public partial class ConsultarInformes : ContentPage
    {
        List<InformeDTO> listadoInformes;
        public ConsultarInformes()
        {
            InitializeComponent();
            try
            {
                listadoInformes = DatabaseHelper.CargarInformes(UsuarioActivoDTO.IdArbitro);

                if (listadoInformes != null && listadoInformes.Count() > 0)
                {
                    listaInformes.ItemsSource = listadoInformes;
                    lblInfo.Text = "Informes disponibles: " + System.Environment.NewLine;
                }
                else
                {
                    lblInfo.Text = "No hay informes disponibles";
                    lblInfo.HorizontalOptions = LayoutOptions.Center;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", "Error al cargar los informes: " + ex.Message, "ACEPTAR");
                lblInfo.Text = "ERROR " + ex.Message; 
            }
        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}