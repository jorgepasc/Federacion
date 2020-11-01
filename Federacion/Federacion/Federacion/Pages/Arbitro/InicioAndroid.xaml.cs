using Federacion.Datos;
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
    public partial class InicioAndroid : ContentPage
    {
        public InicioAndroid()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// Limpia el stack y cierra sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnLogOut_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool respuesta = await DisplayAlert("INFO", "¿Seguro que deseas cerrar sesión?", "CONTINUAR", "CANCELAR");
                if (respuesta)
                {
                    BorrarRecuerdame();
                    await Application.Current.MainPage.Navigation.PushModalAsync((new LoginPage()));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", "Error al cerrar sesión: " + ex.Message, "ACEPTAR");
                Log.Log.LogMessageToFile("Error al hacer logout: " + ex.Message + System.Environment.NewLine + ex.StackTrace + System.Environment.NewLine);
            }
        }

        //Comentado porque no funcionaba
        ///// <summary>
        ///// Pide confirmacion y si el usuario la da, cierra la aplicacion
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>        
        //private async void BtnSalir_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        bool respuesta = await DisplayAlert("INFO", "¿Seguro que deseas salir de la aplicación?", "CONTINUAR", "CANCELAR");
        //        if (respuesta)
        //        {
        //            Application.Current.Quit();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("ERROR", "Error al salir de la app: " + ex.Message, "ACEPTAR");
        //        Log.Log.LogMessageToFile("Error al salir de la app: " + ex.Message + System.Environment.NewLine + ex.StackTrace + System.Environment.NewLine);
        //    }                                    
        //}

        private async void ConsultarNominasTapped(object sender, EventArgs e)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ConsultarNominas()));
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al abrir las nominas: " + ex.Message + System.Environment.NewLine + ex.StackTrace + System.Environment.NewLine);
            }

        }
        private async void ConsultarPartidosTapped(object sender, EventArgs e)
        {
                try
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ConsultarPartidos()));
                }
                catch (Exception ex)
                {
                    Log.Log.LogMessageToFile("Error al abrir los partidos: " + ex.Message + System.Environment.NewLine + ex.StackTrace + System.Environment.NewLine);
                }
        }
        private async void EnviarDisponibilidadTapped(object sender, EventArgs e)
        {
            try
            {
               await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new Disponibilidad()));
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al abrir la disponibilidad: " + ex.Message + " EN " + ex.StackTrace);
            }
        }
        private async void EnviarResultadosTapped(object sender, EventArgs e)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ConsultarInformes()));
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al abrir los informes: " + ex.Message + System.Environment.NewLine + ex.StackTrace + System.Environment.NewLine);
            }
        }

        /// <summary>
        /// Borra el fichero que se genera con los datos del usuario para recordar la sesion
        /// </summary>
        private void BorrarRecuerdame()
        {
            try
            {
                string path = UsuarioActivoDTO.GetRutaRecuerdame();
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al borrar recuerdame: " + ex.Message + " EN " + ex.StackTrace);
            }

        }

        //No funcionan los focus, en movil haces tap, solo haces focus cuando te pones a escribir en un entry por ejemplo
        private void AbsoluteLayout_Focused(object sender, FocusEventArgs e)
        {
            AbsoluteLayout layout = (AbsoluteLayout)sender;
            layout.BackgroundColor = Color.SteelBlue;
        }

        private void AbsoluteLayout_Unfocused(object sender, FocusEventArgs e)
        {
            AbsoluteLayout layout = (AbsoluteLayout)sender;
            layout.BackgroundColor = Color.LightSteelBlue;
        }        
    }
}