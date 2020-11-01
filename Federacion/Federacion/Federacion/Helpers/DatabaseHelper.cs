using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Federacion.Datos;
using Xamarin.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Federacion.Helpers
{
    static class DatabaseHelper
    {
        public static SqlConnection connection;

        #region General

        public static void InitializeAutomapper()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Crea la conexion con el connectionString del servidor de Azure
        /// </summary>
        public static void CrearConexion()
        {
            try
            {
                string connectionString;
                connectionString = @"";
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Login

        /// <summary>
        /// Consulta la base de datos para ver si coinciden los datos de login
        /// </summary>
        /// <param name="usuario">Usuario escrito en pantalla de login</param>
        /// <returns>Si el login es correcto rellenara UsuarioActivoDTO</returns>
        public static bool ComprobarLogin(string usuario, string pass)
        {
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                cmd = new SqlCommand("SELECT IdUsuario, ISNULL(IdArbitro, 0), ISNULL(IdAdministrador, 0), " +
                    "Usuario, Password, ISNULL(IsAdmin, 0)" +
                    " FROM Usuario WHERE Usuario = @param1 AND Password = @param2", connection);
                //construimos el comando con los valores introducidos por el usuario
                cmd.Parameters.AddWithValue("@param1", usuario);
                cmd.Parameters.AddWithValue("@param2", pass);
                dataReader = cmd.ExecuteReader();

                //Si obtengo resultados los guardo en UsuarioActivo, si no pongo usuario a 0 y desde el login si esta a 0 mando mensaje de error
                if (dataReader.Read())
                {
                    UsuarioActivoDTO.IdUsuario = dataReader.GetInt32(0);
                    UsuarioActivoDTO.IdArbitro = dataReader.GetInt32(1);
                    UsuarioActivoDTO.IdAdministrador = dataReader.GetInt32(2);
                    UsuarioActivoDTO.Usuario = dataReader.GetString(3);
                    UsuarioActivoDTO.Password = dataReader.GetString(4);
                    UsuarioActivoDTO.IsAdmin = dataReader.GetBoolean(5);
                    

                    if (UsuarioActivoDTO.IsAdmin)
                        return true;
                    else
                        return false;
                }
                else
                {
                    UsuarioActivoDTO.IdUsuario = 0;
                    throw new Exception(" Usuario o contraseña incorrectos ");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
        }

        #endregion

        #region Nominas
        /// <summary>
        /// Obtiene todas las nóminas de un arbitro de este año
        /// </summary>
        /// <param name="idArbitro"></param>
        /// <returns></returns>
        public static List<NominaDTO> CargarNominasHistoricoByIdArbitro(int idArbitro)
        {
            List<NominaDTO> listadoNominas = new List<NominaDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                NominaDTO nomina;
                cmd = new SqlCommand("SELECT IdNomina, Fecha, Total FROM Nomina WHERE IdArbitro = @IdArbitro AND Fecha >= @Fecha order by Fecha", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IdArbitro", idArbitro);
                cmd.Parameters.AddWithValue("@Fecha", DateTime.Today.GetLast1Septiembre());

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto  
                {
                    nomina = new NominaDTO();

                    nomina.IdNomina = dataReader.GetInt32(0);
                    nomina.FechaNomina = dataReader.GetDateTime(1);
                    nomina.Total = dataReader.GetDecimal(2);
                    nomina.PeriodoNomina = String.Format("Del 1 al {0} de {1} de {2}", nomina.FechaNomina.GetLastDayOfMonth().ToString("dd"), nomina.FechaNomina.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")), nomina.FechaNomina.ToString("yyyy"));

                    listadoNominas.Add(nomina);
                }
                return listadoNominas;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Obtiene todas las nóminas de un mes
        /// </summary>
        /// <param name="idArbitro"></param>
        /// <returns></returns>
        public static List<NominaDTO> CargarNominasHistorico(int mes)
        {
            List<NominaDTO> listadoNominas = new List<NominaDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                NominaDTO nomina;
                cmd = new SqlCommand("select IdNomina, Fecha, Total, Alias from Nomina join Arbitro on Nomina.IdArbitro = Arbitro.IdArbitro where DATEPART(month, Fecha) = @Mes", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Mes", mes);

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto  
                {
                    nomina = new NominaDTO();

                    nomina.IdNomina = dataReader.GetInt32(0);
                    nomina.FechaNomina = dataReader.GetDateTime(1);
                    nomina.Total = dataReader.GetDecimal(2);
                    nomina.PeriodoNomina = String.Format("Del 1 al {0} de {1} de {2}", nomina.FechaNomina.GetLastDayOfMonth().ToString("dd"), nomina.FechaNomina.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")), nomina.FechaNomina.ToString("yyyy"));
                    nomina.Alias = dataReader.GetString(3);

                    listadoNominas.Add(nomina);
                }
                return listadoNominas;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Comprueba si hay registros en la tabla nomina de la base de datos de este mes
        /// </summary>
        /// <param name="mes"></param>
        /// <returns></returns>
        internal static bool ComprobarNominasCalculadas(int mes)
        {
            bool IsNominasCalculadas = false;
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                cmd = new SqlCommand("SELECT IdNomina, Fecha from Nomina where DATEPART(MONTH, Fecha) = @Mes", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Mes", mes);

                dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    //si tiene datos devolvemos true (ya estan calculadas)
                    IsNominasCalculadas = true;
                }
                return IsNominasCalculadas;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al comprobar el calculo de nominas " + ex.Message + System.Environment.NewLine + ex.StackTrace
                    + System.Environment.NewLine + System.Environment.NewLine);
                throw new Exception("Error al comprobar el cálculo de nóminas: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Agrupa todos los partidos arbitrados de un mes por arbitro y suma sus importes 
        /// (que se ha calculado previamente en designar partido gracias al IdCategoria e IdFuncion)
        /// </summary>
        /// <param name="primeroDeMes"></param>
        /// <param name="ultimoDeMes"></param>
        /// <returns></returns>
        internal static List<NominaDTO> CalcularNominas(DateTime primeroDeMes, DateTime ultimoDeMes)
        {
            List<NominaDTO> listadoNominas = new List<NominaDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                NominaDTO nomina;
                cmd = new SqlCommand("select IdArbitro, sum(Importe) as TotalMes from PartidoArbitrado " +
                    "JOIN Partido p on p.IdPartido = PartidoArbitrado.IdPartido " +
                    "where p.Fecha >= @PrimeroDeMes AND p.Fecha <= @UltimoDeMes " +
                    "group by IdArbitro", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@PrimeroDeMes", primeroDeMes);
                cmd.Parameters.AddWithValue("@UltimoDeMes", ultimoDeMes);

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto  
                {
                    nomina = new NominaDTO();

                    nomina.FechaNomina = primeroDeMes;
                    nomina.IdArbitro = dataReader.GetInt32(0);
                    nomina.Total = dataReader.GetDecimal(1);

                    listadoNominas.Add(nomina);
                }
                return listadoNominas;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw new Exception("Error al calcular las nóminas: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Inserta en BD la nomina dada por parametro
        /// </summary>
        /// <param name="nomina"></param>
        /// <returns></returns>
        internal static int InsertarNomina(NominaDTO nomina)
        {
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            try
            {
                //Prepara el comando
                cmd = new SqlCommand("INSERT INTO Nomina VALUES (@Fecha, @Total, @IdArbitro)", connection);

                cmd.Parameters.AddWithValue("@Fecha", nomina.FechaNomina);
                cmd.Parameters.AddWithValue("@Total", nomina.Total);
                cmd.Parameters.AddWithValue("@IdArbitro", nomina.IdArbitro);

                //Ejecuta el insert
                adapter = new SqlDataAdapter();
                adapter.InsertCommand = cmd;
                return adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al insertar nomina en BD:" + ex.Message + System.Environment.NewLine + ex.StackTrace
                    + System.Environment.NewLine + System.Environment.NewLine);
                throw new Exception("Error al insertar las nóminas: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (adapter != null)
                {
                    adapter.Dispose();
                }
            }
        }
        #endregion 

        #region Disponibilidad

        /// <summary>
        /// Carga la disponibilidad del arbitro dado por parametro
        /// </summary>
        /// <returns>Disponibilidad o null</returns>
        public static DisponibilidadDTO CargarDisponibilidad(int idArbitro, DateTime fecha)
        {
            DisponibilidadDTO disponibilidad = new DisponibilidadDTO();
            //Para bd
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;

            //Para el xml
            XmlSerializer serializer = null;
            try
            {
                cmd = new SqlCommand("SELECT IdDisponibilidad, Disponibilidad, Fecha, IdArbitro FROM Disponibilidad " +
                    "WHERE IdArbitro = @param1 AND cast(Fecha as date) = @param2", connection);
                //construimos el comando con los valores introducidos por el usuario
                cmd.Parameters.AddWithValue("@param1", idArbitro);
                cmd.Parameters.AddWithValue("@param2", fecha);

                dataReader = cmd.ExecuteReader();

                if (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto  
                {
                    DisponibilidadXMLDTO infoDisponibilidadDTO = new DisponibilidadXMLDTO();
                    serializer = new XmlSerializer(typeof(DisponibilidadXMLDTO));

                    //Leemos resultados de bd
                    disponibilidad.IdDisponibilidad = dataReader.GetInt32(0);
                    disponibilidad.FechaDisponibilidad = dataReader.GetDateTime(2);
                    disponibilidad.IdArbitro = dataReader.GetInt32(3);

                    string infoDisponibilidadXML = dataReader.GetString(1);
                    using (TextReader reader = new StringReader(infoDisponibilidadXML))
                    {
                        infoDisponibilidadDTO = (DisponibilidadXMLDTO)serializer.Deserialize(reader);
                    }

                    disponibilidad.InfoDisponibilidad = infoDisponibilidadDTO;

                    return disponibilidad;
                }
                else//No tiene datos
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al cargar la disponibilidad: " + ex.Message + " EN " + ex.StackTrace);
                throw new Exception("Error al cargar la disponibilidad: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
        }

        /// <summary>
        /// Inserta la disponibilidad en base de datos
        /// </summary>
        /// <param name="disponibilidad"></param>
        /// <param name="fechaDisponibilidad"></param>
        /// <returns>Numero de filas insertadas</returns>
        public static int InsertarDisponibilidad(DisponibilidadDTO disponibilidad)
        {
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            StringWriter stringWriter = null;
            try
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    //Deserializamos la clase XML a un String para insertarlo en BD
                    stringWriter = new StringWriter();
                    XmlSerializer serializer = new XmlSerializer(typeof(DisponibilidadXMLDTO));
                    serializer.Serialize(stringWriter, disponibilidad.InfoDisponibilidad);

                    //Prepara el comando
                    cmd = new SqlCommand("INSERT INTO Disponibilidad VALUES (@param1, @param2, @param3)", connection);
                    cmd.Parameters.AddWithValue("@param1", stringWriter.ToString());
                    cmd.Parameters.AddWithValue("@param2", disponibilidad.FechaDisponibilidad.GetDateAnteriorLunes().AddMinutes(1));
                    cmd.Parameters.AddWithValue("@param3", UsuarioActivoDTO.IdArbitro);

                    //Ejecuta el insert
                    adapter = new SqlDataAdapter();
                    adapter.InsertCommand = cmd;
                    return adapter.InsertCommand.ExecuteNonQuery();
                }
                else //Error con la conexión - Connection not open
                {
                    throw new Exception("Error con la conexión - Connection not open");
                }
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al insertar disponibilidad en BD:" + ex.Message + " EN " + ex.StackTrace);
                throw new Exception("Inserción incorrecta: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (adapter != null)
                {
                    adapter.Dispose();
                }
                if (stringWriter != null)
                {
                    stringWriter.Close();
                }
            }
        }
        #endregion

        #region Partidos
        /// <summary>
        /// Devuelve todos los partidos del árbitro activo en un rango de fechas
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="IsConsultarNominas">Reutilizamos el método en la pantalla de las nóminas, pero nos saltamos la carga de los detalles</param>
        /// <returns></returns>
        public static List<PartidoDTO> CargarPartidos(int IdArbitro, DateTime fechaDesde, DateTime fechaHasta, bool IsConsultarNominas)
        {
            List<PartidoDTO> listadoPartidos = new List<PartidoDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                PartidoDTO partido;
                cmd = new SqlCommand("GetDatosPartido", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdArbitro", IdArbitro);
                cmd.Parameters.AddWithValue("@FechaDesde", fechaDesde);
                cmd.Parameters.AddWithValue("@FechaHasta", fechaHasta.AddDays(-1));

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto  
                {
                    partido = new PartidoDTO();

                    partido.IdPartido = dataReader.GetInt32(0);
                    partido.FechaPartido = dataReader.GetDateTime(1);
                    partido.EquipoLocal = dataReader.GetString(2);
                    partido.EquipoVisitante = dataReader.GetString(3);
                    partido.Equipos = partido.EquipoLocal + " - " + partido.EquipoVisitante;
                    partido.IdCategoria = dataReader.GetInt32(4);
                    partido.DescripcionCategoria = dataReader.GetString(5);
                    partido.AbreviaturaCategoria = dataReader.GetString(6);
                    partido.Ubicacion = dataReader.GetString(7);
                    partido.Observaciones = dataReader.GetString(8);
                    partido.IdPartidoArbitrado = dataReader.GetInt32(9);
                    partido.IdFuncion = dataReader.GetInt32(10);
                    partido.DescripcionFuncion = dataReader.GetString(11);
                    partido.Importe = dataReader.GetDecimal(12);
                    partido.IdArbitro = dataReader.GetInt32(13);
                    partido.Alias = dataReader.GetString(14);
                    partido.NumTelefono = dataReader.GetString(15);
                    partido.Resultado = dataReader.GetString(16);

                    //Para pantalla designar partidos (visibilidad de funcion)
                    partido.IsVisibleTextoFuncion = true;
                    partido.IsVisiblePickerFuncion = false;
                    partido.Funcion = new FuncionDTO(partido.IdFuncion, partido.DescripcionFuncion);
                    partido.listaFunciones = CargarTodasFunciones();

                    if (!IsConsultarNominas) //En la pantalla consultar nominas no cambiamos de color ni mostramos los detalles
                    {
                        if (partido.FechaPartido < DateTime.Now)
                        {
                            partido.Color = "#05AD29";//Si el partido ya ha sido jugado, le ponemos color verde para que se vea en la pantalla como completado
                            partido.ColorLetra = "#fff";
                            partido.ImageSource = "flechablanca.png";
                        }
                        else
                        {
                            partido.Color = "#fff";//Si el partido todavía no se ha jugado le ponemos color blanco
                            partido.ColorLetra = "#000000";
                            partido.ImageSource = "flechaderecha.png";
                        }
                        partido.listaArbitros = GetListaArbitrosPartido(partido.IdPartido);
                    }
                    listadoPartidos.Add(partido);
                }
                return listadoPartidos;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al cargar los partidos: " + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw new Exception("Error al cargar los partidos: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Carga toda la tabla Funcion en una lista (para el Picker de DesignarPartidos)
        /// </summary>
        /// <returns></returns>
        private static List<FuncionDTO> CargarTodasFunciones()
        {
            List<FuncionDTO> listaFunciones = new List<FuncionDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                FuncionDTO funcion;
                //Estamos recorriendo un dataReader (en CargarPartidos) y necesitamos otro para cargar las funciones, 
                //para eso se pone el MultipleActiveResultSets=true en el connectionString
                cmd = new SqlCommand("SELECT IdFuncion, Descripcion FROM Funcion", connection);
                cmd.CommandType = CommandType.Text;

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto                  
                {
                    funcion = new FuncionDTO();

                    funcion.IdFuncion = dataReader.GetInt32(0);
                    funcion.Descripcion = dataReader.GetString(1);

                    listaFunciones.Add(funcion);
                }
                return listaFunciones;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al cargar funciones" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw new Exception("Error al cargar las funciones: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Devuelve todos los árbitros que han participado en determinado partido
        /// </summary>
        /// <param name="idPartido"></param>
        /// <returns></returns>
        private static List<ArbitroDTO> GetListaArbitrosPartido(int idPartido)
        {
            List<ArbitroDTO> listaArbitros = new List<ArbitroDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                ArbitroDTO arbitro;
                //Estamos recorriendo un dataReader (en CargarPartidos) y necesitamos otro para cargar los árbitros, 
                //para eso se pone el MultipleActiveResultSets=true en el connectionString
                cmd = new SqlCommand("GetArbitrosPartido", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPartido", idPartido);

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto                  
                {
                    arbitro = new ArbitroDTO();

                    arbitro.IdArbitro = dataReader.GetInt32(0);
                    arbitro.Alias = dataReader.GetString(1);
                    arbitro.Nombre = dataReader.GetString(2);
                    arbitro.Apellidos = dataReader.GetString(3);
                    arbitro.NumTelefono = dataReader.GetString(4);
                    arbitro.Funcion = dataReader.GetString(5);

                    listaArbitros.Add(arbitro);
                }

                return listaArbitros;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al cargar los árbitros: " + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw new Exception("Error al cargar los árbitros: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Actualiza el campo resultado de un partido con los datos introducidos por pantalla
        /// </summary>
        /// <param name="partidoActivo"></param>
        /// <param name="resultado"></param>
        /// <returns></returns>
        public static int EnviarResultado(int idPartido, string resultado)
        {
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            try
            {
                cmd = new SqlCommand("UPDATE Partido SET Resultado = @Resultado WHERE IdPartido = @IdPartido", connection);
                cmd.Parameters.AddWithValue("@Resultado", resultado);
                cmd.Parameters.AddWithValue("@IdPartido", idPartido);

                //Ejecuta el update
                adapter = new SqlDataAdapter();
                adapter.UpdateCommand = cmd;
                return adapter.UpdateCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (adapter != null)
                {
                    adapter.Dispose();
                }
            }
        }

        #endregion

        #region Informes
        /// <summary>
        /// Carga los informes del año vigente de un determinado árbitro
        /// </summary>
        /// <param name="idArbitro"></param>
        /// <returns></returns>
        internal static List<InformeDTO> CargarInformes(int idArbitro)
        {
            List<InformeDTO> listadoInformes = new List<InformeDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                InformeDTO informe;

                cmd = new SqlCommand("SELECT IdInforme, FechaInforme, ISNULL(TextoInforme, '') as TextoInforme, IsFavorable FROM Informe WHERE IdArbitro = @IdArbitro order by FechaInforme", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IdArbitro", idArbitro);

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    informe = new InformeDTO();

                    informe.IdInforme = dataReader.GetInt32(0);
                    informe.FechaInforme = dataReader.GetDateTime(1);
                    informe.TextoInforme = dataReader.GetString(2);
                    informe.IsFavorable = dataReader.GetBoolean(3);

                    if (informe.IsFavorable)//Si el informe es favorable ponemos color verde, si es desfavorable ponemos color naranja
                    {
                        informe.Color = "#05AD29";
                    }
                    else
                    {
                        informe.Color = "#BB2912";
                    }

                    listadoInformes.Add(informe);
                }
                return listadoInformes;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Inserta en BD el informe dado por parámetro
        /// </summary>
        /// <param name="informe"></param>
        /// <returns></returns>
        internal static int CrearInforme(InformeDTO informe)
        {
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            try
            {
                //Prepara el comando
                cmd = new SqlCommand("INSERT INTO Informe VALUES (@IdArbitro, @FechaInforme , @TextoInforme , @IsFavorable)", connection);

                cmd.Parameters.AddWithValue("@IdArbitro", informe.IdArbitro);
                cmd.Parameters.AddWithValue("@FechaInforme", informe.FechaInforme);
                cmd.Parameters.AddWithValue("@TextoInforme", informe.TextoInforme);
                cmd.Parameters.AddWithValue("@IsFavorable", informe.IsFavorable);

                //Ejecuta el insert
                adapter = new SqlDataAdapter();
                adapter.InsertCommand = cmd;
                return adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al insertar informe en BD:" + ex.Message + System.Environment.NewLine + ex.StackTrace
                    + System.Environment.NewLine + System.Environment.NewLine);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (adapter != null)
                {
                    adapter.Dispose();
                }
            }
        }
        #endregion

        #region Designacion

        /// <summary>
        /// Obtiene una lista de los arbitros con algo de disponibilidad, para seleccionar uno y designarle sus partidos
        /// </summary>
        /// <param name="idPartido"></param>
        /// <returns></returns>
        public static List<ArbitroDTO> GetListaArbitrosDesignar()
        {
            List<ArbitroDTO> listaArbitros = new List<ArbitroDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                ArbitroDTO arbitro;
                cmd = new SqlCommand("GetNumPartidosArbitro", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaDesde", DateTime.Today.GetDateAnteriorLunes());
                cmd.Parameters.AddWithValue("@FechaHasta", DateTime.Today.GetDateSiguienteLunes());

                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto                  
                {
                    arbitro = new ArbitroDTO();

                    arbitro.IdArbitro = dataReader.GetInt32(0);
                    arbitro.Alias = dataReader.GetString(1);
                    arbitro.Nombre = dataReader.GetString(2);
                    arbitro.Apellidos = dataReader.GetString(3);
                    arbitro.NumPartidos = dataReader.GetInt32(4);
                    arbitro.NombreCompleto = arbitro.Nombre + " " + arbitro.Apellidos;

                    listaArbitros.Add(arbitro);
                }
                return listaArbitros;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw new Exception("Error al obtener los árbitros: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Obtiene los partidos que cumplen la disponibilidad enviada por el árbitro de un fin de semana
        /// </summary>
        /// <param name="IdArbitro"></param>
        public static List<PartidoDTO> GetPartidosDisponibles(int IdArbitro)
        {
            List<PartidoDTO> listaPartidos = new List<PartidoDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                PartidoDTO partido;
                cmd = new SqlCommand("GetPartidosDisponibles", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdArbitro", IdArbitro);
                cmd.Parameters.AddWithValue("@FechaDesde", DateTime.Today.GetDateAnteriorLunes());
                cmd.Parameters.AddWithValue("@FechaHasta", DateTime.Today.GetDateSiguienteLunes());

                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto                  
                {
                    partido = new PartidoDTO();

                    partido.IdPartido = dataReader.GetInt32(0);
                    partido.FechaPartido = dataReader.GetDateTime(1);
                    partido.EquipoLocal = dataReader.GetString(2);
                    partido.EquipoVisitante = dataReader.GetString(3);
                    partido.Equipos = partido.EquipoLocal + " - " + partido.EquipoVisitante;
                    partido.IdCategoria = dataReader.GetInt32(4);
                    partido.Ubicacion = dataReader.GetString(5);
                    partido.Observaciones = dataReader.GetString(6);
                    partido.DescripcionCategoria = dataReader.GetString(7);
                    partido.AbreviaturaCategoria = dataReader.GetString(8);
                    partido.listaArbitros = GetListaArbitrosPartido(partido.IdPartido);
                    partido.listaFunciones = CargarTodasFunciones();

                    listaPartidos.Add(partido);
                }
                return listaPartidos;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw new Exception("Error al cargar los partidos disponibles: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Introduce en BD los partidos elegidos por el administrador para un arbitro
        /// </summary>
        /// <param name="listaPartidosAsignados"></param>
        public static int GuardarPartidosDesignados(PartidoDTO partido, int idArbitro)
        {
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            try
            {
                //Prepara el comando
                cmd = new SqlCommand("INSERT INTO PartidoArbitrado VALUES (@IdPartido, @IdArbitro, @IdFuncion, @Importe)", connection);
                cmd.Parameters.AddWithValue("@IdPartido", partido.IdPartido);
                cmd.Parameters.AddWithValue("@IdArbitro", idArbitro);
                if (partido.Funcion != null)
                {
                    cmd.Parameters.AddWithValue("@IdFuncion", partido.Funcion.IdFuncion);
                    decimal importe = CalculaImporte(partido.IdCategoria, partido.Funcion.IdFuncion);
                    cmd.Parameters.AddWithValue("@Importe", importe);
                }
                else
                {
                    throw new Exception("Error al guardar: Selecciona la función del árbitro");
                }

                //Ejecuta el insert
                adapter = new SqlDataAdapter();
                adapter.InsertCommand = cmd;
                return adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (adapter != null)
                {
                    adapter.Dispose();
                }
            }
        }

        /// <summary>
        /// Calcula el importe que se le paga al arbitro por un partido dependiendo de la categoria que se pite 
        /// y la funcion que se realice
        /// </summary>
        /// <param name="idCategoria"></param>
        /// <param name="idFuncion"></param>
        /// <returns></returns>
        private static decimal CalculaImporte(int idCategoria, int idFuncion)
        {
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                decimal importe = 0;
                cmd = new SqlCommand("SELECT Importe FROM CategoriaLinea WHERE IdCategoria = @IdCategoria and IdFuncion = @IdFuncion", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);
                cmd.Parameters.AddWithValue("@IdFuncion", idFuncion);

                dataReader = cmd.ExecuteReader();
                if (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto  
                {
                    importe = dataReader.GetDecimal(0);
                }
                return importe;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("" + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla PartidoArbitrado
        /// </summary>
        /// <param name="idPartido"></param>
        /// <param name="idArbitro"></param>
        public static int DesasignarPartido(int idPartido, int idArbitro)
        {
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            try
            {
                //Prepara el comando
                cmd = new SqlCommand("DELETE FROM PartidoArbitrado WHERE IdPartido = @IdPartido AND IdArbitro = @IdArbitro", connection);
                cmd.Parameters.AddWithValue("@IdPartido", idPartido);
                cmd.Parameters.AddWithValue("@IdArbitro", idArbitro);

                //Ejecuta el delete
                adapter = new SqlDataAdapter();
                adapter.DeleteCommand = cmd;
                return adapter.DeleteCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (adapter != null)
                {
                    adapter.Dispose();
                }
            }
        }
        #endregion

        #region Resultados
        /// <summary>
        /// Carga todos los partidos disputados un fin de semana con sus resultados
        /// </summary>
        /// <param name="lunesAnterior"></param>
        /// <param name="lunesSiguiente"></param>
        /// <returns></returns>
        internal static List<PartidoDTO> CargarResultados(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<PartidoDTO> listadoPartidos = new List<PartidoDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                PartidoDTO partido;
                cmd = new SqlCommand("GetResultadosPartidos", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaDesde", fechaDesde);
                cmd.Parameters.AddWithValue("@FechaHasta", fechaHasta);

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto  
                {
                    partido = new PartidoDTO();

                    partido.IdPartido = dataReader.GetInt32(0);
                    partido.FechaPartido = dataReader.GetDateTime(1);
                    partido.EquipoLocal = dataReader.GetString(2);
                    partido.EquipoVisitante = dataReader.GetString(3);
                    partido.Equipos = partido.EquipoLocal + " - " + partido.EquipoVisitante;
                    partido.IdCategoria = dataReader.GetInt32(4);
                    partido.DescripcionCategoria = dataReader.GetString(5);
                    partido.AbreviaturaCategoria = dataReader.GetString(6);
                    partido.Ubicacion = dataReader.GetString(7);
                    partido.Resultado = dataReader.GetString(8);

                    listadoPartidos.Add(partido);
                }
                return listadoPartidos;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al cargar los resultados: " + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }
        #endregion

        #region Altas
        /// <summary>
        /// Devuelve una lista con todos los administradores de la base de datos
        /// </summary>
        /// <returns></returns>
        internal static List<AdministradorDTO> CargarTodosAdmins()
        {
            List<AdministradorDTO> listadoAdmins = new List<AdministradorDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                AdministradorDTO admin;
                cmd = new SqlCommand("SELECT IdAdministrador, DNI, Nombre, Apellidos, NumTelefono, Alias FROM Administrador", connection);
                cmd.CommandType = CommandType.Text;

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto  
                {
                    admin = new AdministradorDTO();

                    admin.IdAdministrador = dataReader.GetInt32(0);
                    admin.DNI = dataReader.GetString(1);
                    admin.Nombre = dataReader.GetString(2);
                    admin.Apellidos = dataReader.GetString(3);
                    admin.NumTelefono = dataReader.GetString(4);
                    admin.Alias = dataReader.GetString(5);

                    listadoAdmins.Add(admin);
                }
                return listadoAdmins;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al cargar los admins: " + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }
        /// <summary>
        /// Devuelve una lista con todos los arbitros de base de datos
        /// </summary>
        internal static List<ArbitroDTO> CargarTodosArbitros()
        {
            List<ArbitroDTO> listadoArbis = new List<ArbitroDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                ArbitroDTO arbi;
                cmd = new SqlCommand("SELECT IdArbitro, Alias, DNI, Nombre, Apellidos, NumTelefono FROM Arbitro", connection);
                cmd.CommandType = CommandType.Text;

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto  
                {
                    arbi = new ArbitroDTO();

                    arbi.IdArbitro = dataReader.GetInt32(0);
                    arbi.Alias = dataReader.GetString(1);
                    arbi.DNI = dataReader.GetString(2);
                    arbi.Nombre = dataReader.GetString(3);
                    arbi.Apellidos = dataReader.GetString(4);
                    arbi.NumTelefono = dataReader.GetString(5);

                    listadoArbis.Add(arbi);
                }
                return listadoArbis;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al cargar los arbitros: " + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Devuelve una lista con todos los administradores de la base de datos
        /// </summary>
        /// <returns></returns>
        internal static List<CategoriaDTO> CargarTodasCategorias()
        {
            List<CategoriaDTO> listadoCategorias = new List<CategoriaDTO>();
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                CategoriaDTO categoria;
                cmd = new SqlCommand("SELECT IdCategoria, Descripcion, Abreviatura FROM Categoria", connection);
                cmd.CommandType = CommandType.Text;

                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto  
                {
                    categoria = new CategoriaDTO();

                    categoria.IdCategoria = dataReader.GetInt32(0);
                    categoria.Descripcion = dataReader.GetString(1);
                    categoria.Abreviatura = dataReader.GetString(2);

                    listadoCategorias.Add(categoria);
                }
                return listadoCategorias;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al cargar las categorias: " + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        /// <summary>
        /// Inserta el usuario dado por parametro en base de datos
        /// </summary>
        /// <param name="usuario"></param>
        internal static int CrearUsuario(UsuarioDTO usuario)
        {
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            try
            {
                //Prepara el comando
                cmd = new SqlCommand("INSERT INTO Usuario VALUES (@IdArbitro, @IdAdministrador , @Usuario , @Password , @IsAdmin)", connection);

                int? IdArbitro = usuario.IdArbitro;
                int? IdAdmin = usuario.IdAdministrador;

                if (IdArbitro == null)
                {
                    cmd.Parameters.AddWithValue("@IdArbitro", DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdAdministrador", IdAdmin);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@IdArbitro", IdArbitro);
                    cmd.Parameters.AddWithValue("@IdAdministrador", DBNull.Value);
                }
                cmd.Parameters.AddWithValue("@Usuario", usuario.Usuario);
                cmd.Parameters.AddWithValue("@Password", usuario.Password);
                cmd.Parameters.AddWithValue("@IsAdmin", usuario.IsAdmin);

                //Ejecuta el insert
                adapter = new SqlDataAdapter();
                adapter.InsertCommand = cmd;
                return adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al insertar usuario en BD:" + ex.Message + " EN " + ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (adapter != null)
                {
                    adapter.Dispose();
                }
            }
        }

        /// <summary>
        /// Inserta el arbitro dado por parametro en base de datos
        /// </summary>
        /// <param name="arbitro"></param>
        /// <returns></returns>
        internal static int CrearArbitro(ArbitroDTO arbitro)
        {
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            try
            {
                //Prepara el comando
                cmd = new SqlCommand("INSERT INTO Arbitro VALUES (@Alias, @DNI , @Nombre , @Apellidos , @NumTelefono)", connection);

                cmd.Parameters.AddWithValue("@Alias", arbitro.Alias.ToUpper() ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@DNI", arbitro.DNI ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Nombre", arbitro.Nombre ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Apellidos", arbitro.Apellidos ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NumTelefono", arbitro.NumTelefono ?? (object)DBNull.Value);

                //Ejecuta el insert
                adapter = new SqlDataAdapter();
                adapter.InsertCommand = cmd;
                return adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al insertar arbitro en BD:" + ex.Message + System.Environment.NewLine +
                    ex.StackTrace + System.Environment.NewLine + System.Environment.NewLine);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (adapter != null)
                {
                    adapter.Dispose();
                }
            }
        }

        /// <summary>
        /// Inserta el partido dado por parametro en base de datos
        /// </summary>
        /// <param name="partido"></param>
        /// <returns></returns>
        internal static int CrearPartido(PartidoDTO partido)
        {
            SqlCommand cmd = null;
            SqlDataAdapter adapter = null;
            try
            {
                //Prepara el comando
                cmd = new SqlCommand("INSERT INTO Partido VALUES (@Fecha, @Local , @Visitante , @IdCategoria , @Ubicacion, @Observaciones, null)", connection);

                cmd.Parameters.AddWithValue("@Fecha", partido.FechaPartido);
                cmd.Parameters.AddWithValue("@Local", partido.EquipoLocal ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Visitante", partido.EquipoVisitante ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IdCategoria", partido.IdCategoria);
                cmd.Parameters.AddWithValue("@Ubicacion", partido.Ubicacion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Observaciones", partido.Observaciones ?? (object)DBNull.Value);

                //Ejecuta el insert
                adapter = new SqlDataAdapter();
                adapter.InsertCommand = cmd;
                return adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al insertar parrtido en BD:" + ex.Message + System.Environment.NewLine +
                    ex.StackTrace + System.Environment.NewLine + System.Environment.NewLine);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (adapter != null)
                {
                    adapter.Dispose();
                }
            }
        }

        /// <summary>
        /// Obtiene el IdArbitro cuyo alias es igual al dado por parametro
        /// </summary>
        /// <param name="Alias"></param>
        /// <returns></returns>
        internal static int GetIdArbitroByAlias(string Alias)
        {
            int IdArbitro = 0;
            SqlCommand cmd = null;
            SqlDataReader dataReader = null;
            try
            {
                cmd = new SqlCommand("SELECT IdArbitro FROM Arbitro WHERE Alias like @Alias", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Alias", Alias);

                dataReader = cmd.ExecuteReader();
                while (dataReader.Read())//Si obtengo datos "mapeo(a mano)" el resultado a un objeto  
                {
                    IdArbitro = dataReader.GetInt32(0);
                }
                return IdArbitro;
            }
            catch (Exception ex)
            {
                Log.Log.LogMessageToFile("Error al consultar el id por alias: " + ex.Message + System.Environment.NewLine + ex.StackTrace);
                throw ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (dataReader != null)
                {
                    dataReader.Dispose();
                }
            }
        }

        #endregion
    }
}
