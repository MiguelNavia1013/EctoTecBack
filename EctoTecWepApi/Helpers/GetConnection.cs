using Microsoft.Extensions.Configuration;

namespace JWTNetCore.Helpers
{
    /// <summary>
    /// Get Connection
    /// </summary>
    public class GetConnection
    {

        private static IConfiguration Configuration { get; set; }

        public GetConnection(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GetConnectionDefault
        {
            get
            {
                string strConn = Configuration["ConnectionStrings:DefaultConnection"];
                return strConn;
            }
        }

    }
}