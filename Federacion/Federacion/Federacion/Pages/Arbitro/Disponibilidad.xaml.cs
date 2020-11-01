using Federacion.Helpers;
using Federacion.Datos;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Federacion.ViewModels;

namespace Federacion.Pages.Arbitro
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Disponibilidad : ContentPage
    {
        bool disponibilidadEnviada;
        DisponibilidadDTO disponibilidad;

        public Disponibilidad()
        {
            try
            {
                InitializeComponent();
                DisponibilidadPage.Title = "Disponibilidad semana " + DateTime.Today.GetDateSiguienteLunes().ToString("dd/MM/yy");
                //Comprobamos si ya se ha enviado la disponibilidad esta semana, si devuelve objeto ya hay disponibilidad y bloqueamos la pantalla
                disponibilidad = DatabaseHelper.CargarDisponibilidad(UsuarioActivoDTO.IdArbitro, DateTime.Today.GetDateSiguienteLunes());
                //BindingContext = new ConsultarPartidosViewModel();
                if (disponibilidad != null)
                {
                    disponibilidadEnviada = true;
                    SetValorControles();
                    DeshabilitarControles();
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", ex.Message, "ACEPTAR");
                lblInfo.Text = "ERROR: " + ex.Message;
            }
        }

        #region Metodos
        /// <summary>
        /// Bindea los elementos de la pantalla a las propiedades correspondientes de DisponibilidadDTO
        /// </summary>
        private void SetValorControles()
        {
            if (disponibilidad != null && disponibilidad.InfoDisponibilidad != null)
            {
                DisponibilidadXMLDTO infoDisponibilidad = disponibilidad.InfoDisponibilidad;
                cbViernes.IsToggled = infoDisponibilidad.Viernes;
                cbSabado1.IsToggled = infoDisponibilidad.Sabado1;
                cbSabado2.IsToggled = infoDisponibilidad.Sabado2;
                cbDomingo1.IsToggled = infoDisponibilidad.Domingo1;
                cbDomingo2.IsToggled = infoDisponibilidad.Domingo2;
                txtComentarios.Text = infoDisponibilidad.Comentarios;
            }
        }

        /// <summary>
        /// Si la disponibilidad ya está enviada bloqueamos los controles para que no pueda volverla a enviar
        /// </summary>
        private void DeshabilitarControles()
        {
            btnEnviar.IsEnabled = false;
            btnReiniciar.IsEnabled = false;
            cbViernes.IsEnabled = false;
            cbSabado1.IsEnabled = false;
            cbSabado2.IsEnabled = false;
            cbDomingo1.IsEnabled = false;
            cbDomingo2.IsEnabled = false;
            txtComentarios.IsEnabled = false;
        }

        /// <summary>
        /// Mete los datos elegidos por el usuario en un objeto DisponibilidadDTO
        /// </summary>
        /// <returns></returns>
        private DisponibilidadDTO RellenarDisponibilidad()
        {
            DisponibilidadDTO disponibilidad = new DisponibilidadDTO();
            StringWriter stringWriter = null;
            try
            {
                DisponibilidadXMLDTO disponibilidadXML = new DisponibilidadXMLDTO();
                //Viernes = cbViernes.IsToggled;
                disponibilidadXML.Viernes = cbViernes.IsToggled;
                disponibilidadXML.Sabado1 = cbSabado1.IsToggled;
                disponibilidadXML.Sabado2 = cbSabado2.IsToggled;
                disponibilidadXML.Domingo1 = cbDomingo1.IsToggled;
                disponibilidadXML.Domingo2 = cbDomingo2.IsToggled;
                disponibilidadXML.Comentarios = txtComentarios.Text;
                disponibilidadXML.FechaDisponibilidad = DateTime.Today.GetDateSiguienteLunes();

                disponibilidad.InfoDisponibilidad = disponibilidadXML;
                disponibilidad.FechaDisponibilidad = DateTime.Today.GetDateSiguienteLunes();

                //Serializamos la clase disponibilidadXMLDto y lo pasamos a un string
                return disponibilidad;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
            finally
            {
                if (stringWriter != null)
                {
                    stringWriter.Close();
                }
            }
        }
        #endregion

        #region Commands
        private void BtnReiniciar_Clicked(object sender, EventArgs e)
        {
            cbViernes.IsToggled = false;
            cbSabado1.IsToggled = false;
            cbSabado2.IsToggled = false;
            cbDomingo1.IsToggled = false;
            cbDomingo2.IsToggled = false;
            txtComentarios.Text = String.Empty;
        }

        private async void BtnEnviar_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool respuesta;
                //Le pedimos confirmación para enviar la disponibilidad
                respuesta = await DisplayAlert("Confirmación", "¿Seguro que quieres enviar la disponibilidad?", "CONTINUAR", "CANCELAR");

                if (respuesta)
                {
                    DisponibilidadDTO disponibilidad = RellenarDisponibilidad();

                    int filasInsertadas = DatabaseHelper.InsertarDisponibilidad(disponibilidad);
                    if (filasInsertadas != 1)
                    {
                        await DisplayAlert("ERROR", "Error al enviar la disponibilidad", "ACEPTAR");
                    }
                    else
                    {
                        await DisplayAlert("OK", "Disponibilidad enviada correctamente", "ACEPTAR");
                        SetValorControles();
                        DeshabilitarControles();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al enviar la disponibilidad: " + ex.Message + " EN " + ex.StackTrace);
            }
        }

        protected override void OnAppearing()
        {
            if (disponibilidadEnviada)
                DisplayAlert("INFO", "Ya has enviado la disponibilidad de la semana siguiente", "ACEPTAR");
        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
        #endregion
    }
}