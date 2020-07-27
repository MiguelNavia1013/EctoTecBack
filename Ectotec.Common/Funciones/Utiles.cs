using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace Ectotec.Common.Funciones
{
    public class Utiles
    {
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            DataTable table = null;
            if (data != null && data.Count > 0)
            {
                PropertyDescriptorCollection properties =
                   TypeDescriptor.GetProperties(typeof(T));
                table = new DataTable();
                foreach (PropertyDescriptor prop in properties)
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? String.Empty;
                    table.Rows.Add(row);
                }
            }
            return table;

        }

        public static DateTime ConvertToTimeMexico(DateTime hwTime)
        {
            DateTime mxFechaHora = hwTime;
            try
            {
                TimeZoneInfo setTimeZoneInfo;
                DateTime currentDateTime = hwTime;

                //Ponemos la información de la zona horaria a Central Standard Time (México)
                setTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

                //Obtenemos la fecha y la hora estandard de México
                currentDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, setTimeZoneInfo);
                mxFechaHora = currentDateTime;

            }
            catch (TimeZoneNotFoundException)
            {
                Console.WriteLine("No se encontro el Id de zona horaria");
            }
            catch (InvalidTimeZoneException)
            {
                Console.WriteLine("El registro de zona horaria está corrupto");
            }
            return mxFechaHora;
        }

        public static DateTime ConvertToTimeDB(DateTime hwTime)
        {
            DateTime mxFechaHora = hwTime;
            try
            {
                TimeZoneInfo setTimeZoneInfo;
                DateTime currentDateTime = hwTime;

                //Ponemos la información de la zona horaria a Central Standard Time (México)
                setTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

                //Obtenemos la fecha y la hora estandard de México
                currentDateTime = TimeZoneInfo.ConvertTime(currentDateTime, setTimeZoneInfo);
                mxFechaHora = currentDateTime;

            }
            catch (TimeZoneNotFoundException)
            {
                Console.WriteLine("No se encontro el Id de zona horaria");
            }
            catch (InvalidTimeZoneException)
            {
                Console.WriteLine("El registro de zona horaria está corrupto");
            }
            return mxFechaHora;
        }

        public static string ConvertDateToString(DateTime fecha)
        {
            string respuesta = string.Empty;
            respuesta = fecha.ToString("yyyy/mm/dd: HH:MM");
            return respuesta;

        }

    }

}
