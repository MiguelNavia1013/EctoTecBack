using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Ectotec.Common.dbConnection
{
    public class BaseDataAccess : IDisposable
    {
        private Boolean disposed;

        protected string ConnectionString { get; set; }

        private SqlConnection mainConnection;
        private SqlConnection ReaderConnection;

        public BaseDataAccess()
        {
        }

        public BaseDataAccess(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            string Cadena = this.ConnectionString;

            mainConnection = new SqlConnection(Cadena);
            if (mainConnection.State != ConnectionState.Open)
                mainConnection.Open();


            return mainConnection;
        }

        private SqlCommand GetCommand(DbConnection connection, string commandText)
        {
            SqlCommand command = new SqlCommand();
            if (!string.IsNullOrEmpty(commandText))
            {
                command = new SqlCommand(commandText, connection as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
            }

            return command;
        }

        public SqlParameter GetParameter(string parameter, object value)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, value ?? DBNull.Value)
            {
                Direction = ParameterDirection.Input
            };
            return parameterObject;
        }

        public SqlParameter GetParameterOut(string parameter, SqlDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, type); ;

            if (type == SqlDbType.NVarChar || type == SqlDbType.VarChar || type == SqlDbType.NText || type == SqlDbType.Text)
            {
                parameterObject.Size = -1;
            }

            parameterObject.Direction = parameterDirection;

            if (value != null)
            {
                parameterObject.Value = value;
            }
            else
            {
                parameterObject.Value = DBNull.Value;
            }

            return parameterObject;
        }

        public int ExecuteNonQuery(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int returnValue = -1;

            using (SqlConnection connection = this.GetConnection())
            {
                DbCommand cmd = this.GetCommand(connection, procedureName);

                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                returnValue = cmd.ExecuteNonQuery();

                connection.Close();
            }

            return returnValue;
        }

        public string ExecuteScalar(string procedureName, List<SqlParameter> parameters)
        {
            object returnValue = null;

            using (DbConnection connection = this.GetConnection())
            {
                DbCommand cmd = this.GetCommand(connection, procedureName);

                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                returnValue = cmd.ExecuteScalar();

                connection.Close();

            }

            return returnValue.ToString();
        }

        public SqlDataReader GetDataReader(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            SqlDataReader ds;

            //using (
            ReaderConnection = this.GetConnection();
            //{
            SqlCommand cmd = this.GetCommand(ReaderConnection, procedureName);


            if (parameters != null && parameters.Count > 0)
                for (int i = 0; i < parameters.Count; i++)
                    cmd.Parameters.Add(parameters[i]);

            ds = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            //}            

            return ds;
        }

        public DataSet GetDataDS(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = this.GetConnection())
            {
                SqlCommand cmd = this.GetCommand(connection, procedureName);


                if (parameters != null && parameters.Count > 0)
                    for (int i = 0; i < parameters.Count; i++)
                        cmd.Parameters.Add(parameters[i]);


                SqlDataAdapter ad = new SqlDataAdapter(cmd);


                ad.Fill(ds);

                connection.Close();
            }

            return ds;
        }

        public DataTable GetDataDT(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            DataTable dt = new DataTable(); ;

            using (SqlConnection connection = this.GetConnection())
            {
                if (commandType == CommandType.Text)
                {
                    SqlCommand cmdText = this.GetCommand(connection, procedureName);
                    cmdText.CommandType = CommandType.Text;

                    if (parameters != null && parameters.Count > 0)
                        for (int i = 0; i < parameters.Count; i++)
                            cmdText.Parameters.Add(parameters[i]);


                    SqlDataAdapter ad = new SqlDataAdapter(cmdText);
                    ad.Fill(dt);
                }
                else
                {
                    SqlCommand cmd = this.GetCommand(connection, procedureName);
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null && parameters.Count > 0)
                        for (int i = 0; i < parameters.Count; i++)
                            cmd.Parameters.Add(parameters[i]);
                    //cmd.Parameters.AddRange(parameters.ToArray());
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.Fill(dt);
                }





                connection.Close();
            }

            return dt;
        }

        public void CloseConnection()
        {
            if (mainConnection.State != ConnectionState.Closed)
                mainConnection.Close();

            if (ReaderConnection != null)
            {
                if (ReaderConnection.State != ConnectionState.Closed)
                    ReaderConnection.Close();
            }
        }

        /// <summary>
        /// Implementación de IDisposable. No se sobreescribe.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            // GC.SupressFinalize quita de la cola de finalización al objeto.
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Limpia los recursos manejados y no manejados.
        /// </summary>
        /// <param name="disposing">
        /// Si es true, el método es llamado directamente o indirectamente
        /// desde el código del usuario.
        /// Si es false, el método es llamado por el finalizador
        /// y sólo los recursos no manejados son finalizados.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // Preguntamos si Dispose ya fue llamado.
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Llamamos al Dispose de todos los RECURSOS MANEJADOS.
                    this.Dispose();
                }

                // Acá finalizamos correctamente los RECURSOS NO MANEJADOS
                // ...

            }
            this.disposed = true;
        }
    }
}
