using Federacion.Helpers;
using Federacion.Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Federacion.Pages.Arbitro;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Federacion.Pages.Admin;

namespace Federacion.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            txtPass.Text = String.Empty;
        }

        private async void BtnIniciar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (txtUsuario.Text != null && txtUsuario.Text.Trim() != "" && txtPass.Text != null && txtPass.Text.Trim() != "")
                {                    
                    if (DatabaseHelper.ComprobarLogin(txtUsuario.Text, txtPass.Text))
                    {
                        if(Device.RuntimePlatform == Device.UWP)
                        {
                            if (switchRecordar.IsToggled)
                            {
                                UsuarioActivoDTO.EscribirRecuerdame();
                            }
                            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new InicioUWP()));
                        }
                        else
                        {
                            await DisplayAlert("ERROR", "No se puede iniciar sesión como administrador en Android", "ACEPTAR");
                        }                        
                    }
                    else
                    {
                        if(Device.RuntimePlatform == Device.Android)
                        {
                            if (switchRecordar.IsToggled)
                            {
                                UsuarioActivoDTO.EscribirRecuerdame();
                            }
                            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new InicioAndroid()));
                        }
                        else
                        {
                            await DisplayAlert("ERROR", "No se puede iniciar sesión como árbitro en Windows", "ACEPTAR");
                        }                        
                    }
                }
                else
                {
                    await DisplayAlert("ERROR", "Los campos no pueden estar vacíos", "ACEPTAR");
                }
            }
            catch (Exception ex)
            {
                txtPass.Text = "";
                await DisplayAlert("ERROR", "Error al conectar: " + ex.Message, "ACEPTAR");
                Log.Log.LogMessageToFile("Error en el Login: " + ex.Message + " EN " + ex.StackTrace);
            }
        }

        private void ShowPass_Toggled(object sender, ToggledEventArgs e)
        {
            txtPass.IsPassword = !txtPass.IsPassword;
        }
    }
}