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

namespace Federacion.Pages.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalcularNominas : ContentPage
    {
        List<MesDTO> listaMeses = new List<MesDTO>();
        public CalcularNominas()
        {
            InitializeComponent();

            listaMeses.Add(new MesDTO { Numero = 9, Nombre = "Septiembre" });
            listaMeses.Add(new MesDTO { Numero = 10, Nombre = "Octubre" });
            listaMeses.Add(new MesDTO { Numero = 11, Nombre = "Noviembre" });
            listaMeses.Add(new MesDTO { Numero = 9, Nombre = "Diciembre" });
            listaMeses.Add(new MesDTO { Numero = 1, Nombre = "Enero" });
            listaMeses.Add(new MesDTO { Numero = 2, Nombre = "Febrero" });
            listaMeses.Add(new MesDTO { Numero = 3, Nombre = "Marzo" });
            listaMeses.Add(new MesDTO { Numero = 4, Nombre = "Abril" });
            listaMeses.Add(new MesDTO { Numero = 5, Nombre = "Mayo" });
            listaMeses.Add(new MesDTO { Numero = 6, Nombre = "Junio" });

            pickerMes.ItemsSource = listaMeses;
        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void BtnCalcular_Clicked(object sender, EventArgs e)
        {

            try
            {
                MesDTO mesSelected = (MesDTO)pickerMes.SelectedItem;
                if (mesSelected != null)
                {
                    //Solo tenemos en cuenta desde septiembre hasta junio. 
                    //Si ha seleccionado de septiembre a diciembre creamos fecha con anio anterior al actual, sino con anio actual
                    DateTime fechaNomina;
                    if (mesSelected.Numero >= 9 && mesSelected.Numero <= 12)
                    {
                        fechaNomina = new DateTime(DateTime.Today.AddYears(-1).Year, mesSelected.Numero, 1);
                    }
                    else
                    {
                        fechaNomina = new DateTime(DateTime.Today.Year, mesSelected.Numero, 1);
                    }

                    //Si estan calculadas ya las nominas del mes seleccionado (= ya hay algun registro del mes en la tabla nomina)
                    //no las volvemos a calcular
                    if (DatabaseHelper.ComprobarNominasCalculadas(mesSelected.Numero))
                    {
                        bool respuesta = await DisplayAlert("ERROR", "Las nominas de " + mesSelected.Nombre + " ya están calculadas. " +
                            "¿Desea visualizarlas?", "CONTINUAR", "CANCELAR");
                        if (respuesta)
                        {                            
                            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new VisualizarNominas(fechaNomina)));
                        }
                    }
                    else //Calcular nominas
                    {
                        List<NominaDTO> nominasCalculadas = new List<NominaDTO>();
                        nominasCalculadas = DatabaseHelper.CalcularNominas(fechaNomina.GetFirstDayOfMonth(), fechaNomina.GetLastDayOfMonth());

                        if(nominasCalculadas != null && nominasCalculadas.Any())
                        {
                            await DisplayAlert("INFO", "Nóminas de " + mesSelected.Nombre + " calculadas. " +
                                "Insertando en base de datos...", "ACEPTAR");
                            bool todosCorrectos = true;
                            foreach (NominaDTO nomina in nominasCalculadas)
                            {
                                int rowsAffected = DatabaseHelper.InsertarNomina(nomina);
                                if(rowsAffected != 1)
                                {
                                    todosCorrectos = false;
                                }
                            }

                            if (todosCorrectos)
                            {
                                await DisplayAlert("INFO", "Se han insertado todas las nóminas correctamente", "ACEPTAR");
                                bool respuesta = await DisplayAlert("INFO", "¿Desea visualizarlas?", "CONTINUAR", "CANCELAR");
                                if (respuesta)
                                {
                                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new VisualizarNominas(fechaNomina)));
                                }
                            }
                            else
                            {
                                await DisplayAlert("ERROR", "No se han podido insertar correctamente todas las nóminas", "ACEPTAR");
                            }
                        }
                        else
                        {
                            await DisplayAlert("ERROR", "No se han encontrado nóminas por calcular ", "ACEPTAR");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("ERROR", "Seleccione un mes del que calcular las nóminas", "ACEPTAR");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "ACEPTAR");
                Log.Log.LogMessageToFile("Error al calcular las nóminas: " + ex.Message + System.Environment.NewLine + ex.StackTrace
                    + System.Environment.NewLine + System.Environment.NewLine);
            }
        }
    }
}