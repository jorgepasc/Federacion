using Federacion.Datos;
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
    public partial class InicioUWP : ContentPage
    {
        public InicioUWP()
        {
            InitializeComponent();

            ObservableCollection<MenuUWP> itemsMenu = new ObservableCollection<MenuUWP>();
            itemsMenu.Add(new MenuUWP { Nombre = "Designar Partidos", Source = "disponibilidad.png" });
            itemsMenu.Add(new MenuUWP { Nombre = "Calcular Nóminas", Source = "nomina3.png" });
            itemsMenu.Add(new MenuUWP { Nombre = "Visualizar Nóminas", Source = "busqueda.png" });
            itemsMenu.Add(new MenuUWP { Nombre = "Consultar Resultados", Source = "resultados2.png" });
            itemsMenu.Add(new MenuUWP { Nombre = "Dar de alta árbitro", Source = "arbitro.png" });
            itemsMenu.Add(new MenuUWP { Nombre = "Dar de alta usuario", Source = "usuario.png" });
            itemsMenu.Add(new MenuUWP { Nombre = "Crear partido", Source = "calendario.png" });
            itemsMenu.Add(new MenuUWP { Nombre = "Crear informe", Source = "crearinforme.png" });

            listaMenu.ItemsSource = itemsMenu;
        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool respuesta = await DisplayAlert("INFO", "¿Deseas cerrar sesión?", "CONTINUAR", "CANCELAR");
                if (respuesta)
                {
                    BorrarRecuerdame();
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", "Error al cerrar sesión: " + ex.Message, "ACEPTAR");
            }
        }

        private async void ViewCell_Tapped(object sender, EventArgs e)
        {
            MenuUWP item = (MenuUWP)listaMenu.SelectedItem;

            switch (item.Nombre)
            {
                case "Designar Partidos":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new DesignarArbitro()));
                    break;
                case "Calcular Nóminas":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CalcularNominas()));
                    break;
                case "Visualizar Nóminas":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new VisualizarNominas(DateTime.Today)));
                    break;
                case "Consultar Resultados":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ConsultarResultados()));
                    break;
                case "Dar de alta árbitro":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CrearArbitro()));
                    break;
                case "Dar de alta usuario":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CrearUsuario(null)));
                    break;
                case "Crear partido":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new CrearPartido()));
                    break;
                case "Crear informe":
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new EscribirInforme()));
                    break;
                default:
                    break;
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
    }
}