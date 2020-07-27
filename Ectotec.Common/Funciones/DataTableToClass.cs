using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

namespace Ectotec.Common.Funciones
{
    public static class DataTableToClass
    {
        public static List<T> DataTableToList<T>(this DataTable table) where T : new()
        {
            List<T> list = new List<T>();
            foreach (var row in table.AsEnumerable())
            {
                var entity = new T();
                entity.InjectFrom<ReaderInjection>(row);
                list.Add(entity);
            }

            return list;
        }
    }

    public class ReaderInjection : KnownSourceInjection<DataRow>
    {
        protected override void Inject(DataRow source, object target)
        {
            for (var i = 0; i < source.Table.Columns.Count; i++)
            {
                var activeTarget = target.GetType().GetProperty(source.Table.Columns[i].ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (activeTarget == null) continue;

                var value = source[source.Table.Columns[i].ColumnName];
                
                try
                {
                    activeTarget.SetValue(target, Convert.ChangeType(value, activeTarget.PropertyType, CultureInfo.CurrentCulture), null);
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}