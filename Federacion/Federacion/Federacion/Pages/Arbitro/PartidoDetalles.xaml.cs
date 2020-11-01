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
    public partial class PartidoDetalles : ContentPage
    {
        List<PartidoDTO> listadoPartidos;
        PartidoDTO partidoActivo;
        public PartidoDetalles(PartidoDTO partido)
        {
            try
            {
                InitializeComponent();
                partidoActivo = partido;

                DateTime lunesAnterior, lunesSiguiente;
                lunesAnterior = DateTime.Today.GetDateAnteriorLunes();
                lunesSiguiente = DateTime.Today.GetDateSiguienteLunes();

                SetValoresControles(partido);
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", ex.Message, "ACEPTAR");
            }
        }

        private void SetValoresControles(PartidoDTO partido)
        {
            try
            {
                txtEquipos.Text = partido.Equipos.Trim();
                txtCategoria.Text = partido.DescripcionCategoria.Trim();
                txtFecha.Text = partido.FechaPartido.ToString("dd/MM/yy HH:mm");
                txtUbicacion.Text = partido.Ubicacion.Trim();
                txtFuncion.Text = partido.DescripcionFuncion.Trim();
                txtObservaciones.Text = partido.Observaciones.Trim();
                listaArbitros.ItemsSource = partido.listaArbitros;

                /// Para poder enviar el resultado se debe cumplir:
                /// 1. Que no este enviado ya
                /// 2. Que sea el arbitro principal el que lo envie
                /// 3. Que se haya jugado el partido
                /// En caso contrario bloquearemos el boton

                /// Se compara el idFuncion para ver si el arbitro tenia de funcion arbitro principal. Se deberia hacer consulta en BD 
                /// pero no hay algo que asegure que el numero 1 va a ser el principal en BD. 
                /// Se podria buscar el que tenga en descripcion "Principal" pero tampoco es fiable. Lo dejo hardcodeado a 1 y ya
                if (partido.IdFuncion != 1 || partido.FechaPartido > DateTime.Now)
                {
                    entryLocal.IsEnabled = false;
                    entryVisitante.IsEnabled = false;
                    btnEnviar.IsEnabled = false;
                }

                if (!String.IsNullOrEmpty(partido.Resultado))
                {
                    string[] resultados = partido.Resultado.Split('-');
                    entryLocal.Text = resultados[0];
                    entryVisitante.Text = resultados[1];
                    entryLocal.IsEnabled = false;
                    entryVisitante.IsEnabled = false;
                    btnEnviar.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", "Error al cargar la pantalla: " + ex.Message, "ACEPTAR");
            }

        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void EnviarResultado_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool respuesta = await DisplayAlert("INFO", "¿Desea enviar el resultado?", "CONTINUAR", "CANCELAR");
                if (respuesta)
                {
                    if (!String.IsNullOrEmpty(entryLocal.Text) && !String.IsNullOrEmpty(entryVisitante.Text))
                    {
                        if (DatabaseHelper.EnviarResultado(partidoActivo.IdPartido, entryLocal.Text + " - " + entryVisitante.Text) == 1)
                        {
                            await DisplayAlert("OK", "Resultado enviado correctamente", "ACEPTAR");
                        }
                        else
                        {
                            await DisplayAlert("ERROR", "Error al enviar el resultado", "ACEPTAR");
                        }
                    }
                    else
                    {
                        await DisplayAlert("ERROR", "Los campos no pueden estar vacios", "ACEPTAR");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", ex.Message, "ACEPTAR");
            }


        }
    }
}