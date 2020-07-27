using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ectotec.Business;
using Ectotec.Common.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace EctoTecWepApi.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        IConfiguration Configuration { get; }
        private readonly IEmailSender _emailSender;

        public HomeController(IEmailSender emailSender, IConfiguration paramConfig)
        {
            Configuration = paramConfig;
            _emailSender = emailSender;
        }

        #region guardarDatosPersonales
        [HttpGet]
        [Route("guardarDatosPersonales")]
        public IActionResult GuardarDatosPersonales(string nombre, string email, string telefono, string fecha, string ciudadEstado)
        {
            MessageResponse<int> messageResponse = new MessageResponse<int>();
            try
            {
                Business business = new Business(new JWTNetCore.Helpers.GetConnection(Configuration).GetConnectionDefault);
                int idUser = business.guardarDatosPersonales(nombre, email, telefono, fecha, ciudadEstado);
                messageResponse.response = idUser;
                return Ok(messageResponse);
            }
            catch (System.Exception ex)
            {
                return BadRequest(messageResponse = new MessageResponse<int> { success = false, errorMessage = ex.Message, response = 0 });
            }
        }
        #endregion

        #region sendEmail
        [Route("sendEmail")]
        [HttpGet]
        public async Task<IActionResult> SendEmail(int idUser)
        {
            MessageResponse<DatosPersonales> messageResponse = new MessageResponse<DatosPersonales>();
            Business business = new Business(new JWTNetCore.Helpers.GetConnection(Configuration).GetConnectionDefault);
            try
            {
                DatosPersonales datos = new DatosPersonales();
                datos = business.getUserById(idUser);
                if (datos != null)
                {
                    await _emailSender.SendEmailAsync(datos.email, "Registro Exitoso: " + datos.nombre.ToUpper(),
                     $"<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" /> <title>Invitacion evento</title> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/> </head> <body style=\"margin: 0; padding: 0;\"> <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\"> <tr style=\"padding: 20px 0 30px 0;\"> <td align=\"left\" bgcolor=\"#06e107\" style=\"padding: 40px 0 30px 0; border-radius: 30px; color: #ffffff; padding: 40px 30px 40px 30px; font-size: 40px;\"> <b> Green Leaves </b> <img align=\"right\" src=\"https://images.vexels.com/media/users/3/207136/isolated/lists/dc6980a67acd5e2d4a13bc446e9e3378-icono-grande-de-hoja-verde.png\" alt=\"Imagen\" width=\"100\" height=\"100\" style=\"display: block;\" /> </td> </tr>  <tr> <td bgcolor=\"#ffffff\" style=\"padding: 40px 30px 40px 30px;\"> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"> <tr> <td colspan=\"2\" align=\"left\"> <b style=\"color: #153643; font-family: Arial, sans-serif; font-size: 20px; line-height: 20px;\"> Estimado {datos.nombre} </b> </td> </tr> <tr> <td colspan=\"2\" style=\"padding: 20px 0 5px 0;\"> <b>Hemos recibido sus datos y nos pondremos en contacto con usted en la brevedad posible. Enviaremos un correo con información a su cuenta:</b> {datos.email} </td> </tr> <tr> <td align=\"right\"> <b>Atte.</b> </td> </tr> <tr> <td style=\"color: #06e107;\" align=\"right\"> <b>Green Leaves</b> </td> </tr> <tr> <td align=\"right\"> <b> {datos.ciudadEstado} a {datos.fecha} </b> </td> </tr> </table> </td> </tr> </body> </html>");
                    return Ok(new MessageResponse<string> { success = true, errorMessage = "", response = "" });
                }
                else
                {
                    return Ok(new MessageResponse<string> { success = false, errorMessage = "", response = "" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(messageResponse = new MessageResponse<DatosPersonales> { success = false, errorMessage = ex.Message });
            }
        }
        #endregion

        #region GetCatalogoCiudadEstado
        [Route("GetCatalogoCiudadEstado")]
        [HttpGet]
        public IActionResult GetCatalogoCiudadEstado()
        {
            MessageResponse<List<string>> respuesta = new MessageResponse<List<string>>();
            try
            {
                Business business = new Business(new JWTNetCore.Helpers.GetConnection(Configuration).GetConnectionDefault);
                respuesta.response = business.GetCatalogoCiudadEstado();
                if (respuesta.response == null || respuesta.response.Count == 0)
                {
                    respuesta.success = false;
                    respuesta.errorMessage = "";
                }
                else
                {
                    respuesta.success = true;
                    respuesta.errorMessage = "Ok";
                }
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new MessageResponse<List<string>> { success = false, errorMessage = ex.Message, response = null };
                return BadRequest(respuesta);
            }
        }
        #endregion
    }
}
