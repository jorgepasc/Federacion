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
	public partial class CrearUsuario : ContentPage
	{
        List<ArbitroDTO> listadoArbitros = new List<ArbitroDTO>();
        List<AdministradorDTO> listadoAdmins = new List<AdministradorDTO>();
        public CrearUsuario (ArbitroDTO arbitro)
		{
			InitializeComponent ();
            try
            {
                //Si no viene de crear un arbitro le cargamos todos los administradores y arbitros
                if(arbitro == null)
                {              
                    //Cargar Todos los arbitros
                    listadoArbitros = DatabaseHelper.CargarTodosArbitros();
                    if (listadoArbitros != null && listadoArbitros.Any())
                    {
                        pickerArbitros.ItemsSource = listadoArbitros;
                    }

                    //Cargar Todos los admins
                    listadoAdmins = DatabaseHelper.CargarTodosAdmins();
                    if (listadoAdmins != null && listadoAdmins.Any())
                    {
                        pickerAdmins.ItemsSource = listadoAdmins;
                    }
                }
                else
                {
                    IsAdmin.IsToggled = false;
                    IsAdmin.IsEnabled = false;
                    pickerAdmins.IsVisible = false;
                    pickerArbitros.IsVisible = true;
                    pickerArbitros.IsEnabled = false;
                    List<ArbitroDTO> lista = new List<ArbitroDTO>();
                    lista.Add(arbitro);
                    pickerArbitros.ItemsSource = lista;
                    pickerArbitros.SelectedItem = arbitro;
                    
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

        private void BtnReiniciar_Clicked(object sender, EventArgs e)
        {
            ReiniciarControles();
        }

        private void ReiniciarControles()
        {
            txtUsuario.Text = String.Empty;
            txtPass.Text = String.Empty;
            IsAdmin.IsToggled = false;
            pickerArbitros.SelectedItem = null;
            pickerAdmins.SelectedItem = null;
        }

        private void BtnConfirmar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ValidarDatos())
                {
                    UsuarioDTO usuario = CrearObjetoUsuario();
                    if(DatabaseHelper.CrearUsuario(usuario) != 1)
                    {
                        DisplayAlert("ERROR", "El usuario no se ha podido crear correctamente", "ACEPTAR");
                    }
                    else
                    {
                        DisplayAlert("OK", "El usuario se ha creado correctamente", "ACEPTAR");
                        ReiniciarControles();
                    }
                }
            }
            catch(Exception ex)
            {
                DisplayAlert("ERROR", "Error al crear el usuario: " + ex.Message, "ACEPTAR");
            }
        }

        private UsuarioDTO CrearObjetoUsuario()
        {
            try
            {
                UsuarioDTO user = new UsuarioDTO();
                if (IsAdmin.IsToggled)
                {
                    user.IdArbitro = null;
                    AdministradorDTO adminSelected = (AdministradorDTO)pickerAdmins.SelectedItem;
                    user.IdAdministrador = adminSelected.IdAdministrador;
                }
                else
                {
                    user.IdAdministrador = null;
                    ArbitroDTO arbiSelected = (ArbitroDTO)pickerArbitros.SelectedItem;
                    user.IdArbitro = arbiSelected.IdArbitro;
                }
                user.Usuario = txtUsuario.Text.Trim();
                user.Password = txtPass.Text.Trim();
                user.IsAdmin = IsAdmin.IsToggled;

                return user;
            }
            catch(Exception ex)
            {
                return null;
                throw ex;             
            }            
        }

        private bool ValidarDatos()
        {
            if (String.IsNullOrEmpty(txtUsuario.Text))
            {
                DisplayAlert("ERROR", "Introduce un nombre de usuario válido", "ACEPTAR");
                return false;
            }

            if (String.IsNullOrEmpty(txtPass.Text))
            {
                DisplayAlert("ERROR", "Introduce un password válido", "ACEPTAR");
                return false;
            }

            if (IsAdmin.IsToggled)
            {
                if(pickerAdmins.SelectedItem == null)
                {
                    DisplayAlert("ERROR", "Escoge un administrador al que asociar el usuario", "ACEPTAR");
                    return false;
                }
            }
            else
            {
                if(pickerArbitros.SelectedItem == null)
                {
                    DisplayAlert("ERROR", "Escoge un arbitro al que asociar el usuario", "ACEPTAR");
                    return false;
                }
            }

            return true;
        }

        private void IsAdmin_Toggled(object sender, ToggledEventArgs e)
        {
            Switch IsAdmin = (Switch)sender;

            if (IsAdmin.IsToggled)
            {
                pickerArbitros.IsVisible = false;
                pickerAdmins.IsVisible = true;
            }
            else
            {
                pickerArbitros.IsVisible = true;
                pickerAdmins.IsVisible = false;
            }
        }

        private void SwitchMostrarPass_Toggled(object sender, ToggledEventArgs e)
        {
            txtPass.IsPassword = !txtPass.IsPassword;
        }
    }
}