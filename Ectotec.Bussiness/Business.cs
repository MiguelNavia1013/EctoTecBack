using System;
using Ectotec.DataAccess;
using Ectotec.Common.Model;
using System.Collections.Generic;

namespace Ectotec.Business
{
    public class Business
    {
        string strConn { get; }

        public Business(string paramConfig) => strConn = paramConfig;

        #region guardarDatosPersonales
        public int guardarDatosPersonales(string nombre, string email, string telefono, string fecha, string ciudadEstado)
        {
            Data da = new Data(strConn);
            return da.guardarDatosPersonales(nombre, email, telefono, fecha, ciudadEstado);
        }
        #endregion

        #region getUsuario
        public DatosPersonales getUserById(int iduser)
        {
            Data da = new Data(strConn);
            return da.getUserById(iduser);
        }
        #endregion

        #region GetCatalogoCiudadEstado
        public List<string> GetCatalogoCiudadEstado()
        {
            Data da = new Data(strConn);
            return da.GetCatalogoCiudadEstado();
        }
        #endregion
    }
}
