using Federacion.Datos;
using Federacion.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Federacion.Pages.Admin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CrearPartido : ContentPage
	{
        List<ArbitroDTO> listadoArbitros = new List<ArbitroDTO>();
        List<CategoriaDTO> listadoCategorias = new List<CategoriaDTO>();
        public CrearPartido()
		{
            InitializeComponent();
            try
            {
                //Cargar categorias
                listadoCategorias = DatabaseHelper.CargarTodasCategorias();
                if (listadoCategorias != null && listadoCategorias.Any())
                {
                    pickerCategorias.ItemsSource = listadoCategorias;
                }
            }
            catch(Exception ex)
            {
                DisplayAlert("ERROR", "Error al cargar las categorías: " + ex.Message, "ACEPTAR");
                lblInfo.Text = "ERROR" + ex.Message;
            }
        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void BtnReiniciar_Clicked(object sender, EventArgs e)
        {
            ReiniciarControles();
        }

        private void ReiniciarControles()
        {
            txtLocal.Text = String.Empty;
            txtVisitante.Text = String.Empty;
            txtUbicacion.Text = String.Empty;
            txtObservaciones.Text = String.Empty;
            pickerCategorias.SelectedItem = null;            
        }

        private async void BtnConfirmar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ValidarDatos())
                {
                    PartidoDTO partido = CrearObjetoPartido();
                    if(DatabaseHelper.CrearPartido(partido) != 1)
                    {
                        await DisplayAlert("ERROR", "El partido no se ha podido crear correctamente", "ACEPTAR");
                    }
                    else
                    {
                        await DisplayAlert("OK", "El partido se ha creado correctamente", "ACEPTAR");
                        ReiniciarControles();
                    }
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("ERROR", "Error al crear el partido: " + ex.Message, "ACEPTAR");
            }
        }

        private PartidoDTO CrearObjetoPartido()
        {
            try
            {
                PartidoDTO partido = new PartidoDTO();

                partido.EquipoLocal = txtLocal.Text;
                partido.EquipoVisitante = txtVisitante.Text;
                partido.FechaPartido = pickFecha.Date + pickHora.Time;
                partido.Ubicacion = txtUbicacion.Text;
                partido.Observaciones = txtObservaciones.Text;
                CategoriaDTO categoria = (CategoriaDTO)pickerCategorias.SelectedItem;
                partido.IdCategoria = categoria.IdCategoria;

                return partido;
            }
            catch(Exception ex)
            {
                throw ex;             
            }            
        }

        private bool ValidarDatos()
        {
            if (String.IsNullOrEmpty(txtLocal.Text))
            {
                DisplayAlert("ERROR", "Introduce un equipo local válido", "ACEPTAR");
                return false;
            }

            if (String.IsNullOrEmpty(txtVisitante.Text))
            {
                DisplayAlert("ERROR", "Introduce un equipo visitante válido", "ACEPTAR");
                return false;
            }

            if (String.IsNullOrEmpty(txtUbicacion.Text))
            {
                DisplayAlert("ERROR", "Introduce una ubicación válida", "ACEPTAR");
                return false;
            }

            if (pickerCategorias.SelectedItem == null)
            {
                DisplayAlert("ERROR", "Introduce una categoría válida", "ACEPTAR");
                return false;
            }

            return true;
        }
        
    }
}