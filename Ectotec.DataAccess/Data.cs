using System;
using Ectotec.Common.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Ectotec.Common.dbConnection;

namespace Ectotec.DataAccess
{
    public class Data
    {
        string connStr { get; set; }

        public Data(string parStrConn) => connStr = parStrConn;

        #region guardarDatosPersonales
        public int guardarDatosPersonales(string nombre, string email, string telefono, string fecha, string ciudadEstado)
        {
            BaseDataAccess db = new BaseDataAccess(connStr);
            List<SqlParameter> param = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName="@nombre",SqlDbType = SqlDbType.VarChar,Value = nombre, IsNullable = false,Direction=ParameterDirection.Input},
                    new SqlParameter() {ParameterName="@email",SqlDbType = SqlDbType.VarChar,Value = email, IsNullable = false,Direction=ParameterDirection.Input},
                    new SqlParameter() {ParameterName="@telefono",SqlDbType = SqlDbType.VarChar,Value = telefono, IsNullable = false,Direction=ParameterDirection.Input},
                    new SqlParameter() {ParameterName="@fecha",SqlDbType = SqlDbType.VarChar,Value = fecha, IsNullable = false,Direction=ParameterDirection.Input},
                    new SqlParameter() {ParameterName="@ciudadEstado",SqlDbType = SqlDbType.VarChar,Value = ciudadEstado, IsNullable = false,Direction=ParameterDirection.Input}
                };
            int resultado = Convert.ToInt32(db.ExecuteScalar("InsertDatosPersonales", param));
            db.CloseConnection();
            return resultado;
        }
        #endregion

        #region "getUserById"
        public DatosPersonales getUserById(int idUser)
        {
            List<DatosPersonales> datos = new List<DatosPersonales>();
            BaseDataAccess db = new BaseDataAccess(connStr);
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@idUser", SqlDbType = SqlDbType.Int, Value = idUser, IsNullable = false, Direction = ParameterDirection.Input }
            };
            DataTable dt = db.GetDataDT("getUserById", param, CommandType.StoredProcedure);

            datos = dt.AsEnumerable().Select(c => new DatosPersonales()
            {
                idUser = c.Field<int>("idUser"),
                nombre = c.Field<string>("nombre"),
                email = c.Field<string>("email"),
                telefono = c.Field<string>("telefono"),
                fecha = c.Field<string>("fecha"),
                ciudadEstado = c.Field<string>("ciudadEstado")
            }).ToList();
            return datos.Count > 0 ? datos.First() : null;
        }
        #endregion

        #region GetCatalogoCiudadEstado
        public List<string> GetCatalogoCiudadEstado()
        {
            List<string> respuesta = new List<string>();
            BaseDataAccess db = new BaseDataAccess(connStr);
            using (SqlDataReader dr = db.GetDataReader("getCatCiudadesEstado", null, CommandType.StoredProcedure))
            {
                string strCiudad = "";
                while (dr.Read())
                {
                    strCiudad = dr[0] != null ? dr[0].ToString().Trim() : string.Empty;
                    respuesta.Add(strCiudad);
                }
            }
            return respuesta;
        }
        #endregion
    }
}
