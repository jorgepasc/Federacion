using Federacion.Helpers;
using Federacion.Datos;
using Federacion.Pages.Arbitro;
using Federacion.Pages;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Federacion.Pages.Admin;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Federacion
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                InitializeComponent();
                //Inicializo el automapper y la conexión a base de datos
                DatabaseHelper.InitializeAutomapper();
                DatabaseHelper.CrearConexion();

                //File.Delete(UsuarioActivoDTO.GetRutaRecuerdame());

                if (UsuarioActivoDTO.LeerRecuerdame()) //Si devuelve true, ya esta registrado el usuario
                {
                    if (UsuarioActivoDTO.IsAdmin)
                    {
                        MainPage = new NavigationPage(new InicioUWP());
                    }
                    else
                    {
                        MainPage = new NavigationPage(new InicioAndroid());                        
                    }
                }
                else
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
            catch(Exception ex)
            {                
                Log.Log.LogMessageToFile("Error al iniciar la aplicacion:" + ex.Message + " EN " + ex.StackTrace);
                MainPage = new ErrorPage();
            }            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
