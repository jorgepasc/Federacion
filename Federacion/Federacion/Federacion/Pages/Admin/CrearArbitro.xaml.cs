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
	public partial class CrearArbitro : ContentPage
	{
        List<ArbitroDTO> listadoArbitros = new List<ArbitroDTO>();
        List<AdministradorDTO> listadoAdmins = new List<AdministradorDTO>();
        public CrearArbitro ()
		{
			InitializeComponent ();                   
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
            txtAlias.Text = String.Empty;
            txtDNI.Text = String.Empty;
            txtNombre.Text = String.Empty;
            txtApellidos.Text = String.Empty;
            txtNumTelefono.Text = String.Empty;           
        }

        private async void BtnConfirmar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ValidarDatos())
                {
                    ArbitroDTO arbitro = CrearObjetoArbitro();
                    if(DatabaseHelper.CrearArbitro(arbitro) != 1)
                    {
                        await DisplayAlert("ERROR", "El árbitro no se ha podido crear correctamente", "ACEPTAR");
                    }
                    else
                    {
                        await DisplayAlert("OK", "El árbitro se ha creado correctamente", "ACEPTAR");
                        bool respuesta = await DisplayAlert("INFO", "¿Desea crearle un usuario al nuevo árbitro?", "CONTINUAR", "CANCELAR");
                        if (respuesta)
                        {
                            //Al ser un arbitro nuevo se le pone el Id con el identity, pero no llega al objeto, lo obtenemos aqui
                            arbitro.IdArbitro = DatabaseHelper.GetIdArbitroByAlias(arbitro.Alias);
                            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CrearUsuario(arbitro)));
                        }
                        ReiniciarControles();
                    }
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("ERROR", "Error al crear el árbitro: " + ex.Message, "ACEPTAR");
            }
        }

        private ArbitroDTO CrearObjetoArbitro()
        {
            try
            {
                ArbitroDTO arbitro = new ArbitroDTO();
                
                arbitro.Alias = txtAlias.Text.ToUpper().Trim();
                arbitro.DNI = txtDNI.Text;
                arbitro.Nombre = txtNombre.Text;
                arbitro.Apellidos = txtApellidos.Text;
                arbitro.NumTelefono = txtNumTelefono.Text;

                return arbitro;
            }
            catch(Exception ex)
            {
                return null;
                throw ex;             
            }            
        }

        private bool ValidarDatos()
        {
            if (String.IsNullOrEmpty(txtAlias.Text))
            {
                DisplayAlert("ERROR", "Introduce un alias válido", "ACEPTAR");
                return false;
            }

            if (String.IsNullOrEmpty(txtDNI.Text))
            {
                DisplayAlert("ERROR", "Introduce un DNI válido", "ACEPTAR");
                return false;
            }

            if (String.IsNullOrEmpty(txtNombre.Text))
            {
                DisplayAlert("ERROR", "Introduce un Nombre válido", "ACEPTAR");
                return false;
            }

            if (String.IsNullOrEmpty(txtApellidos.Text))
            {
                DisplayAlert("ERROR", "Introduce unos apellidos válidos", "ACEPTAR");
                return false;
            }

            if (String.IsNullOrEmpty(txtNumTelefono.Text))
            {
                DisplayAlert("ERROR", "Introduce un número de teléfono válido", "ACEPTAR");
                return false;
            }

            return true;
        }
        
    }
}