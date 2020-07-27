using System;
using System.Collections.Generic;
using System.Text;

namespace Ectotec.Common.Model
{
    public class DatosPersonales
    {
        public int? idUser { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string fecha { get; set; }
        public string ciudadEstado { get; set; }
    }
}
