using System;
using System.Collections.Generic;
using System.Text;

namespace Federacion.Helpers
{
    static class DateTimeHelper
    {
        /// <summary>
        /// Obtiene la fecha del lunes siguiente a la fecha dada por parametro
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns>Fecha del lunes</returns>
        public static DateTime GetDateSiguienteLunes(this DateTime fecha)
        {
            DateTime fechaAnterior = GetDateAnteriorLunes(fecha);
            while (fecha.DayOfWeek != DayOfWeek.Monday || fecha.Date == fechaAnterior.Date)
                fecha = fecha.AddDays(1);            
            return fecha;
        }

        /// <summary>
        /// Obtiene la fecha del lunes siguiente a la fecha dada por parametro
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns>Fecha del lunes</returns>
        public static DateTime GetDateAnteriorLunes(this DateTime fecha)
        {
            while (fecha.DayOfWeek != DayOfWeek.Monday)
                fecha = fecha.AddDays(-1);
            return fecha;
        }

        /// <summary>
        /// Obtiene el primer día del mes de la fecha dada por parametro
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfMonth(this DateTime fecha)
        {
            return new DateTime(fecha.Year, fecha.Month, 1);
        }

        /// <summary>
        /// Devuelve el último dia del mes de la fecha dada por parametro
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime fecha)
        {
            return fecha.GetFirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Septiembre es el mes en el que inicia la temporada
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static DateTime GetLast1Septiembre(this DateTime fecha)
        {
            while (fecha.Month != 9)
                fecha = fecha.AddMonths(-1);
            return new DateTime(fecha.Year, fecha.Month, 1);
        }
    }
}
