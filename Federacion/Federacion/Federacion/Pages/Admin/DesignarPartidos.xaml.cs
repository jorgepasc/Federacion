using Federacion.Datos;
using Federacion.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Federacion.Pages.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DesignarPartidos : ContentPage
    {
        ObservableCollection<PartidoDTO> listaPartidosAsignables = new ObservableCollection<PartidoDTO>();
        ObservableCollection<PartidoDTO> listaPartidosAsignados = new ObservableCollection<PartidoDTO>();
        ArbitroDTO ArbitroDesignando;
        public DesignarPartidos(ArbitroDTO arbitro)
        {
            try
            {
                InitializeComponent();
                ArbitroDesignando = arbitro;
                pageDesignarPartidos.Title = "Designación partidos de " + arbitro.Alias + ": ";                
                lblAlias.Text = arbitro.Alias;
                
                //Cargamos la disponibilidad del arbitro (de esta semana)
                DisponibilidadDTO disponibilidad = DatabaseHelper.CargarDisponibilidad(arbitro.IdArbitro, DateTime.Today.GetDateAnteriorLunes());
                if(disponibilidad != null)
                    SetValorControles(disponibilidad);


                //Cargamos los partidos que quedan por designar 
                listaPartidosAsignables = new ObservableCollection<PartidoDTO>(DatabaseHelper.GetPartidosDisponibles(ArbitroDesignando.IdArbitro));
                if (listaPartidosAsignables != null && listaPartidosAsignables.Any())
                {
                    listaAsignables.ItemsSource = listaPartidosAsignables;
                }

                //Cargamos los partidos que ya tiene designados
                listaPartidosAsignados = new ObservableCollection<PartidoDTO>(DatabaseHelper.CargarPartidos(ArbitroDesignando.IdArbitro, DateTime.Today.GetDateAnteriorLunes(), DateTime.Today.GetDateSiguienteLunes(), false));
                if (listaPartidosAsignados != null && listaPartidosAsignados.Any())
                {
                    listaAsignados.ItemsSource = listaPartidosAsignados;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", ex.Message, "ACEPTAR");
                lblInfo.Text = "ERROR " + ex.Message; 
            }
        }

        /// <summary>
        /// Traduce los valores de disponibilidad de base de datos a colores en la pantalla
        /// </summary>
        /// <param name="disponibilidad"></param>
        private void SetValorControles(DisponibilidadDTO disponibilidad)
        {
            try
            {
                if (disponibilidad.InfoDisponibilidad.Viernes)
                    lblViernes.BackgroundColor = Color.FromHex("#dafa7a");
                else
                    lblViernes.BackgroundColor = Color.FromHex("#ff6666");

                if (disponibilidad.InfoDisponibilidad.Sabado1)
                    lblSabado1.BackgroundColor = Color.FromHex("#dafa7a");
                else
                    lblSabado1.BackgroundColor = Color.FromHex("#ff6666");

                if (disponibilidad.InfoDisponibilidad.Sabado2)
                    lblSabado2.BackgroundColor = Color.FromHex("#dafa7a");
                else
                    lblSabado2.BackgroundColor = Color.FromHex("#ff6666");

                if (disponibilidad.InfoDisponibilidad.Domingo1)
                    lblDomingo1.BackgroundColor = Color.FromHex("#dafa7a");
                else
                    lblDomingo1.BackgroundColor = Color.FromHex("#ff6666");

                if (disponibilidad.InfoDisponibilidad.Domingo2)
                    lblDomingo2.BackgroundColor = Color.FromHex("#dafa7a");
                else
                    lblDomingo2.BackgroundColor = Color.FromHex("#ff6666");

                string comentarios = disponibilidad.InfoDisponibilidad.Comentarios;
                if(String.IsNullOrEmpty(comentarios))
                   lblComentarios.Text = "Comentarios: ";
                else
                    lblComentarios.Text = "Comentarios: " + comentarios;
            }
            catch(Exception ex)
            {
                DisplayAlert("ERROR", "Error al cargar la disponibilidad " + ex.Message + System.Environment.NewLine + ex.StackTrace, "ACEPTAR");
            }
        }
        #region Commands

        private void PartidoAsignable_Tapped(object sender, EventArgs e) //lista1 tocada, pasamos a lista 2
        {
            PartidoDTO partidoSelected = (PartidoDTO)listaAsignables.SelectedItem;
            partidoSelected.IsVisibleDesasignar = true;
            partidoSelected.IsVisiblePickerFuncion = true;
            partidoSelected.IsVisibleTextoFuncion = false;
            listaPartidosAsignados.Add(partidoSelected);
            listaPartidosAsignables.Remove(partidoSelected);

            listaAsignados.ItemsSource = listaPartidosAsignados.OrderBy(f => f.FechaPartido);//refrescamos las listas
            listaAsignables.ItemsSource = listaPartidosAsignables.OrderBy(f => f.FechaPartido);
        }

        private void PartidoAsignado_Tapped(object sender, EventArgs e) //lista2 tocada pasamos a lista 1
        {
            PartidoDTO partidoSelected = (PartidoDTO)listaAsignados.SelectedItem;

            //Si ya estaban asignados de antes no lo puede desasignar tocando en la pantalla (porque no se actualiza bd)
            if (partidoSelected.IsVisibleDesasignar)
            {
                listaPartidosAsignados.Remove(partidoSelected);
                listaPartidosAsignables.Add(partidoSelected);

                listaAsignados.ItemsSource = listaPartidosAsignados.OrderBy(f => f.FechaPartido);//refrescamos la lista
                listaAsignables.ItemsSource = listaPartidosAsignables.OrderBy(f => f.FechaPartido);
            }
        }

        private async void BtnSalir_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        /// <summary>
        /// Borra de la tabla PartidoArbitrado todos los registros de la lista de designados (derecha)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnDesasignar_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool todosCorrectos = true;
                bool respuesta = await DisplayAlert("CONFIRMAR", "¿Desea desasignar TODOS los partidos del árbitro?", "CONTINUAR", "CANCELAR");
                if (respuesta)
                {
                    IEnumerable<PartidoDTO> listaDesasignar = (IEnumerable<PartidoDTO>)listaAsignados.ItemsSource;
                    var lista = listaDesasignar.Where(f => f.IsVisiblePickerFuncion == false);
                    foreach (PartidoDTO partido in lista)
                    {
                        int rowsAffected = DatabaseHelper.DesasignarPartido(partido.IdPartido, ArbitroDesignando.IdArbitro);
                        if(rowsAffected != 1)
                        {
                            todosCorrectos = false;
                        }
                    }

                    if (!todosCorrectos)
                    {
                        await DisplayAlert("INFO", "Es posible que no se hayan desasignado todos los partidos correctamente. Vuelva a intentarlo mas tarde", "ACEPTAR");
                    }
                    else
                    {
                        await DisplayAlert("OK", "Todos los partidos han sido desasignados correctamente", "ACEPTAR");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", "Error al desasignar: " + ex.Message, "ACEPTAR");
                Log.Log.LogMessageToFile("Error al desasignar" + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// Al darle a guardar se insertaran en la tabla PartidoArbitrado los partidos que haya elegido el usuario (lista derecha)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool todosCorrectos = true;
                //En la lista partidos asignados estaran solo los que haya aniadido en esta "sesion" los que ya estaban asignados no los volvera a meter
                var lista = listaPartidosAsignados.Where(f => f.IsVisiblePickerFuncion == true);
                foreach (PartidoDTO partido in lista)
                {
                    int rows = DatabaseHelper.GuardarPartidosDesignados(partido, ArbitroDesignando.IdArbitro);
                    if(rows != 1)
                    {
                        todosCorrectos = false;
                    }
                }                
                if (!todosCorrectos)
                {
                   await DisplayAlert("INFO", "Es posible que no se hayan designado todos los partidos correctamente. Vuelva a intentarlo mas tarde", "ACEPTAR");
                }
                else
                {
                    await DisplayAlert("OK", "Todos los partidos han sido designados correctamente", "ACEPTAR");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ERROR", "Error al guardar los partidos: " + ex.Message, "ACEPTAR");
                Log.Log.LogMessageToFile("Error al guardar los partidos: " + ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }
        
        //Searchbar de partidos asignables
        private void SearchBar_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            SearchBar bar = (SearchBar)sender;
            string filtro = bar.Text.ToUpper();
            listaAsignables.ItemsSource = listaPartidosAsignables.Where(f => f.AbreviaturaCategoria.ToUpper().Contains(filtro));
        }

        //Searchbar de partidos asignados
        private void SearchBar_TextChanged_1(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                SearchBar bar = (SearchBar)sender;
                string filtro = bar.Text.ToUpper();
                listaAsignados.ItemsSource = listaPartidosAsignados.Where(f => f.AbreviaturaCategoria.ToUpper().Contains(filtro));
            }
            catch (Exception ex)
            {
                DisplayAlert("ERROR", ex.Message, "ACEPTAR");
            }
        }
        
        ///// <summary>
        ///// Muestra en una alerta los arbitros ya designados para el partido
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private async void BtnMasInfo_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //buscamos el partido desde el que se ha llamado
        //        ImageButton imgBtn = (ImageButton)sender;                
        //        Element element = imgBtn.Parent;                
        //        ListViewItem a;
                
        //        while(element.GetType() != item.GetType())
        //        {
        //            element = element.Parent;
        //        }
                    

                
        //        PartidoDTO partido = (PartidoDTO)listaAsignados.SelectedItem;
        //        string listadoArbitros = String.Empty;
        //        foreach(ArbitroDTO arbi in partido.listaArbitros)
        //        {
        //            listadoArbitros += String.Format("{0} - {1}{2}", arbi.Alias.ToUpper(), arbi.NombreCompleto, Environment.NewLine); 
        //        }
        //        await DisplayAlert("LISTA ÁRBITROS", listadoArbitros, "ACEPTAR");
        //    }catch(Exception ex)
        //    {
        //        await DisplayAlert("ERROR", ex.Message, "ACEPTAR");
        //    }
        //}
        #endregion


    }
}