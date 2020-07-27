using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTNetCore.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; }
        public bool FirstTimeAccess { get; set; }
        public int? IdImagenPerfil { get; set; }
        public int? idRolTMP { get; set; }
    }
}
