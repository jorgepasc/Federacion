using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Federacion.Datos
{
    public static class UsuarioActivoDTO
    {
        private static Int32 _IdUsuario;
        private static Int32 _IdArbitro;
        private static Int32 _IdAdministrador;
        private static string _Usuario;
        private static string _Password;
        private static bool _IsAdmin;

        public static Int32 IdUsuario { get; set; }
        public static Int32 IdArbitro { get; set; }
        public static Int32 IdAdministrador { get; set; }
        public static string Usuario { get; set; }
        public static string Password { get; set; }
        public static bool IsAdmin { get; set; }

        /// <summary>
        /// Si el usuario elige "recordarse" en el login guardo los datos del usuario en un fichero.
        /// Al iniciar la aplicacion lo leo y si tiene datos me salto el login (y se los asigno al usuario activo)
        /// </summary>
        public static bool LeerRecuerdame()
        {
            try
            {
                //Si ha elegido "recuerdame" no le enviamos al login, le enviamos al inicio
                string path = GetRutaRecuerdame();

                if (File.Exists(path)) //comprueba si se ha escrito algo en el fichero
                {
                    string textoFichero = File.ReadAllText(path);
                    textoFichero = textoFichero.Trim();
                    if (textoFichero != null && !String.IsNullOrEmpty(textoFichero))
                    {
                        //Si hay datos en el fichero los separamos (los hemos escrito separado por ':' ej. 1:2:0:Jorge:Pasc:false)
                        string[] valoresUsuarioActivo = textoFichero.Split(':');
                        IdUsuario = Convert.ToInt32(valoresUsuarioActivo[0]);
                        IdArbitro = Convert.ToInt32(valoresUsuarioActivo[1]);
                        IdAdministrador = Convert.ToInt32(valoresUsuarioActivo[2]);
                        Usuario = valoresUsuarioActivo[3];
                        Password = valoresUsuarioActivo[4];
                        IsAdmin = Convert.ToBoolean(valoresUsuarioActivo[5]);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// Si el usuario elige "recordarse" en el login guardo los datos del usuario en un fichero de texto
        /// </summary>
        public static void EscribirRecuerdame()
        {
            StreamWriter sw = null;
            try
            {
                sw = File.AppendText(GetRutaRecuerdame());
                String line = String.Format(
                        "{0}:{1}:{2}:{3}:{4}:{5}", IdUsuario, IdArbitro, IdAdministrador, Usuario, Password, IsAdmin);
                sw.WriteLine(line);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }

        }

        /// <summary>
        /// Obtiene la ruta donde se guardan los datos para recordar al usuario
        /// </summary>
        /// <returns></returns>
        public static string GetRutaRecuerdame()
        {
            try
            {
                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (!path.EndsWith("\\")) path += "\\recuerdame.txt";                
                return path;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
